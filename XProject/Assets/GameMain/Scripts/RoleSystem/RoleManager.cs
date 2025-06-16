using Framework;
using UnityEngine;

namespace RoleSystem
{
    public class RoleManager : GlobalSystem<RoleManager>, IUpdatable, ILateUpdatable
    {
        public RoleGameplay Gameplay { get; private set; }
        public RoleModel Model { get; } = new();

        /// <summary>
        /// 进入角色系统
        /// </summary>
        public void OnEnterRoleSystem()
        {
            // 加载角色数据
            var loaded = LoadRoleData();
            if (!loaded)
            {
                DebugUtil.Log("角色数据加载失败");
                return;
            }

            Gameplay = new RoleGameplay();
            Gameplay.Init();

            DebugUtil.Log("角色系统初始化完成");
        }

        /// <summary>
        /// 加载角色数据
        /// </summary>
        /// <returns>是否加载成功</returns>
        private bool LoadRoleData()
        {
            // TODO: 实现角色数据加载逻辑
            // 例如：从配置文件、服务器或本地存储加载角色信息
            return true;
        }

        public void Update(float deltaTime)
        {
            Gameplay?.OnUpdate(deltaTime);
        }

        public void LateUpdate(float deltaTime)
        {
            Gameplay?.OnLateUpdate(deltaTime);
        }

        /// <summary>
        /// 退出角色系统
        /// </summary>
        public void Exit()
        {
            Gameplay?.Release();
            Gameplay = null;
            DebugUtil.Log("角色系统已退出");
        }

        /// <summary>
        /// 重置角色系统
        /// </summary>
        public void Reset()
        {
            Exit();
            OnEnterRoleSystem();
        }
    }
}
