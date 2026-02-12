# 仓库指南（Repository Guidelines）

## 项目结构与模块组织
本仓库是 Unity 6 项目（`ProjectSettings/ProjectVersion.txt` 当前为 `6000.3.7f1`）。

- `Assets/GameMain/Scripts/`：核心业务代码（`TripleMerge/` 玩法核心，`Framework/` 框架衔接，`UI/` 界面，`Common/` 通用逻辑，`SaveFile/` 存档）。
- `Assets/Editor/`：编辑器工具与菜单扩展（如预制体生成器、引用查找工具）。
- `Assets/Scenes/`：场景入口与主流程场景（如 `InitScene.unity`、`Main.unity`）。
- `Assets/UnityGameFramework/`、`Assets/ThirdParty/`：框架与第三方依赖。
- `Bundles/`：资源打包输出目录。
- `Docs/`：方案文档与迁移说明。

## 构建、测试与开发命令
- `dotnet build XProject.sln`
  - 在 Unity 外快速做 C# 编译检查。
- `"<UnityEditorPath>/Unity.exe" -batchmode -projectPath . -quit -executeMethod ResourceBuild.Build`
  - 命令行执行 YooAsset 资源构建（等价于编辑器菜单 `YooAsset/BuildRes`）。
- `"<UnityEditorPath>/Unity.exe" -batchmode -projectPath . -runTests -testPlatform EditMode -quit -logFile Logs/editmode-tests.log`
  - 运行 EditMode 自动化测试（若当前工程已配置测试）。
- 编辑器内联调建议：先执行 `YooAsset/BuildRes`，再从 `InitScene` 进入 PlayMode 做冒烟验证。

## 代码风格与命名规范
- 语言为 C#，统一 4 空格缩进，文件编码 UTF-8。
- 命名规则：类型/公共成员用 `PascalCase`，私有字段用 `_camelCase`，局部变量与参数用 `camelCase`。
- 所有字段/方法显式声明访问修饰符。
- 编辑器专用代码必须使用 `#if UNITY_EDITOR` 包裹。
- 运行时代码优先使用 `DebugUtil.Log()`，避免直接 `Debug.Log()`。
- 避免在玩法与 UI 代码中使用 `Transform.Find()` / `GameObject.Find()`，优先序列化引用或绑定组件。

## 测试指南
- 当前仓库未定义强制覆盖率阈值。
- PR 最低验证要求为主流程冒烟：启动链路（`Launch -> TripleMerge`）、地图加载、宝箱生成、存档读写恢复。
- 新增自动化测试建议遵循 Unity Test Runner 约定：
  - EditMode：`Assets/**/Tests/EditMode/`
  - PlayMode：`Assets/**/Tests/PlayMode/`
  - 文件命名：`*Tests.cs`

## 提交与合并请求规范
- 历史提交以简短中文主题为主（例如：`框架调整`、`存档改造+资源改名`）。
- 每次提交保持单一变更意图；建议带范围前缀，例如：`TripleMerge: 修复地块净化状态保存`。
- 本项目内允许使用：`git add`、`git diff`、`git status`、`git commit`、`git pull`。
- 本项目内禁止使用：`git push`。
- 所有 `git commit` 提交信息必须为中文，且准确描述改动目的与范围。
- PR 需包含：
  - 改动内容与动机。
  - 影响范围（场景、预制体、配置）。
  - 验证证据（日志、截图，UI/玩法变更可附短视频）。
  - 涉及资源框架调整时的回滚说明。
