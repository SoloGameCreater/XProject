# 仓库指南（Repository Guidelines）

## 项目结构与模块组织
本仓库是 Unity 6 项目（`ProjectSettings/ProjectVersion.txt` 当前为 `6000.3.8f1`）。

- `Assets/GameMain/Scripts/`：核心业务代码（`TripleMerge/` 玩法核心，`Framework/` 框架衔接，`UI/` 界面，`Common/` 通用逻辑，`SaveFile/` 存档）。
- `Assets/Editor/`：编辑器工具与菜单扩展（如预制体生成器、引用查找工具）。
- `Assets/Scenes/`：场景入口与主流程场景（如 `InitScene.unity`、`Main.unity`）。
- `Assets/UnityGameFramework/`、`Assets/ThirdParty/`：框架与第三方依赖。
- `Bundles/`：资源打包输出目录。
- `Docs/`：方案文档与迁移说明。

## 代码风格与命名规范
- 默认采用 fast fail：编写代码时不要过分追求兜底；遇到关键前置条件不满足、配置缺失、状态异常时，优先显式报错/断言并尽快暴露问题，避免吞异常、静默回退或为“继续跑下去”添加过度兼容逻辑；仅在需求明确要求容错时再补兜底。
- 执行 `review` 任务时，结论、问题描述和建议统一使用中文。
- 保持 `.meta` 同步；忽略 `Library/Temp`；大文件走 LFS；敏感配置不入库。

## 提交前检查
- Commit 使用简短中文；
- 每次提交保持单一变更意图；建议带范围前缀，例如：`TripleMerge: 修复地块净化状态保存`。
- 以后凡是提交代码，统一先调用 `$git-commit-guard` 按规则分析提交范围、description 与 review 阈值，再执行 commit。
- 本项目内允许使用：`git add`、`git diff`、`git status`、`git commit`、`git pull`。
- 本项目内禁止使用：`git push`。
- 在本项目中执行 `/review` 命令时，必须使用中文回复评审结果。

## 当前可用 Skills
- 路径约定：普通 skill 为 `~/.codex/skills/<skill>/SKILL.md`；标记 `[system]` 的 skill 为 `~/.codex/skills/.system/<skill>/SKILL.md`。

