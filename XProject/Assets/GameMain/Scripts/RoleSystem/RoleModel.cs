using Framework;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    [System.Serializable]
    public class RoleData
    {
        public int roleId;
        public string roleName;
        public int level;
        public float experience;
        public Dictionary<string, object> attributes;

        public RoleData()
        {
            attributes = new Dictionary<string, object>();
        }
    }

    public class RoleModel
    {
        private Dictionary<int, RoleData> _roles;
        private int _currentRoleId;

        public RoleModel()
        {
            _roles = new Dictionary<int, RoleData>();
            _currentRoleId = -1;
        }

        /// <summary>
        /// 获取当前选中的角色
        /// </summary>
        public RoleData CurrentRole => GetRole(_currentRoleId);

        /// <summary>
        /// 设置当前角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        public void SetCurrentRole(int roleId)
        {
            if (_roles.ContainsKey(roleId))
            {
                _currentRoleId = roleId;
                DebugUtil.Log($"切换到角色: {roleId}");
            }
            else
            {
                DebugUtil.Log($"角色 {roleId} 不存在");
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleData">角色数据</param>
        public void AddRole(RoleData roleData)
        {
            if (roleData != null && !_roles.ContainsKey(roleData.roleId))
            {
                _roles[roleData.roleId] = roleData;
                DebugUtil.Log($"添加角色: {roleData.roleName} (ID: {roleData.roleId})");
            }
        }

        /// <summary>
        /// 获取角色数据
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>角色数据</returns>
        public RoleData GetRole(int roleId)
        {
            _roles.TryGetValue(roleId, out var role);
            return role;
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns>所有角色数据</returns>
        public IReadOnlyDictionary<int, RoleData> GetAllRoles()
        {
            return _roles;
        }

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        public bool RemoveRole(int roleId)
        {
            if (_roles.Remove(roleId))
            {
                if (_currentRoleId == roleId)
                {
                    _currentRoleId = -1;
                }
                DebugUtil.Log($"移除角色: {roleId}");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 清空所有角色数据
        /// </summary>
        public void Clear()
        {
            _roles.Clear();
            _currentRoleId = -1;
            DebugUtil.Log("清空所有角色数据");
        }
    }
} 