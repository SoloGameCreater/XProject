📘 Dino Backpack AutoBattler – 60 Day Development Plan

Codex / GitHub Copilot Workspace 专用版本

本文档根据原始文件《恐龙背包乱斗 · 60 天强化版开发计划》转换
目的：让 Codex 能够逐日读取、拆分任务、自动生成脚本与系统。

🛠 使用方式

在 Codex 里输入（示例）：

读取 dino_60days_plan.md，执行 Day 3 的所有任务，为我生成拼装插槽 UI 的代码与 Prefab 结构。


或：

执行本计划 Day 10，生成金币系统与战斗结算 UI。


Codex 会严格按任务推进。

—————————————————————————————
Week 1 — 项目骨架 + 最小战斗原型
—————————————————————————————
Day 1 — 项目初始化 + 工程结构
📌 任务

创建 Unity 项目（使用任意 LTS）

建立基础目录：
代码类放置于：..Assets\GameMain\Scripts之下，文件夹名为：
Dinosaur
资源类放置于：..\Assets\ExtraRes\Dinosaur之下，文件夹名为：
Prefabs

Art

UI

Data

VFX

Audio

创建 MainScene（含 Camera、GameRoot）

✅ 验收标准

场景能运行

文件结构清晰

Day 2 — 基础数据结构（骨头/部位）
📌 任务

创建：

BonePart.cs

PartType enum（Head / Body / Tail / Leg / Arm）

BonePartConfig ScriptableObject

字段：

Id

PartType

HP / ATK / DEF

✅ 验收标准

Inspector 能新建骨头数据并读取。

Day 3 — 拼装插槽 UI 原型
📌 任务

左侧：背包格子

右侧：恐龙插槽（Head/Body/Tail/LegL/LegR）

插槽可高亮接受的部件类型

✅ 验收标准

UI 正常显示

插槽悬停高亮

Day 4 — 拖拽交互
📌 任务

背包的 Bone UI 可拖动

插槽判断类型

成功放置 → 更新 DinoBuild

✅ 验收标准

拖入后 Console 输出构建信息。

Day 5 — DinoEntity 属性系统
📌 任务

创建：

DinoEntity.cs

RebuildStatsFromParts()

功能：

汇总 HP / ATK / DEF

✅ 验收标准

拖不同骨头后点击“重算” → 属性变化。

Day 6 — 最小战斗循环
📌 任务

战斗规则：

每 1 秒互砍

伤害 = ATK - DEF（至少 1）

HP ≤ 0 结束

✅ 验收标准

能进行简易互殴并输出胜负。

Day 7 — 整理与重构
📌 任务

脚本命名整理

添加基础注释

整合流程：拼装 → 战斗 → 返回

—————————————————————————————
Week 2 — 闭环打通（拼装 → 战斗 → 奖励 → 商店）
—————————————————————————————
Day 8 — 攻击节奏
📌 任务

攻击间隔由固定 1 秒 → AttackInterval

AttackInterval 来自 DinoEntity

Day 9 — 腺体系统 1：Electric
📌 任务

在 BonePartConfig 增加：

GlandType（None/Electric/Poison/...）

Electric 效果：

命中时 20% 额外伤害

Day 10 — 战斗奖励
📌 任务

PlayerGoldManager

胜：+5

负：+3

战斗结算 UI

Day 11 — 商店界面
📌 任务

ShopPanel（4 普通 + 1 腺体 + 1 稀有）

ShopItem 数据结构

从池中随机抽取商品

Day 12 — 购买逻辑
📌 任务

扣金币

加入背包

禁止重复购买

Day 13 — 商店刷新
📌 任务

刷新按钮

消耗 2 金币

刷新全部商品

Day 14 — 完整循环测试
📌 任务

跑通：

拼装

战斗

结算

商店

购买

再拼装

—————————————————————————————
Week 3 — 敌人构筑库 + 难度曲线
—————————————————————————————
Day 15 — 敌人构筑模板 EnemyBuildConfig
📌 字段

Id

Name

Description

PartList

DifficultyTag

Day 16 — 敌人生成管线
📌 任务

BattleManager 引用敌方构筑

EnemyManager 负责选择构筑

战斗前根据构筑生成敌方 DinoEntity

Day 17 — 六套基础构筑
📌 类型

速攻

毒

坦

骨刺

电

混合

Day 18 — 难度 Level 系统
📌 任务

DifficultyLevel (1~5)

根据难度乘区调整属性

Day 19 — Round 流程（1~8）
📌 任务

GameSessionManager：

记录 RoundIndex

每回合切换敌人

Day 20 — 数值调整
Day 21 — 整理与重构
—————————————————————————————
Week 4 — 爽点强化（VFX + 机制刺激）
—————————————————————————————
Day 22 — 爆炸腺 ExplodeGland
📌 效果

击杀/暴击时 AOE

3 秒冷却

Day 23 — 骨刺腺 ThornsGland
📌 效果

受击反伤

避免反伤死循环

Day 24 — 命中 & 死亡 VFX
Day 25 — 电击 & 毒雾 VFX
Day 26 — Shader/风格统一
Day 27 — 镜头震动
Day 28 — 爽点评估
—————————————————————————————
Week 5 — Demo 内容封装
—————————————————————————————
Day 29 — Demo 回合设计
📌 回合结构

Round 1：教学

Round 2~3：风格差异

Round 4~6：强度递增

Round 7~8：Boss

Day 30 — 回合驱动敌人选择
Day 31 — 部件池扩充至 ≥30 件
Day 32 — 刷新券系统
Day 33 — 战斗节奏（10~20 秒）
Day 34 — 通关 & 失败体验
Day 35 — 流畅性检查
—————————————————————————————
Week 6 — 玩家体验打磨
—————————————————————————————
Day 36 — 新手引导 1（拼装）
Day 37 — 新手引导 2（战斗）
Day 38 — 新手引导 3（商店）
Day 39 — 基础音效
Day 40 — BGM
Day 41 — 错误容错
Day 42 — 小规模玩家测试
—————————————————————————————
Week 7 — Steam Demo 包装
—————————————————————————————
Day 43 — 性能优化
Day 44 — 工程清理
Day 45 — 截图（至少 5 张）
Day 46 — 战斗视频素材
Day 47 — Trailer（30~45 秒）
Day 48 — Steam 文案（中/英）
Day 49 — 商店页草稿
—————————————————————————————
Week 8 — 封板 & Demo 上线
—————————————————————————————
Day 50 — 内部封板测试
Day 51 — 修复关键问题
Day 52 — 平衡微调
Day 53 — UI/文案打磨
Day 54 — 最终打包与自测
Day 55 — 提交 Steam Demo
Day 56 — Demo 上线
Day 57~60 — 数据观察 & 反馈总结