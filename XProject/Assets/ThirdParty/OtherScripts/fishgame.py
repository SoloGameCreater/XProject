-- ============================================================
-- 钓鱼小游戏 MVP —— 对标 Far Cry 5 PRD (M1 闭环)
-- 脚本类型：LocalScript（放入 StarterPlayerScripts）
-- 核心机制：收线消耗鱼体力 → 体力归零 = 上鱼
--           张力超限 = 断线失败
--           鱼冲刺方向反向拉 = 张力暴增
-- ============================================================

local Players         = game:GetService("Players")
local UserInputService = game:GetService("UserInputService")
local RunService       = game:GetService("RunService")
local TweenService     = game:GetService("TweenService")

local player = Players.LocalPlayer
local mouse  = player:GetMouse()

-- ============================================================
-- 1. 数据配置（对应 PRD §7.1 / §7.2 / §7.6）
-- ============================================================

-- 鱼竿配置（PRD §7.1）
local ROD_CONFIG = {
	BasicRod = {
		name             = "Basic Rod",
		tensionMax       = 60,   -- 断线阈值（0-100）
		fatigueMultiplier= 1.0,  -- 鱼疲劳速度倍率
		reelSpeed        = 1.5,  -- 收线速度 m/s
	},
	NaturalRod = {
		name             = "Natural Rod",
		tensionMax       = 75,
		fatigueMultiplier= 1.2,
		reelSpeed        = 1.8,
	},
	WonderboyRod = {
		name             = "Wonderboy Rod",
		tensionMax       = 90,
		fatigueMultiplier= 1.5,
		reelSpeed        = 2.0,
	},
	OldBetsy = {
		name             = "Old Betsy",
		tensionMax       = 100,
		fatigueMultiplier= 2.0,
		reelSpeed        = 2.5,
	},
}

-- 鱼饵配置（PRD §7.2）
local LURE_CONFIG = {
	{ id="BasicMayfly",      name="Basic Mayfly",      targetTags={"any"},      unlocked=true  },
	{ id="BassPopper",       name="Bass Popper",        targetTags={"bass"},     unlocked=false },
	{ id="HybridSturgeon",   name="Hybrid Sturgeon",    targetTags={"sturgeon"}, unlocked=false },
	{ id="SalmonEgg",        name="Salmon Egg",         targetTags={"salmon"},   unlocked=false },
	{ id="TroutBugger",      name="Trout Bugger",       targetTags={"trout"},    unlocked=false },
}

-- 鱼种配置（PRD §7.6 + §9.1）
-- stamina：鱼的体力上限（越大越难钓）
-- fightPower：每秒给张力施加的压力
-- sprintInterval：冲刺间隔（秒）
-- sprintDuration：冲刺持续时间
-- weightRange：重量随机范围 {min, max}（kg）
-- lureTag：推荐鱼饵标签（命中时 biteChance 翻倍）
-- rarity：common / uncommon / rare / legendary
local FISH_CONFIG = {
	{ species="Rock Bass",         rarity="common",    stamina=40,  fightPower=15, sprintInterval=6,  sprintDuration=1.5, weightRange={0.3,1.2},  lureTag="bass",     sellPricePerKg=5  },
	{ species="Largemouth Bass",   rarity="common",    stamina=60,  fightPower=20, sprintInterval=5,  sprintDuration=2.0, weightRange={0.8,3.0},  lureTag="bass",     sellPricePerKg=8  },
	{ species="Smallmouth Bass",   rarity="common",    stamina=55,  fightPower=18, sprintInterval=5,  sprintDuration=1.8, weightRange={0.5,2.5},  lureTag="bass",     sellPricePerKg=7  },
	{ species="Rainbow Trout",     rarity="common",    stamina=50,  fightPower=22, sprintInterval=4,  sprintDuration=2.0, weightRange={0.4,2.0},  lureTag="trout",    sellPricePerKg=9  },
	{ species="Bull Trout",        rarity="uncommon",  stamina=80,  fightPower=28, sprintInterval=4,  sprintDuration=2.5, weightRange={1.0,5.0},  lureTag="trout",    sellPricePerKg=12 },
	{ species="Lake Trout",        rarity="uncommon",  stamina=85,  fightPower=30, sprintInterval=4,  sprintDuration=2.5, weightRange={1.5,6.0},  lureTag="trout",    sellPricePerKg=13 },
	{ species="Golden Trout",      rarity="rare",      stamina=100, fightPower=35, sprintInterval=3,  sprintDuration=3.0, weightRange={0.5,2.5},  lureTag="trout",    sellPricePerKg=25 },
	{ species="Kokanee Salmon",    rarity="uncommon",  stamina=75,  fightPower=26, sprintInterval=4,  sprintDuration=2.0, weightRange={0.8,3.5},  lureTag="salmon",   sellPricePerKg=11 },
	{ species="Chinook Salmon",    rarity="rare",      stamina=120, fightPower=40, sprintInterval=3,  sprintDuration=3.5, weightRange={3.0,12.0}, lureTag="salmon",   sellPricePerKg=20 },
	{ species="Arctic Grayling",   rarity="uncommon",  stamina=65,  fightPower=24, sprintInterval=5,  sprintDuration=2.0, weightRange={0.5,2.0},  lureTag="salmon",   sellPricePerKg=10 },
	{ species="Paddlefish Sturgeon",rarity="rare",     stamina=150, fightPower=45, sprintInterval=4,  sprintDuration=4.0, weightRange={5.0,20.0}, lureTag="sturgeon", sellPricePerKg=22 },
	{ species="Pallid Sturgeon",   rarity="legendary", stamina=200, fightPower=55, sprintInterval=3,  sprintDuration=5.0, weightRange={8.0,30.0}, lureTag="sturgeon", sellPricePerKg=35 },
}

-- 稀有度权重（PRD §7.3 难度分级）
local RARITY_WEIGHT = { common=55, uncommon=30, rare=12, legendary=3 }

-- ============================================================
-- 2. 状态枚举
-- ============================================================
local FishingState = {
	IDLE      = "idle",
	CASTING   = "casting",
	WAITING   = "waiting",   -- 等待咬钩
	FIGHTING  = "fighting",  -- 遛鱼阶段（收线/放线/鱼冲刺）
}

-- ============================================================
-- 3. 运行时状态
-- ============================================================
local state = FishingState.IDLE

-- 玩家当前装备
local equippedRod  = ROD_CONFIG.BasicRod
local activeLureIdx = 1   -- LURE_CONFIG 索引

-- 遛鱼数据
local fight = {
	fish         = nil,    -- 当前鱼数据
	weight       = 0,      -- 本次鱼重（kg）
	stamina      = 0,      -- 鱼当前体力（0=疲劳，可上鱼）
	staminaMax   = 0,
	tension      = 0,      -- 线张力（0-100），超过 rod.tensionMax = 断线
	lineLength   = 20,     -- 当前线长（m）
	lineLengthMax= 30,
	lineLengthMin= 1.5,    -- 达到此值且 stamina=0 → 上鱼
	reeling      = false,  -- 是否正在收线（左键按住）
	releasing    = false,  -- 是否正在放线（右键按住）
	-- 鱼冲刺状态
	isSprinting  = false,
	sprintDir    = 0,      -- 1=向外冲, -1=向内冲（极罕见）
	sprintTimer  = 0,
	sprintCooldown = 0,
}

-- 背包（已钓到的鱼）
local inventory = {}
local totalScore = 0

-- ============================================================
-- 4. 工具函数
-- ============================================================

-- 按权重随机选择稀有度
local function pickRarity()
	local total = 0
	for _, w in pairs(RARITY_WEIGHT) do total = total + w end
	local r = math.random() * total
	local acc = 0
	for rarity, w in pairs(RARITY_WEIGHT) do
		acc = acc + w
		if r <= acc then return rarity end
	end
	return "common"
end

-- 根据鱼饵和稀有度选鱼
local function pickFish()
	local rarity = pickRarity()
	local lure   = LURE_CONFIG[activeLureIdx]

	-- 先筛同稀有度
	local pool = {}
	for _, f in ipairs(FISH_CONFIG) do
		if f.rarity == rarity then
			table.insert(pool, f)
		end
	end
	if #pool == 0 then pool = FISH_CONFIG end

	-- 鱼饵匹配度：命中 lureTag 的鱼权重 ×3
	local weightedPool = {}
	for _, f in ipairs(pool) do
		local w = 1
		if lure.targetTags[1] == "any" or f.lureTag == lure.targetTags[1] then
			w = 3
		end
		for _ = 1, w do
			table.insert(weightedPool, f)
		end
	end

	return weightedPool[math.random(1, #weightedPool)]
end

-- 随机鱼重
local function randomWeight(fishData)
	local lo, hi = fishData.weightRange[1], fishData.weightRange[2]
	return math.floor((lo + math.random() * (hi - lo)) * 10) / 10
end

-- 稀有度中文标签
local RARITY_LABEL = { common="普通", uncommon="稀有", rare="罕见", legendary="传说" }

-- ============================================================
-- 5. UI 系统
-- ============================================================
-- 使用简单的 ScreenGui 动态创建，避免依赖外部资源

local gui, panels = nil, {}

local function ensureGui()
	local pg = player:WaitForChild("PlayerGui")
	gui = pg:FindFirstChild("FishingUI")
	if not gui then
		gui = Instance.new("ScreenGui")
		gui.Name = "FishingUI"
		gui.ResetOnSpawn = false
		gui.Parent = pg
	end
end

-- 通用文本标签工厂
local function makeLabel(name, size, pos, parent)
	local lbl = parent:FindFirstChild(name)
	if lbl then return lbl end
	lbl = Instance.new("TextLabel")
	lbl.Name            = name
	lbl.Size            = size
	lbl.Position        = pos
	lbl.BackgroundColor3= Color3.new(0,0,0)
	lbl.BackgroundTransparency = 0.55
	lbl.TextColor3      = Color3.new(1,1,1)
	lbl.TextScaled      = true
	lbl.Font            = Enum.Font.GothamBold
	lbl.TextXAlignment  = Enum.TextXAlignment.Left
	lbl.Parent          = parent
	return lbl
end

-- 进度条工厂
local function makeBar(name, color, pos, parent)
	local frame = parent:FindFirstChild(name)
	if frame then return frame end
	-- 背景
	local bg = Instance.new("Frame")
	bg.Name             = name
	bg.Size             = UDim2.new(0, 220, 0, 14)
	bg.Position         = pos
	bg.BackgroundColor3 = Color3.new(0.15,0.15,0.15)
	bg.BorderSizePixel  = 0
	bg.Parent           = parent
	-- 前景
	local fg = Instance.new("Frame")
	fg.Name             = "Fill"
	fg.Size             = UDim2.new(1, 0, 1, 0)
	fg.BackgroundColor3 = color
	fg.BorderSizePixel  = 0
	fg.Parent           = bg
	return bg
end

-- 更新进度条填充比例
local function setBar(barFrame, ratio)
	ratio = math.clamp(ratio, 0, 1)
	local fill = barFrame:FindFirstChild("Fill")
	if fill then fill.Size = UDim2.new(ratio, 0, 1, 0) end
end

-- 通知浮层
local function notify(msg, duration, color)
	ensureGui()
	local lbl = gui:FindFirstChild("_Notify")
	if not lbl then
		lbl = Instance.new("TextLabel")
		lbl.Name               = "_Notify"
		lbl.Size               = UDim2.new(0, 400, 0, 50)
		lbl.Position           = UDim2.new(0.5, -200, 0.22, 0)
		lbl.BackgroundColor3   = Color3.new(0,0,0)
		lbl.BackgroundTransparency = 0.4
		lbl.TextScaled         = true
		lbl.Font               = Enum.Font.GothamBold
		lbl.TextColor3         = Color3.new(1,1,1)
		lbl.TextXAlignment     = Enum.TextXAlignment.Center
		lbl.Visible            = false
		lbl.Parent             = gui
	end
	lbl.Text      = msg
	lbl.TextColor3= color or Color3.new(1,1,1)
	lbl.Visible   = true
	task.spawn(function()
		task.wait(duration or 2)
		if lbl.Text == msg then lbl.Visible = false end
	end)
end

-- 构建主 HUD（只在第一次调用时创建）
local hudFrame = nil
local staminaBar, tensionBar, lineBar = nil, nil, nil
local lblState, lblHint, lblWeight, lblScore, lblLineDist = nil, nil, nil, nil, nil

local function buildHUD()
	ensureGui()
	if gui:FindFirstChild("HUD") then return end

	hudFrame = Instance.new("Frame")
	hudFrame.Name               = "HUD"
	hudFrame.Size               = UDim2.new(0, 240, 0, 260)  -- 加高容纳线长条
	hudFrame.Position           = UDim2.new(0, 12, 0, 12)
	hudFrame.BackgroundTransparency = 1
	hudFrame.Parent             = gui

	lblState  = makeLabel("lblState",  UDim2.new(1,0,0,28), UDim2.new(0,0,0,0),   hudFrame)
	lblWeight = makeLabel("lblWeight", UDim2.new(1,0,0,24), UDim2.new(0,0,0,32),  hudFrame)
	lblScore  = makeLabel("lblScore",  UDim2.new(1,0,0,24), UDim2.new(0,0,0,60),  hudFrame)

	-- 体力条（绿色）
	makeLabel("lblStamLbl", UDim2.new(1,0,0,18), UDim2.new(0,0,0,92), hudFrame).Text = "鱼体力"
	staminaBar = makeBar("StaminaBar", Color3.fromRGB(80,200,80),  UDim2.new(0,0,0,112), hudFrame)

	-- 张力条（红色）
	makeLabel("lblTensLbl", UDim2.new(1,0,0,18), UDim2.new(0,0,0,132), hudFrame).Text = "线张力"
	tensionBar = makeBar("TensionBar", Color3.fromRGB(220,60,60),  UDim2.new(0,0,0,152), hudFrame)

	-- 线长条（蓝色，从右往左填充：越满=线越短=越接近上鱼）
	-- 标签右侧附上实时米数
	lblLineDist = makeLabel("lblLineDist", UDim2.new(1,0,0,18), UDim2.new(0,0,0,176), hudFrame)
	lblLineDist.Text = "收线距离  20.0 m"
	lineBar = makeBar("LineBar", Color3.fromRGB(80,160,255), UDim2.new(0,0,0,196), hudFrame)
	-- 在进度条右端加"上鱼"小标签，让玩家知道满格=上鱼
	local lblLineEnd = Instance.new("TextLabel")
	lblLineEnd.Size               = UDim2.new(0,30,0,14)
	lblLineEnd.Position           = UDim2.new(1,2,0,196)
	lblLineEnd.BackgroundTransparency = 1
	lblLineEnd.TextColor3         = Color3.fromRGB(80,200,255)
	lblLineEnd.Text               = "上鱼"
	lblLineEnd.TextScaled         = true
	lblLineEnd.Font               = Enum.Font.GothamBold
	lblLineEnd.Parent             = hudFrame

	lblHint = makeLabel("lblHint", UDim2.new(0,420,0,44),
		UDim2.new(0.5,-210,0.9,0), gui)
	lblHint.TextXAlignment = Enum.TextXAlignment.Center
	lblHint.BackgroundTransparency = 0.35
end

-- 刷新 HUD 每帧调用
local function refreshHUD()
	if not hudFrame then return end

	-- 状态文本
	local stateText = {
		[FishingState.IDLE]     = "🎣  空闲  （左键抛竿）",
		[FishingState.CASTING]  = "⬆  抛竿中...",
		[FishingState.WAITING]  = "🌊  等待咬钩...",
		[FishingState.FIGHTING] = "⚡  遛鱼中！",
	}
	lblState.Text = stateText[state] or ""

	-- 装备信息
	local lureUnlocked = LURE_CONFIG[activeLureIdx].unlocked
	local lureDisplay  = lureUnlocked and LURE_CONFIG[activeLureIdx].name or "（未解锁）"
	lblWeight.Text  = string.format("鱼竿：%s  |  鱼饵：%s",
		equippedRod.name, lureDisplay)
	lblScore.Text   = "得分：" .. totalScore

	-- 进度条
	if state == FishingState.FIGHTING then
		setBar(staminaBar, fight.stamina / fight.staminaMax)
		local tensionRatio = fight.tension / equippedRod.tensionMax
		setBar(tensionBar, tensionRatio)

		-- 线长进度条：从 0（线最长）到 1（线最短=上鱼）反向显示
		local lineLengthMax = fight.lineLengthMax
		local lineLengthMin = fight.lineLengthMin
		local lineRatio = 1 - math.clamp(
			(fight.lineLength - lineLengthMin) / (lineLengthMax - lineLengthMin), 0, 1)
		setBar(lineBar, lineRatio)

		-- 线长条在体力归零后高亮为金色，提示"继续收线即可上鱼"
		local lineFill = lineBar:FindFirstChild("Fill")
		if lineFill then
			if fight.stamina <= 0 then
				lineFill.BackgroundColor3 = Color3.fromRGB(255,210,0)  -- 金色：体力耗尽，继续收线！
			else
				lineFill.BackgroundColor3 = Color3.fromRGB(80,160,255) -- 蓝色：正常收线中
			end
		end

		-- 实时线长数字
		if lblLineDist then
			lblLineDist.Text = string.format("收线距离  %.1f m → 上鱼需 %.1f m",
				fight.lineLength, lineLengthMin)
		end

		-- 张力条变色
		local fill = tensionBar:FindFirstChild("Fill")
		if fill then
			if tensionRatio < 0.6 then
				fill.BackgroundColor3 = Color3.fromRGB(80,200,80)
			elseif tensionRatio < 0.85 then
				fill.BackgroundColor3 = Color3.fromRGB(230,180,0)
			else
				fill.BackgroundColor3 = Color3.fromRGB(220,50,50)
			end
		end

		-- 操作提示：优先级 冲刺 > 体力归零 > 普通
		if fight.isSprinting then
			lblHint.Text       = "🔴  鱼在冲刺！放线降低张力 [右键按住]"
			lblHint.TextColor3 = Color3.fromRGB(255,100,100)
		elseif fight.stamina <= 0 then
			lblHint.Text       = "✨  鱼已疲劳！继续收线 [左键按住] 即可上鱼！"
			lblHint.TextColor3 = Color3.fromRGB(255,220,50)
		else
			lblHint.Text       = "[左键按住] 收线消耗体力  |  [右键按住] 放线降张力"
			lblHint.TextColor3 = Color3.fromRGB(255,255,255)
		end
		hudFrame.Visible = true
	else
		setBar(staminaBar, 0)
		setBar(tensionBar, 0)
		if lineBar then setBar(lineBar, 0) end
		if lblLineDist then lblLineDist.Text = "收线距离  --" end
		lblHint.Text = (state == FishingState.IDLE) and
			"[左键] 对准水面抛竿  |  [Q/E] 切换鱼饵" or ""
		lblHint.TextColor3 = Color3.fromRGB(255,255,255)
	end
end

-- ============================================================
-- 6. 核心钓鱼逻辑
-- ============================================================

-- 6.1 开始咬钩：初始化遛鱼数据
local function startFight(fishData, weight)
	fight.fish        = fishData
	fight.weight      = weight
	fight.staminaMax  = fishData.stamina * (weight / fishData.weightRange[2]) -- 越重体力越高
	fight.stamina     = fight.staminaMax
	fight.tension     = 0
	fight.lineLength  = 20
	fight.reeling     = false
	fight.releasing   = false
	fight.isSprinting = false
	fight.sprintDir   = 0
	fight.sprintTimer  = 0
	fight.sprintCooldown = fishData.sprintInterval + math.random(-1, 2)
	state = FishingState.FIGHTING

	local label = RARITY_LABEL[fishData.rarity] or "普通"
	notify(string.format("🐟  咬钩！%s（%s，%.1f kg）\n收线消耗体力，放线降张力！",
		fishData.species, label, weight), 3,
		fight.fish.rarity == "legendary"
			and Color3.fromRGB(180,100,255)
			or  Color3.fromRGB(255,220,50))
end

-- 6.2 钓鱼成功
local function onCatch()
	local f = fight.fish
	local w = fight.weight
	local price = math.floor(w * f.sellPricePerKg)
	local pts   = price

	table.insert(inventory, { species=f.species, weight=w, price=price })
	totalScore = totalScore + pts

	notify(string.format("✅  上鱼！%s  %.1f kg\n价值 $%d  | 得分 +%d",
		f.species, w, price, pts), 4,
		Color3.fromRGB(80,255,120))

	-- 重置
	state = FishingState.IDLE
	fight.fish = nil
end

-- 6.3 钓鱼失败
local function onFail(reason)
	notify("❌  " .. reason, 3, Color3.fromRGB(255,80,80))
	state = FishingState.IDLE
	fight.fish = nil
	fight.tension = 0
	fight.stamina = 0
end

-- 6.4 抛竿
local function castLine()
	if state ~= FishingState.IDLE then return end

	state = FishingState.CASTING
	notify("🌊  抛竿中...", 1)
	task.wait(0.8)

	-- 检查是否命中有效水面（简化：只要鼠标 Hit.Y < 1 视为水面）
	local hitPos = mouse.Hit.Position
	if hitPos.Y > 2 then
		notify("⚠  请对准水面抛竿", 1.5)
		state = FishingState.IDLE
		return
	end

	-- 等待咬钩
	state = FishingState.WAITING
	local lure = LURE_CONFIG[activeLureIdx]

	-- 基础咬钩概率（鱼饵越匹配越快）
	local baseWait = lure.targetTags[1] == "any" and 1.0 or 0.7
	local waitTime = math.random() * 6 * baseWait + 2

	task.spawn(function()
		local elapsed = 0
		local dt      = 0.5
		while state == FishingState.WAITING do
			task.wait(dt)
			elapsed = elapsed + dt
			if elapsed >= waitTime then
				local fishData = pickFish()
				local weight   = randomWeight(fishData)
				startFight(fishData, weight)
				break
			end
		end
	end)
end

-- ============================================================
-- 7. 遛鱼每帧更新（RenderStepped）
-- ============================================================

RunService.RenderStepped:Connect(function(dt)
	if state ~= FishingState.FIGHTING then return end
	local f = fight
	local rod = equippedRod

	-- ── 7.1 鱼冲刺逻辑 ──────────────────────────────────────
	if not f.isSprinting then
		f.sprintCooldown = f.sprintCooldown - dt
		if f.sprintCooldown <= 0 then
			f.isSprinting  = true
			f.sprintTimer  = f.fish.sprintDuration
			f.sprintDir    = 1 -- 向外冲（增加线长）
		end
	else
		f.sprintTimer = f.sprintTimer - dt
		if f.sprintTimer <= 0 then
			f.isSprinting    = false
			f.sprintCooldown = f.fish.sprintInterval + math.random(-1, 2)
		end

		-- 冲刺时线长增加（鱼往外跑）
		if f.sprintDir == 1 then
			f.lineLength = math.min(f.lineLengthMax, f.lineLength + dt * 4)
		end

		-- 冲刺中强行收线 → 张力暴增
		if f.reeling then
			f.tension = f.tension + f.fish.fightPower * 0.25 * dt
		else
			-- 不放线则张力缓慢增加
			f.tension = f.tension + f.fish.fightPower * 0.08 * dt
		end
	end

	-- ── 7.2 收线：消耗鱼体力，增加张力 ─────────────────────
	if f.reeling and not f.isSprinting then
		-- 体力消耗 = 收线速度 × 鱼竿疲劳倍率 × deltaTime
		local fatigueDmg = rod.reelSpeed * rod.fatigueMultiplier * dt * 8
		f.stamina = math.max(0, f.stamina - fatigueDmg)

		-- 线长缩短：体力归零后收线速度 ×2.5，明显感受到鱼被拉近
		local currentReelSpeed = rod.reelSpeed
		if f.stamina <= 0 then
			currentReelSpeed = rod.reelSpeed * 2.5
		end
		f.lineLength = math.max(f.lineLengthMin, f.lineLength - currentReelSpeed * dt)

		-- 张力自然增加（体力耗尽后鱼不再抵抗，张力几乎不增加）
		local tensionIncrease = (f.stamina > 0)
			and f.fish.fightPower * 0.04 * dt
			or  f.fish.fightPower * 0.005 * dt  -- 疲劳后张力几乎停止增长
		f.tension = f.tension + tensionIncrease
	end

	-- ── 7.3 放线：降低张力，鱼体力小幅恢复 ─────────────────
	if f.releasing then
		f.tension    = math.max(0, f.tension - 30 * dt)
		f.lineLength = math.min(f.lineLengthMax, f.lineLength + 2 * dt)
		-- 鱼放线时可小幅恢复体力（模拟 Far Cry 5 拉锯感）
		f.stamina    = math.min(f.staminaMax, f.stamina + 3 * dt)
	end

	-- 自然张力衰减（不操作时缓慢恢复）
	if not f.reeling and not f.releasing and not f.isSprinting then
		f.tension = math.max(0, f.tension - 15 * dt)
	end

	-- ── 7.4 断线判断 ─────────────────────────────────────────
	if f.tension >= rod.tensionMax then
		onFail(string.format("线断了！（%s 拉力上限 %d）", rod.name, rod.tensionMax))
		return
	end

	-- ── 7.5 上鱼判断：体力归零 + 线收到最短 ─────────────────
	if f.stamina <= 0 and f.lineLength <= f.lineLengthMin + 0.5 then
		onCatch()
		return
	end

	-- ── 7.6 体力耗尽但线还长：提示继续收线 ──────────────────
	if f.stamina <= 0 and not f.reeling then
		-- 仅提示（体力 0 不代表上鱼，还需收线到最短）
	end
end)

-- ============================================================
-- 8. 输入处理
-- ============================================================

UserInputService.InputBegan:Connect(function(input, gpe)
	if gpe then return end

	if input.UserInputType == Enum.UserInputType.MouseButton1 then
		if state == FishingState.IDLE then
			castLine()
		elseif state == FishingState.WAITING then
			-- 在等待期间按左键 = 收竿放弃
			state = FishingState.IDLE
			notify("收竿了", 1)
		elseif state == FishingState.FIGHTING then
			fight.reeling = true
		end

	elseif input.UserInputType == Enum.UserInputType.MouseButton2 then
		if state == FishingState.FIGHTING then
			fight.releasing = true
		end

		-- Q/E 切换鱼饵（PRD §7.2 切换方式）
	elseif input.KeyCode == Enum.KeyCode.Q then
		activeLureIdx = activeLureIdx - 1
		if activeLureIdx < 1 then activeLureIdx = #LURE_CONFIG end
		local l = LURE_CONFIG[activeLureIdx]
		notify("鱼饵：" .. l.name .. (l.unlocked and "" or "（未解锁）"), 1.5)

	elseif input.KeyCode == Enum.KeyCode.E then
		activeLureIdx = activeLureIdx + 1
		if activeLureIdx > #LURE_CONFIG then activeLureIdx = 1 end
		local l = LURE_CONFIG[activeLureIdx]
		notify("鱼饵：" .. l.name .. (l.unlocked and "" or "（未解锁）"), 1.5)

		-- Tab 查看背包（临时 print 方案，实际应弹出 UI）
	elseif input.KeyCode == Enum.KeyCode.Tab then
		print("=== 背包 ===")
		local total = 0
		for _, item in ipairs(inventory) do
			print(string.format("  %s  %.1f kg  $%d", item.species, item.weight, item.price))
			total = total + item.price
		end
		print("合计价值：$" .. total)
		notify("背包信息已输出到控制台", 2)
	end
end)

UserInputService.InputEnded:Connect(function(input, gpe)
	if input.UserInputType == Enum.UserInputType.MouseButton1 then
		fight.reeling = false
	elseif input.UserInputType == Enum.UserInputType.MouseButton2 then
		fight.releasing = false
	end
end)

-- ============================================================
-- 9. Heartbeat：每帧刷新 HUD
-- ============================================================
RunService.Heartbeat:Connect(function()
	refreshHUD()
end)

-- ============================================================
-- 10. 初始化
-- ============================================================
buildHUD()
notify("🎣  钓鱼系统已加载\n左键抛竿 | Q/E 切换鱼饵 | Tab 查看背包", 4)

print([[
===== 钓鱼游戏 MVP 已启动 =====
操作：
  左键（按住）= 收线（消耗鱼体力，增加张力）
  右键（按住）= 放线（降低张力，鱼小幅恢复）
  Q / E       = 切换鱼饵
  Tab         = 查看背包

上鱼条件：鱼体力归零 + 线收到最短
失败条件：张力超过鱼竿断线阈值
鱼冲刺时：建议放线，否则张力暴涨
================================
]])