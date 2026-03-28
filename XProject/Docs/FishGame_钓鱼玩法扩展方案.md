# FishGame 钓鱼玩法扩展方案

## 1. 文档目的

本文档用于冻结本轮 `FishGame` 钓鱼玩法扩展需求，作为后续分阶段开发、联调和验收的统一依据。

本文档覆盖以下内容：

1. 需求冻结与范围说明
2. 当前实现约束
3. 核心玩法闭环
4. 状态机与输入规则
5. 配置化设计
6. 存档与经济设计
7. UI 改造范围
8. 分阶段开发建议

---

## 2. 当前实现约束

基于当前仓库实现，以下约束已经确认：

1. 当前有效钓鱼逻辑集中在 `Assets/GameMain/Scripts/FishGame/FishGamePresenterV2.cs`。
2. 当前有效输入采集集中在 `Assets/GameMain/Scripts/UI/FishGame/FishGameMainUI.cs`。
3. 当前只采集鼠标左键输入，没有右键战斗逻辑。
4. 当前鱼上钩后会立刻加入背包并同步增加 `_totalScore`，这与后续“一键出售全部”逻辑冲突。
5. 当前 FishGame 模块没有自己的专属存档注册。
6. 当前公共货币系统可复用，但 `CurrencyModel.CostCurrency()` 现有实现存在扣费判断错误，后续接入购买时必须一并修复。

---

## 3. 本轮需求范围

### 3.1 本轮必须实现

1. `5m` 内不会有鱼咬钩。
2. 距离越远，越容易钓到高等级鱼。
3. 鱼饵越好，越容易钓到高等级鱼。
4. 鱼等级越高，阻力越大，出售价格越高。
5. 鱼体力可以恢复，体力决定鱼的挣扎强度。
6. 当鱼阻力大且鱼竿等级低时，鱼逃跑速度更快。
7. 鱼如果跑远超过 `45m`，判定本次钓鱼失败，自动快速收线并回到 `Idle`。
8. 增加锁线机制：玩家松开左键并按住右键时，鱼距离被锁定，鱼体力持续下降，线张力缓慢增长。
9. 增加一键出售功能，将鱼篓中的鱼全部卖出。
10. 增加鱼竿升级功能。
11. 增加消耗型鱼饵购买功能。
12. 将关键玩法属性改为配置驱动，而不是继续硬编码在 `Presenter` 中。

### 3.2 本轮只预留，不实现

1. 拟饵系统。
2. 移动端对应的锁线输入方案。
3. 更复杂的鱼种图鉴、任务链、鱼类图鉴收集等附加系统。
4. 单条出售、分批出售、鱼饵快捷购买等扩展交互。

### 3.3 本轮需求口径冻结

1. 鱼钓上来后先进入鱼篓，不再立即转化为金币。
2. `Score` 旧语义废弃，后续统一以 `Coin` 作为经济资源。
3. 一键出售仅允许在 `Idle` 状态下使用。
4. 本期交互平台先按 PC 鼠标输入实现。
5. 消耗型鱼饵按包购买，默认一包 `10` 个。

---

## 4. 核心玩法闭环

本轮目标是形成一个最小可玩的成长闭环：

1. 玩家使用鱼饵抛竿。
2. 根据抛竿距离、鱼饵品质、鱼种配置决定咬钩结果。
3. 玩家通过收线或锁线控制目标鱼。
4. 钓上来的鱼进入鱼篓。
5. 玩家在待命状态下一键出售鱼篓中的鱼，获得 `Coin`。
6. 玩家使用 `Coin` 购买更好的消耗型鱼饵、升级鱼竿。
7. 更高等级的鱼竿和鱼饵，提升挑战更高等级鱼的稳定性和收益。

---

## 5. 状态机设计

建议将钓鱼状态机统一收敛为以下 5 个状态：

### 5.1 Idle

待命状态。

允许行为：

1. 左键按住蓄力抛竿。
2. 切换鱼饵。
3. 打开或使用购买、升级、一键出售等待命功能。
4. 切换玩法。

### 5.2 Casting

抛竿动作播放状态。

行为特征：

1. 根据蓄力时长换算本次抛竿距离。
2. 抛竿动作结束后进入 `Waiting`。

### 5.3 Waiting

鱼线入水后的等待状态。

行为特征：

1. 如果本次抛竿距离 `<= NoBiteDistance`，本次不会咬钩。
2. 玩家此时仍可按住左键提前收线并取消本次抛竿。
3. 若满足咬钩条件，则进入 `Fighting`。

### 5.4 Fighting

遛鱼状态。

行为特征：

1. 左键按住时为收线。
2. 左键松开且右键按住时为锁线。
3. 左右都不按时，鱼按自身能力与鱼竿压制差值进行外冲。
4. 当张力超过鱼竿承载时失败。
5. 当鱼体力耗尽且线长达到收鱼阈值时成功。
6. 当鱼距离超过失败线时失败并进入 `AutoRetrieve`。

### 5.5 AutoRetrieve

自动快速收线状态。

行为特征：

1. 玩家不可操作。
2. 用于处理超距失败后的自动收线表现。
3. 收线完成后回到 `Idle`。

---

## 6. 输入规则设计

### 6.1 待命输入

1. 左键按住中央抛竿区：蓄力。
2. 左键松开：执行抛竿。

### 6.2 战斗输入

1. 按住左键：收线。
2. 松开左键并按住右键：锁线。
3. 左右都不按：不主动控线，鱼按自身状态向外跑。

### 6.3 锁线规则

锁线是一种高风险控鱼手段，不是“放线”。

锁线期间：

1. 鱼的当前距离保持不变。
2. 鱼体力持续下降。
3. 张力持续缓慢增长。
4. 如果鱼正在冲刺，则体力消耗更快，张力增长也更快。

---

## 7. 关键术语说明

### 7.1 FightFailDistance

战斗失败线。

定义：

1. 当鱼在战斗中的距离大于该值时，本次钓鱼直接失败。
2. 失败后进入 `AutoRetrieve` 自动收线。

默认建议值：

- `45m`

### 7.2 FightLineClampDistance

战斗可存在的最大线长上限。

定义：

1. 这是战斗过程中允许线长增长到的硬上限。
2. 它的作用是给失败线留出缓冲空间，避免线长在到达失败线之前就被系统上限卡死。
3. 如果它小于或等于失败线，那么“超过失败线失败”这一条规则将无法触发。

默认建议值：

- `50m`

### 7.3 FightLineClampDistance 和 FightFailDistance 的关系

二者关系如下：

1. `FightFailDistance` 用于判断输赢。
2. `FightLineClampDistance` 用于控制战斗线长系统的物理上限。
3. `FightLineClampDistance` 必须大于 `FightFailDistance`。

推荐关系：

1. `FightFailDistance = 45m`
2. `FightLineClampDistance = 50m`

这样处理后，鱼可以真正跑到 `45m` 以上触发失败，而不会在更小的上限处被提前截断。

---

## 8. 数值设计原则

### 8.1 玩家侧

玩家成长主要由鱼竿能力和鱼饵品质构成。

鱼竿负责：

1. 承受更高张力。
2. 提升收线效率。
3. 提升对高阻力鱼的压制能力。
4. 提升锁线效率。
5. 决定可支持的鱼饵类型或品质范围。

鱼饵负责：

1. 决定本次抛竿是否消耗库存。
2. 影响目标鱼种偏好。
3. 提升高等级鱼出现概率。
4. 影响咬钩等待时间。

### 8.2 鱼侧

鱼的难度与收益主要由以下属性构成：

1. 等级：整体成长阶梯。
2. 稀有度：表现层和稀有性口径。
3. 重量：决定卖价的一部分。
4. 体力：决定遛鱼时长。
5. 阻力：决定张力压力与控鱼难度。
6. 逃跑速度：决定不控线时外冲有多快。
7. 冲刺参数：决定短时爆发压力。

### 8.3 距离侧

距离承担风险收益调节职责：

1. 近距离：收益低，安全。
2. 中距离：基础体验区间。
3. 远距离：更容易出高等级鱼，但更容易超距失败。

---

## 9. 配置表设计

建议按 `ConfigManager + DataJson` 方式接入 FishGame 配置。

### 9.1 FishGlobalConfig

用于全局玩法规则。

建议字段：

1. `MinCastDistance`
2. `NoBiteDistance`
3. `MaxCastDistance`
4. `FightFailDistance`
5. `FightLineClampDistance`
6. `AutoRetrieveSpeed`
7. `WaitTimeMin`
8. `WaitTimeMax`
9. `EmptyHookCooldown`
10. `StarterRodLevel`
11. `StarterBaitId`
12. `StarterBaitCount`
13. `BaitPackCount`

### 9.2 FishRodLevelConfig

用于鱼竿成长配置。

建议字段：

1. `Level`
2. `Name`
3. `UpgradeCostCoin`
4. `LineStrength`
5. `ReelSpeed`
6. `ControlPower`
7. `EscapeMitigation`
8. `LockLineStaminaDamagePerSec`
9. `LockLineTensionGainPerSec`
10. `SupportedBaitTypeMask`
11. `SupportedBaitQualityMax`
12. `RecommendFishLevelMax`

说明：

1. `LineStrength` 用于张力上限。
2. `ControlPower` 用于压制鱼的外冲速度。
3. `EscapeMitigation` 用于面对高阻力鱼时减少逃跑惩罚。
4. `LockLineStaminaDamagePerSec` 和 `LockLineTensionGainPerSec` 用于锁线状态。

### 9.3 FishBaitConfig

用于鱼饵配置。

建议字段：

1. `BaitId`
2. `Name`
3. `Type`
4. `Quality`
5. `BuyPriceCoin`
6. `ConsumePerCast`
7. `TargetTags`
8. `HookWeightBonus`
9. `HighLevelWeightBonus`
10. `WaitTimeMultiplier`
11. `UnlockRodLevel`
12. `EnabledPhase`
13. `CanPurchase`

说明：

1. `Type` 分为 `Consumable` 和 `Lure`。
2. 本期只实现 `Consumable`。
3. `Lure` 只做配置和数据预留，不进入本期功能逻辑。

### 9.4 FishSpeciesConfig

用于鱼种本体配置。

建议字段：

1. `FishId`
2. `Species`
3. `Level`
4. `Rarity`
5. `WeightMin`
6. `WeightMax`
7. `BaseStamina`
8. `StaminaRecoveryPerSec`
9. `Resistance`
10. `EscapeSpeed`
11. `SprintInterval`
12. `SprintDuration`
13. `BasePricePerKg`
14. `PreferredBaitTags`
15. `CatchWeight`
16. `RecommendRodLevel`

说明：

1. `Level` 表示鱼的成长阶梯。
2. `Rarity` 主要用于表现层、掉落口径和展示色。
3. `CatchWeight` 用于鱼种在候选池中的基础出现权重。

### 9.5 FishDistanceTierConfig

用于按距离分层控制掉落风险收益。

建议字段：

1. `MinDistance`
2. `MaxDistance`
3. `CanBite`
4. `FishLevelWeightBonus`
5. `RarityWeightCommon`
6. `RarityWeightUncommon`
7. `RarityWeightRare`
8. `RarityWeightLegendary`
9. `WaitTimeMultiplier`

说明：

1. 该配置表用于控制“越远越容易高等级鱼”。
2. 距离对鱼等级和稀有度的影响应尽量通过配置完成，而不是写死公式。

---

## 10. 经济与售卖设计

### 10.1 经济资源

统一使用 `Coin` 作为钓鱼玩法本轮经济资源。

### 10.2 鱼篓口径

1. 成功钓鱼后，鱼进入鱼篓。
2. 鱼不会立即自动卖出。
3. 鱼篓中的鱼会显示单条信息和总预估价值。

### 10.3 一键出售

规则如下：

1. 只允许在 `Idle` 状态使用。
2. 将鱼篓中全部鱼按照最终售价结算为 `Coin`。
3. 成功出售后清空鱼篓。
4. 需要同步更新鱼篓 UI、金币显示和存档。

### 10.4 售价公式建议

建议按以下方式组织：

1. `最终售价 = Weight * BasePricePerKg`
2. 如需后续扩展，可追加稀有度倍率或活动倍率。

本期建议先不引入复杂售价乘区，先保持可读和可控。

---

## 11. 鱼竿与鱼饵成长设计

### 11.1 鱼竿升级

本期建议：

1. 鱼竿做单条升级线。
2. 默认规划 `5` 级。
3. 每次升级消耗 `Coin`。
4. 升级后提升 `LineStrength`、`ReelSpeed`、`ControlPower` 等关键属性。

### 11.2 消耗型鱼饵购买

本期建议：

1. 默认规划 `3` 档消耗型鱼饵。
2. 每次购买获得固定包数量，默认一包 `10` 个。
3. 每次抛竿消耗 `1` 个当前装备的消耗型鱼饵。
4. 若当前鱼饵库存不足，则禁止抛竿并给出明确提示。

### 11.3 拟饵预留

本期不实现拟饵功能，但配置和存档结构应预留：

1. 拟饵解锁状态
2. 拟饵品质
3. 拟饵适配鱼竿等级
4. 拟饵特殊加成字段

---

## 12. 存档设计

FishGame 需要补专属存档，并由 `FishGameModule` 注册。

建议新增 `SaveFileFishGame`，至少包含：

1. `RodLevel`
2. `EquippedBaitId`
3. `OwnedBaits`
4. `UnlockedLures`
5. `FishBag`
6. `TotalCaughtCount`
7. `TotalSoldCount`

说明：

1. `Coin` 不在 `SaveFileFishGame` 中重复存储，直接复用公共货币系统。
2. `OwnedBaits` 建议保存为 `baitId -> count`。
3. `UnlockedLures` 建议保存为已解锁拟饵集合或字典。
4. `FishBag` 需要保存单条鱼的 `FishId`、`Weight`、`SellPrice`。

---

## 13. UI 改造范围

本期 UI 只做最小必要改造，不另起新玩法框架。

建议范围：

1. 新增金币显示。
2. 新增一键出售按钮。
3. 新增鱼竿升级入口。
4. 新增鱼饵购买入口。
5. 新增当前鱼饵库存显示。
6. 增加右键锁线对应的提示文案。
7. 保留现有抛竿区交互方式。

明确不做：

1. 独立商城页大改版。
2. 复杂分页背包。
3. 移动端专用输入布局。

---

## 14. 默认数值口径

当前已确认或建议采用的默认口径如下：

1. `FightFailDistance = 45m`
2. `FightLineClampDistance = 50m`
3. 锁线期间允许鱼冲刺，但会更快掉体力、更快涨张力。
4. 消耗型鱼饵按包购买，默认每包 `10` 个。
5. 本期仅按 PC 鼠标交互实现。
6. 鱼竿先规划 `5` 级。
7. 消耗型鱼饵先规划 `3` 档。
8. 鱼保留 `4` 个 rarity 和 `5` 个鱼等级梯度即可满足本期最小闭环。

---

## 15. 建议开发顺序

建议后续按以下顺序实施：

1. 先补 FishGame 配置系统。
2. 再补 FishGame 专属存档。
3. 再重构 `FishGamePresenterV2` 状态机与规则逻辑。
4. 再扩展 `FishGameMainUI` 输入采集与 UI 入口。
5. 最后接入购买、升级、出售和文案联调。

这样可以避免先做 UI 再返工玩法底层。

---

## 16. 验收要点

本轮开发完成后，至少需要满足以下验收点：

1. `5m` 内抛竿不会咬钩。
2. 远距离抛竿相比近距离，更容易出高等级鱼。
3. 更高品质鱼饵相比基础鱼饵，更容易出高等级鱼。
4. 高等级鱼在战斗中明显更难控，售价也更高。
5. 左键收线、右键锁线、双手不按时放任外冲三种状态行为明确。
6. 鱼跑到 `45m` 以上时，必然失败并自动快速收线。
7. 一键出售能将鱼篓中的全部鱼转换为 `Coin`。
8. 鱼竿升级与鱼饵购买都能正常持久化。
9. 退出并重新进入 FishGame 后，鱼竿等级、鱼饵库存、鱼篓、金币状态正确恢复。

---

## 17. TODO：战斗公式继续拆分配置化

当前版本已完成核心配置驱动，但战斗阶段仍有一部分数值公式写在 `FishGamePresenterV2` 中。
这些内容本期先不继续扩改，避免影响手感与联调范围；后续如进入第二轮平衡，再统一拆到配置层。

建议后续拆分范围如下：

1. 抛竿阶段固定参数：
   `MaxCastChargeDuration`、抛竿动作时长等。
2. 收线公式参数：
   收线速度衰减系数、净收线抵消系数、收线造成的体力伤害系数、收线带来的张力增长系数。
3. 放任外冲公式参数：
   外冲时张力自然衰减值、鱼体力恢复倍率、冲刺期间恢复衰减倍率。
4. 锁线附加倍率：
   锁线时的冲刺加速倍率、张力增长附加系数、体力损耗附加系数。
5. 推荐差值惩罚参数：
   鱼等级超过鱼竿推荐上限时的惩罚系数、鱼种 `RecommendRodLevel` 高于当前鱼竿等级时的惩罚系数。
6. 冲刺公式参数：
   冲刺速度倍率、冲刺冷却随机浮动范围。
7. 成功判定缓冲值：
   当前 `LineLength <= CatchLineDistance + buffer` 中的 buffer 建议外提配置。

后续落地建议：

1. 先在 `FishGlobalConfig` 中补通用战斗参数。
2. 如需按鱼竿区分战斗风格，再把局部参数拆到 `FishRodLevelConfig`。
3. 拆分时保持默认值与当前线上数值一致，先做“等价迁移”，再做平衡调整。
