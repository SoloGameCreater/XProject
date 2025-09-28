# 三合一合成项目流程框架

本说明汇总 Unity 项目中与三合一合成（Triple Merge）玩法相关的核心目录、启动链路与运行循环，方便新成员快速定位代码、理解数据流并规划迭代步骤。

## 1. 项目鸟瞰
- **引擎与框架**：Unity + 自研框架（`Framework` 命名空间），结合 YooAsset、DOTween、UniTask、UnityGameFramework 等第三方库。【F:README.md†L1-L48】
- **主要玩法**：三合一合成，围绕地图净化、宝箱生成、物品合成与 UI 引导展开。【F:README.md†L8-L28】
- **核心脚本集中于**：`Assets/GameMain/Scripts/TripleMerge` 目录，按 Game/Model/MergeObject/Terrain/FSM 划分子域。【F:README.md†L30-L47】

## 2. 代码与资源速查
| 范围 | 说明 | 关键位置 |
| --- | --- | --- |
| 引导场景 | 初始化加载、状态机管理 | `Assets/GameMain/Scripts/Main` (`MyGame.cs`, `Launching.cs`)【F:Assets/GameMain/Scripts/Main/MyGame.cs†L1-L62】【F:Assets/GameMain/Scripts/Main/Launching.cs†L1-L123】 |
| FSM | 游戏状态管理，驱动 Launch → TripleMerge | `Assets/GameMain/Scripts/Common/Fsm/StateLaunch.cs`、`TripleMerge/FSM/StateTripleMerge.cs`【F:Assets/GameMain/Scripts/Common/Fsm/StateLaunch.cs†L1-L35】【F:Assets/GameMain/Scripts/TripleMerge/FSM/StateTripleMerge.cs†L1-L56】 |
| 玩法系统 | GlobalSystem 单例 + Component 生命周期 | `TripleMergeSystem.cs`、`Game/TripleMergeGameplay.cs`【F:Assets/GameMain/Scripts/TripleMerge/TripleMergeSystem.cs†L1-L42】【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeGameplay.cs†L1-L55】 |
| 地图/地块 | 区域、地块、净化逻辑 | `Game/TripleMergeComponent/TripleMergeMapManager.cs`、`Terrain/MapComponent.cs`【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeComponent/TripleMergeMapManager.cs†L1-L97】【F:Assets/GameMain/Scripts/TripleMerge/Terrain/MapComponent.cs†L1-L120】 |
| 相机与输入 | 跨平台相机控制、拖拽监听 | `Game/TripleMergeComponent/TripleMergeCameraComponent.cs`【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeComponent/TripleMergeCameraComponent.cs†L1-L154】 |
| 物品与宝箱 | 地块物品状态、宝箱生成 | `MergeObject/MergeableCell.cs`、`Game/TripleMergeComponent/TripleMergeTreasureComponent.cs`【F:Assets/GameMain/Scripts/TripleMerge/MergeObject/MergeableCell.cs†L233-L402】【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeComponent/TripleMergeTreasureComponent.cs†L1-L58】 |
| UI | 大厅入口、地图 UI、提示气泡 | `UI/Lobby/LobbyMainUI.cs`、`TripleMerge/MapUI/...`【F:Assets/GameMain/Scripts/UI/Lobby/LobbyMainUI.cs†L14-L55】 |
| 数据与存档 | 地图配置、存档结构、解锁数据 | `Data/MapDataLoader.cs`、`Model/TripleMergeModel.cs`、`SaveFile/TripleMerge/SaveFileTripleMerge.cs`【F:Assets/GameMain/Scripts/TripleMerge/Data/MapDataLoader.cs†L1-L89】【F:Assets/GameMain/Scripts/TripleMerge/Model/TripleMergeModel.cs†L1-L135】【F:Assets/GameMain/Scripts/SaveFile/TripleMerge/SaveFileTripleMerge.cs†L1-L55】 |

## 3. 启动与状态流转
1. **游戏初始化**（`MyGame.OnInit`）
   - 注册框架级子系统（协程、对象池、UI 等）与玩法所需子系统（`TripleMergeSystem`、`MapDataLoader`、`TripleMergeConfigManager` 等）。
   - 初始化有限状态机（FSM），起始态为 `Launch`。【F:Assets/GameMain/Scripts/Main/MyGame.cs†L13-L59】
2. **启动流程**（`StateLaunch` + `Launching`）
   - `StateLaunch.PreEnterAsync` 创建 `Launching` 实例。
   - `Launching.startLaunchSequence` 负责本地化匹配、配置加载、Loading UI 展示，并持续刷新进度条；完成后切换 FSM 至 `FsmStateType.TripleMerge`。【F:Assets/GameMain/Scripts/Common/Fsm/StateLaunch.cs†L5-L33】【F:Assets/GameMain/Scripts/Main/Launching.cs†L1-L123】
3. **进入三合玩法**（`StateTripleMerge`）
   - `PreEnterAsync` 调用 `TripleMergeSystem.OnEnterTripleMerge` 完成地图数据加载与 Gameplay 初始化。
   - `EnterFinish` 打开 `LobbyMainUI`，玩家可触发宝箱生成等交互。【F:Assets/GameMain/Scripts/TripleMerge/FSM/StateTripleMerge.cs†L11-L42】

### 3.1 TripleMergeSystem 初始化
```
StateTripleMerge.PreEnterAsync
    └─> TripleMergeSystem.OnEnterTripleMerge
         ├─ MapDataLoader.LoadMapData()
         ├─ new TripleMergeGameplay().Init()
         │    └─ 依次 OnInitialize(): MapUIManager → MapManager → Camera → Treasure
         └─ 初始化测试宝箱货币
```
【F:Assets/GameMain/Scripts/TripleMerge/TripleMergeSystem.cs†L11-L27】【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeGameplay.cs†L18-L41】

## 4. 玩法系统分层
| 层级 | 主要职责 | 说明 |
| --- | --- | --- |
| GlobalSystem 层 | 生命周期入口、全局数据 | `TripleMergeSystem` 作为子系统由框架驱动 Update/LateUpdate，并维护 `TripleMergeModel` 数据中心。【F:Assets/GameMain/Scripts/TripleMerge/TripleMergeSystem.cs†L6-L39】 |
| Gameplay 聚合层 | 统一管理组件 | `TripleMergeGameplay` 汇总所有 `ITripleMergeComponent`，负责 MapRoot 实例化与组件的统一 Init/Update/Dispose。【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeGameplay.cs†L10-L47】 |
| 组件层 | 具体功能模块 | `TripleMergeComponent` 派生类（地图、相机、宝箱、UI）实现 `OnInitialize/OnUpdate/...` 生命周期，与 Gameplay 共享上下文。示例：`TripleMergeMapManager` 加载区域、缓存地块并暴露空格查询。【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeComponent/TripleMergeMapManager.cs†L1-L82】 |
| 实体层 | 地块与物品逻辑 | `MergeableCell` 负责合成流程、净化值、事件派发；`OnCellObject`/`TreasureChest` 等派生类处理交互动画与存档写入。【F:Assets/GameMain/Scripts/TripleMerge/MergeObject/MergeableCell.cs†L233-L402】【F:Assets/GameMain/Scripts/TripleMerge/MergeObject/TreasureChest.cs†L1-L226】 |
| UI 层 | 视图展示与交互 | `LobbyMainUI` 控制玩法入口按钮；`MapUIManager` 下的视图（地块净化条、区域解锁提示等）实时监听事件更新展示。【F:Assets/GameMain/Scripts/UI/Lobby/LobbyMainUI.cs†L14-L53】【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeComponent/TripleMergeMapUIManager.cs†L1-L63】 |

## 5. 运行时循环
1. **加载阶段**
   - `MapDataLoader.LoadMapData` 读取 `MapData.json` 并反序列化为地块/区域配置。
   - `TripleMergeMapManager` 实例化 `AreaRoot`、挂载 `MapAreaComponent`，初始化区域与地块字典。
   - `MapAreaComponent.Initialize` 设置相机点位、建立地块邻接与存档状态。
   - `TripleMergeCameraComponent` 读取地图提供的相机边界并重置视角。【F:Assets/GameMain/Scripts/TripleMerge/Data/MapDataLoader.cs†L13-L89】【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeComponent/TripleMergeMapManager.cs†L19-L64】【F:Assets/GameMain/Scripts/TripleMerge/Terrain/MapComponent.cs†L25-L92】【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeComponent/TripleMergeCameraComponent.cs†L66-L120】
2. **逐帧更新**
   - `TripleMergeSystem.Update/LateUpdate` → `TripleMergeGameplay.OnUpdate/OnLateUpdate` → 组件的逐帧逻辑（输入监听、UI 刷新、宝箱队列等）。【F:Assets/GameMain/Scripts/TripleMerge/TripleMergeSystem.cs†L27-L39】【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeGameplay.cs†L31-L48】
3. **交互流程**
   - 玩家从 UI 触发宝箱生成 → `TripleMergeTreasureComponent.GenerateTreasure` 扣除货币、选择最近空格、播放飞行动画并放置宝箱。
   - 物品拖拽时 `TripleMergeCameraComponent` 自动跟随并限制相机范围，释放后 `MergeableCell` 判断合成、更新存档并派发区域解锁或净化事件。【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeComponent/TripleMergeTreasureComponent.cs†L19-L58】【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeComponent/TripleMergeCameraComponent.cs†L122-L215】【F:Assets/GameMain/Scripts/TripleMerge/MergeObject/MergeableCell.cs†L318-L402】
4. **数据持久化**
   - `TripleMergeModel` 通过 `SaveFileTripleMerge` 读写地块状态、物品 ID、区域解锁与宝箱开启次数。
   - 新解锁的物品会写入 `UnlockedMergeableItems` 并触发通知，驱动后续内容解锁逻辑。【F:Assets/GameMain/Scripts/TripleMerge/Model/TripleMergeModel.cs†L12-L128】【F:Assets/GameMain/Scripts/SaveFile/TripleMerge/SaveFileTripleMerge.cs†L1-L55】
5. **退出清理**
   - 切换离开玩法时调用 `TripleMergeSystem.Exit`，释放 Gameplay 组件与 MapRoot，确保状态回收。【F:Assets/GameMain/Scripts/TripleMerge/TripleMergeSystem.cs†L31-L39】

## 6. 数据、配置与工具链
- **地图配置**：`MapData.json` 位于 `Resources/Configs/TripleMapData`，支持 Editor 通过 `MapDataLoader.GetMapData` 直接读取生成地块参数。【F:Assets/GameMain/Scripts/TripleMerge/Data/MapDataLoader.cs†L13-L89】
- **存档结构**：`SaveFileTripleMerge` 记录区域 ID、地块字典、已解锁物品、宝箱次数与剧情阶段，并在属性变动时提升 `LocalVersion` 触发保存。【F:Assets/GameMain/Scripts/SaveFile/TripleMerge/SaveFileTripleMerge.cs†L1-L55】
- **配置加载**：`TripleMergeConfigManager` 挂载在子系统列表中，负责三合物品、合成链等静态表读取，为 Model/Map 初始化提供数据源。【F:Assets/GameMain/Scripts/Main/MyGame.cs†L36-L57】【F:Assets/GameMain/Scripts/TripleMerge/Model/TripleMergeModel.cs†L59-L111】
- **外部工具**：根目录提供 `GoogleSheet2Json`（空目录，用于表格导出脚本）及 `CLAUDE.md`/`README.md` 等资料，可拓展数据制作流程。

## 7. 事件与协作机制
- **事件派发**：`EventDispatcher` 广播输入禁用、区域解锁、地图内容变更等事件，组件通过订阅保持解耦；例如宝箱放置后派发 `TripleMergeOnMapContentChanged` 供 UI 更新。【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeComponent/TripleMergeTreasureComponent.cs†L46-L56】【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeComponent/TripleMergeCameraComponent.cs†L94-L113】
- **生命周期约束**：自定义组件需实现 `ITripleMergeComponent` 接口并由 `TripleMergeGameplay` 管理，保证初始化顺序与资源释放一致。
- **输入控制**：相机组件通过引用计数的 `IsInputDisabled` 标志屏蔽输入，确保合成动画或 UI 弹窗期间交互安全。【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeComponent/TripleMergeCameraComponent.cs†L83-L111】

## 8. 扩展与调试建议
- **新增玩法组件**：在 `TripleMergeGameplay.Init` 中注册新组件，利用 `MapRoot` 下的既有节点挂载场景对象，避免重复实例化。【F:Assets/GameMain/Scripts/TripleMerge/Game/TripleMergeGameplay.cs†L22-L37】
- **扩展地块内容**：优先在 `MapData.json`/配置表补充区域与地块，再通过 `MapAreaComponent`/`MergeableCell` 的存档接口注入逻辑，保持与数据驱动一致。【F:Assets/GameMain/Scripts/TripleMerge/Terrain/MapComponent.cs†L41-L104】【F:Assets/GameMain/Scripts/TripleMerge/Model/TripleMergeModel.cs†L75-L129】
- **调试入口**：`LobbyMainUI` 的 Debug 按钮在 Editor/Development 模式下可打开调试界面；必要时可在 `Launching` 阶段插入配置检查或资源预加载步骤。【F:Assets/GameMain/Scripts/UI/Lobby/LobbyMainUI.cs†L20-L53】【F:Assets/GameMain/Scripts/Main/Launching.cs†L1-L123】

> 通过上述分层与流程梳理，可快速定位问题所在的系统层级，并在扩展玩法时遵循现有生命周期、数据和事件规范，降低耦合与维护成本。
