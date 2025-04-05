using System;
using System.Collections.Generic;
using Config.TripleMerge;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace TripleMerge
{
    public partial class MapAreaRegion
    {
        private ValueDropdownList<int> _regionCfgIdList = new();

        private const uint MAX_REGION_COUNT = 9;
        private ValueDropdownList<int> GetValidRegions()
        {
            _regionCfgIdList.Clear();
            _regionCfgIdList.Add(new ValueDropdownItem<int>($"未配置所属地段", 0));

            for (int i = 1; i <= MAX_REGION_COUNT; i++)
            {
                _regionCfgIdList.Add(new ValueDropdownItem<int>($"地段ID:[{i}]", i));
            }

            return _regionCfgIdList;
        }
    }
}