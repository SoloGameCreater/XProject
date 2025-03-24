# DragonU3DSDK 存储系统文档

## 概述

DragonU3DSDK的存储系统是一个用于管理游戏数据持久化的框架，它支持本地存储和远程服务器同步，保证数据的安全性和一致性。该系统采用类型安全的方式组织数据，并能处理存档冲突等复杂情况。

## 核心组件

### StorageManager

StorageManager是整个存储系统的中心管理器，负责存储数据的初始化、读取、保存和同步。

1. **版本管理**
   - `LocalVersion`：本地存档版本号
   - `RemoteVersionACK`：已确认的服务器存档版本号
   - `RemoteVersionSYN`：上传到服务器的本地版本号

2. **关键功能**
   - 本地存储管理
   - 服务器数据同步
   - 存档冲突解决
   - 数据加密

### StorageBase

所有存储类的基类，提供了基础的数据操作和清理功能。

### 特殊存储类型

- `StorageDictionary<TKey, TValue>`：存储键值对数据
- `StorageList<TValue>`：存储列表数据
- `MemoryInt`：特殊的整数存储类型，具有安全性考虑

## 工作流程

1. **初始化**
   ```csharp
   StorageManager.Instance.Init(new List<StorageBase>());
   ```

2. **数据访问**
   ```csharp
   StorageCook cook = StorageManager.Instance.GetStorage<StorageCook>();
   cook.Coins["island_1"] = 200;
   ```

3. **数据同步**
   - 自动本地同步：定期将数据保存到PlayerPrefs
   - 自动远程同步：定期将数据上传到服务器
   - 强制同步：对于重要数据变更，可以标记为强制同步

4. **存档冲突处理**
   - 检测本地版本与服务器版本不一致时触发
   - 可以选择使用服务器版本或保留本地版本
   - 必要时可以合并数据

## 数据安全

1. **数据加密**
   - 使用RijndaelManager进行数据加密存储
   - 对敏感数据采用特殊混淆技术（如MemoryInt）

2. **版本控制**
   - 使用递增的版本号确保数据一致性
   - 可检测和解决多设备间的数据冲突

## 实现细节

1. **本地存储**
   - 数据以JSON格式序列化
   - 加密后存储在PlayerPrefs中

2. **远程同步**
   - 通过API接口上传和下载Profile数据
   - 处理网络错误和版本冲突

3. **事件通知**
   - 提供各种事件回调以便游戏逻辑响应存储变化
   - 如：存档冲突事件、存档替换事件等

## 最佳实践

1. 对于重要数据更改，设置`SyncForce = true`确保立即保存
2. 使用StorageManager.onRebuild事件响应数据重建
3. 为敏感数据使用MemoryInt等安全存储方式
4. 避免频繁修改存储数据以降低性能开销

## 注意事项

1. 存储管理器必须且只能初始化一次
2. 更改存储数据将自动增加LocalVersion
3. 删除存储数据时会有警告日志，需谨慎操作
4. 存储冲突需要适当处理，避免数据丢失 