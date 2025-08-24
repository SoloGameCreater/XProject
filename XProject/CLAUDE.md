# CLAUDE.md

本文件为 Claude Code (claude.ai/code) 提供在此代码库中工作的指导。

## 项目概述

这是一个基于Unity的三合一合成游戏 - 一款休闲合并益智游戏，玩家通过合并三个相同的物品来创建更高级的物品，解锁新区域并推进关卡。游戏特点：

- **核心机制**: 三合一合成系统，包含宝箱、可合成物品和万能卡
- **地图系统**: 基于网格的地形，具有可解锁区域和可净化格子
- **架构**: 使用UnityGameFramework和FSM模式的组件化设计

## 核心技术和框架

- **Unity版本**: 2021.3 LTS或更高版本
- **UnityGameFramework**: 核心游戏框架，用于状态管理和系统
- **YooAsset**: 资源加载和管理系统
- **UniTask**: 异步/等待编程支持
- **DOTween**: 动画和补间系统
- **TextMeshPro**: 增强文本渲染
- **Sirenix Odin Inspector**: 高级编辑器工具

## 开发命令

### 构建命令
- 通过Unity编辑器构建: `File > Build Settings`
- 通过Visual Studio使用 `XProject.sln` 构建解决方案

### 开发工作流
- **场景管理**: 主要场景为 `MergeScene.unity` (主要游戏玩法), `InitScene.unity` (初始化), 和 `Main.unity`
- **资源管理**: 通过YooAsset系统处理资源，使用StreamingAssets包
- **测试**: 未检测到特定测试框架 - 通过Unity编辑器播放模式进行测试

## 代码架构

### 核心系统
- **TripleMergeSystem**: 主要游戏逻辑控制器，位于 `Assets/GameMain/Scripts/TripleMerge/`
- **FSM管理**: 游戏状态通过 `Framework.FSM` 管理，包含如 `StateTripleMerge` 等状态
- **UI系统**: 使用 `UIViewSystem` 和 `UIView` 基类的组件化UI
- **存档系统**: 通过 `SaveFileManager` 和 `SaveFileSystem` 进行数据持久化

### 关键目录
```
Assets/GameMain/Scripts/
├── TripleMerge/           # 核心游戏机制
│   ├── MergeObject/       # 可合成物品和格子
│   ├── Game/              # 游戏组件和逻辑
│   ├── Data/              # 游戏数据结构
│   ├── Terrain/           # 地图和区域管理
│   └── FSM/               # 游戏状态机
├── Framework/             # 核心框架系统
│   ├── UI/                # UI框架组件
│   ├── Manager/           # 系统管理器
│   ├── SubSystems/        # 模块化子系统
│   └── Extensions/        # Unity扩展
├── Common/                # 共享实用工具和配置
├── SaveFile/              # 存档游戏数据结构
└── UI/                    # 游戏特定UI控制器
```

### 设计模式
- **组件系统**: 大量使用Unity的组件化架构
- **状态机**: 通过 `IFsmState` 实现的FSM模式进行游戏流程管理
- **对象池**: `OnCellObjectPool` 用于高效的对象重用
- **管理器模式**: 单例管理器如 `ResourcesManager`, `SubSystemManager`
- **事件系统**: 通过 `EventDispatcher` 实现解耦通信

## 开发指南

### 代码风格（来自.cursorrules）
- 公共成员使用 **PascalCase**，私有成员使用 **camelCase**
- 私有字段以下划线前缀: `_fieldName`
- 使用 `DebugUtil.Log()` 而不是 `Debug.Log()`
- 用 `#if UNITY_EDITOR` 包装仅编辑器代码

### UI开发
- 默认使用 `LocalizeTextMeshProUGUI` 作为文本组件（支持国际化）
- 使用 `[ComponentBinder]` 特性进行UI组件绑定
- 使用 `[AssetAddress]` 指定资源路径
- 避免使用 `Transform.Find()` - 优先使用直接引用

### 性能最佳实践
- 使用 `TryGetComponent()` 避免空引用异常
- 通过 `ObjectPoolMgr` 为频繁实例化的对象实现对象池
- 为可池化对象实现 `PoolableComponent` 接口
- 在 `Awake()` 中缓存组件引用，在 `Start()` 中初始化逻辑
- 使用 `StringBuilder` 进行字符串拼接
- 避免在 `Update()` 方法中进行GC分配

### 架构模式
- MonoBehaviour单例继承自 `Manager<T>`
- 非MonoBehaviour单例继承自 `GlobalSystem<T>`
- 适当使用 `[RequireComponent]` 和 `[DisallowMultipleComponent]` 特性
- 使用 `EventDispatcher` 进行组件解耦
- 实现 `ISerializationCallbackReceiver` 进行自定义序列化

### 内存管理
- 对GameObject使用 `ObjectPoolMgr.SpawnGameObject()` 和 `RecycleGameObject()`
- 在可池化组件中实现 `OnSpawn()` 和 `OnDespawn()`
- 使用 `UnityEngine.Pool.ObjectPool<T>` 进行简单对象池
- 及时释放未使用的资源引用

## 资源和资产管理

- **YooAsset集成**: 所有资源通过YooAsset系统管理
- **Bundle结构**: 资源打包在 `Bundles/StandaloneWindows64/` 中，带版本控制
- **流式资源**: 配置数据存储在 `StreamingAssets/` 中
- **资源加载**: 优先使用异步加载以避免主线程阻塞

## 本地化支持

- **多语言**: 通过 `LocalizationManager` 和 `LocalizeTextMeshProUGUI` 内置支持
- **配置**: 本地化配置位于 `Assets/GameMain/Scripts/Common/Config/Locale/`

## 常用实用工具

- **存档系统**: 通过 `RijndaelEncryptionManager` 实现加密存档文件
- **对象池**: 通过 `ObjectPoolMgr` 和 `OnCellObjectPool` 集中管理
- **扩展**: `Framework/Extensions/` 中的实用Unity扩展
- **调试工具**: 开发版本的调试弹窗系统