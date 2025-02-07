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
    }
}