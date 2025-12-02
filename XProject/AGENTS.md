# Repository Guidelines 仓库指南

-## 项目结构与模块
- Unity 6000.0.32f1 项目（来源：`ProjectSettings/ProjectVersion.txt`），核心脚本位于 `Assets/GameMain/Scripts`，按 TripleMerge/Game/Model/Terrain/UI/Framework/Common/SaveFile 分层；场景在 `Assets/Scenes`（InitScene/Main/MergeScene）；配置与地图数据在 `Assets/Resources/Configs/TripleMapData`；框架与第三方位于 `Assets/UnityGameFramework`、`Assets/ThirdParty`；`Bundles/` 为 YooAsset 构建产物。
- 恐龙合成背包方向：代码集中在 `Assets/GameMain/Scripts/Dinosaur`，资源在 `Assets/ExtraRes/Dinosaur`（建议子目录 Prefabs/Art/UI/Data/VFX/Audio）。

## Dinosaur 模块指引
- 需求来源：`dino_60days_plan.md`，按日推进。开工前确认对应 Day 目标与验收标准，提交时在 PR 描述里标注完成到第几天/子任务。
- 目录规范：脚本遵循 `Dinosaur` 命名空间；ScriptableObject、配置 JSON 放 `Assets/ExtraRes/Dinosaur/Data`；UI Prefab 与 Sprite 放 `Assets/ExtraRes/Dinosaur/UI`；战斗/骨头资源放 `Assets/ExtraRes/Dinosaur/Prefabs`/`Art`。
- 集成边界：尽量不侵入 TripleMerge 逻辑，公共功能抽到 `Framework/Common` 再被双方复用，避免直接引用玩法内部类。

## 构建、运行与开发命令
- 本地运行：通过 Unity Hub 打开本工程，优先从 `Assets/Scenes/InitScene.unity` 进入完整启动链路。
- 快速编译：`dotnet build XProject.sln`（校验脚本可编译，需本机安装 Unity 引用）。
- 批处理预留：`<Unity路径>\\Unity.exe -projectPath . -quit -batchmode -runTests` 可用于 CI 触发 Test Runner；如需自动打包，请在 Editor 下补充 BuildScript 再调用相应 `-executeMethod`。

## 编码风格与命名
- C# 使用 4 空格缩进；类型/方法 PascalCase，局部变量 camelCase，常量 ALL_CAPS；命名贴合玩法语义（例：Map/Region/Mergeable）；事件与枚举保持前后一致。
- 遵循 `TripleMergeSystem → TripleMergeGameplay → ITripleMergeComponent` 生命周期，异步优先使用 UniTask/async，避免高频 LINQ/装箱；注释与日志使用中文。
- 资源/Prefab 路径保持 `Assets/GameMain/...` 规范，勿随意移动 `UnityGameFramework` 与 `ThirdParty` 目录。

## 测试指南
- 当前未建立 `Assets/Tests`，建议新增 PlayMode 覆盖地图加载、宝箱生成、三合链路（含输入模拟）；Dinosaur 方向请增加背包拖拽、部件校验、战斗循环测试。命名建议 `{模块}PlayModeTests`。
- 手工回归：从 InitScene 验证 Launch→TripleMerge 状态切换、地图净化与拖拽、UI 提示；Dinosaur 任务需走背包→组装→战斗链路；资源或配置改动后执行 `dotnet build` 确认无脚本报错。

## 提交与 PR 规范
- git 历史以简短中文/英文句子为主（示例：`文档更新`、`新增一个回溯机制`），建议保持一句话描述 + 影响范围，必要时附 Issue 链接。
- 提交前排除 Library/Temp/Logs/obj 等生成物，确保 .meta 同步；PR 描述需包含变更目的、影响场景（InitScene/Main/MergeScene）、验证步骤/截图，涉及资源包时注明是否需重建 Bundles。
- 如改动全局配置或公共接口，请说明兼容/回滚方案（如恢复原 Prefab、撤销新增 ScriptableObject）。

## 资产与配置提示
- YooAsset/Addressables 构建产物存放 `Bundles/`，勿手工修改；大体积二进制建议使用 LFS 管理。
- 新增配置表放 `Assets/Resources/Configs/TripleMapData`，保持与加载器字段一致，更新后检查旧存档兼容性；Dinosaur 数据放 `Assets/ExtraRes/Dinosaur/Data`，确保字段与加载器/ScriptableObject 对齐。 
