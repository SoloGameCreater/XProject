# 仓库指南

## 项目概览
- 本仓库是 Unity 6 项目，当前版本见 `ProjectSettings/ProjectVersion.txt`。
- 主要目录：
  - `Assets/GameMain/Scripts/`：业务代码与 UI。
  - `Assets/Editor/`：编辑器扩展。
  - `Assets/Scenes/`：场景入口。
  - `Assets/UnityGameFramework/`、`Assets/ThirdParty/`：框架与第三方依赖。
  - `Bundles/`：资源打包输出。
  - `Docs/`：方案与说明文档。

## 开发约定
- 默认采用 fast fail：关键前置条件不满足时，优先显式报错或断言，不做过度兜底。
- 执行 `review` 或 `/review` 时，结论、问题描述和建议统一使用中文。
- 提交 Unity 资源变更时，保持 `.meta` 文件同步；`Library/`、`Temp/` 等临时内容不入库。

## 提交约定
- Commit 使用简短中文，保持单一变更意图。
- 建议使用范围前缀，例如：`TripleMerge: 修复地块净化状态保存`。
- 凡是提交代码，先调用 `$git-commit-guard` 分析提交范围，再执行 `git commit`。
- 本项目禁止 `git push`。

