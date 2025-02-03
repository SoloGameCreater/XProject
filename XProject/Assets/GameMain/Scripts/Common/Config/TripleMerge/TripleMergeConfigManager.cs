using System;
using System.Collections.Generic;
using Framework;
using Newtonsoft.Json;
using UnityEngine;

namespace Config.TripleMerge
{
    public class TripleMergeConfigManager : GlobalSystem<TripleMergeConfigManager>
    {
        public List<MergeableItemCfg> MergeableItemCfgList => GetConfig<MergeableItemCfg>();
        private List<MergeableItemCfg> mergeableItemCfgList;
        
        private readonly Dictionary<Type, string> typeToEnum = new Dictionary<Type,string> { 
            [typeof(MergeableItemCfg)] = "MergeableItem",
            
        };
        private void TryLoadConfig(string subModule)
        {
            switch (subModule)
            { 
                case "MergeableItem": if (mergeableItemCfgList != null) return; break;
                
                default: throw new ArgumentOutOfRangeException(nameof(subModule), subModule, null);
            }
            var path = $"Configs/DataJson/{subModule}";
            var ta = ResourcesManager.Instance.LoadResource<TextAsset>(path);
            if (string.IsNullOrEmpty(ta.text))
            {
                DebugUtil.LogError($"Load {path} error!");
                return;
            }
            switch (subModule)
            { 
                case "MergeableItem": mergeableItemCfgList = JsonConvert.DeserializeObject<List<MergeableItemCfg>>(ta.text); break;
                
                default: throw new ArgumentOutOfRangeException(nameof(subModule), subModule, null);
            }
        }
        private List<T> GetConfig<T>()
        {
            var subModule = typeToEnum[typeof(T)];
            TryLoadConfig(subModule);
            switch (subModule)
            { 
                case "MergeableItem": return mergeableItemCfgList as List<T>;
                
                default: throw new ArgumentOutOfRangeException(nameof(subModule), subModule, null);
            }
        }
    }
}