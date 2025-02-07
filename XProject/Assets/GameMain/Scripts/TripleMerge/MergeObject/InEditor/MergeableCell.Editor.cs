using System.Collections.Generic;
using Config.TripleMerge;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace TripleMerge
{
    public partial class MergeableCell
    {
#if UNITY_EDITOR
        private readonly ValueDropdownList<int> _mergeableItemList = new();
        private ValueDropdownList<int> GetMergeableItemConfigList()
        {
            _mergeableItemList.Clear();
            _mergeableItemList.Add(new ValueDropdownItem<int>("不放置任何合成物", 0));

            // 这里是因为美术库和程序库的路径不一样做的特殊处理
            var jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/ExtraRes/Configs/DataJson/MergeableItem.json");
            var itemList = JsonConvert.DeserializeObject<List<MergeableItemCfg>>(jsonAsset.text);
            
            foreach (var item in itemList)
            {
                _mergeableItemList.Add(new ValueDropdownItem<int>($"{item.Name} [ID:{item.Id}]", item.Id));
            }

            return _mergeableItemList;
        }

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

#endif
    }
}