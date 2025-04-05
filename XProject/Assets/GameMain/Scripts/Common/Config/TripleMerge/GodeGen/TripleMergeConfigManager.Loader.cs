
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Framework;

namespace Config.TripleMerge
{
    public partial class TripleMergeConfigManager
    {   
        
        public List<MergeableItem> MergeableItemList => getConfig<MergeableItem>();
        public List<MergeChain> MergeChainList => getConfig<MergeChain>();
        public List<MapRegion> MapRegionList => getConfig<MapRegion>();
        
        private List<MergeableItem> mergeableitemList;
        private List<MergeChain> mergechainList;
        private List<MapRegion> mapregionList;
        
        private readonly Dictionary<Type, string> typeToEnum = new Dictionary<Type,string> { 
            [typeof(MergeableItem)] = "mergeableitem",
            [typeof(MergeChain)] = "mergechain",
            [typeof(MapRegion)] = "mapregion"
        };
        private void tryLoad(string subModule)
        {
            switch (subModule)
            { 
                case "mergeableitem": if (mergeableitemList != null) return; break;
                case "mergechain": if (mergechainList != null) return; break;
                case "mapregion": if (mapregionList != null) return; break;
                default: throw new ArgumentOutOfRangeException(nameof(subModule), subModule, null);
            }
            var path = $"Configs/DataJson/TripleMerge/{subModule}";
            var ta = ResourcesManager.Instance.LoadResource<TextAsset>(path);
            if (string.IsNullOrEmpty(ta.text))
            {
                DebugUtil.LogError($"Load {path} error!");
                return;
            }
            switch (subModule)
            { 
                case "mergeableitem": mergeableitemList = JsonConvert.DeserializeObject<List<MergeableItem>>(ta.text); break;
                case "mergechain": mergechainList = JsonConvert.DeserializeObject<List<MergeChain>>(ta.text); break;
                case "mapregion": mapregionList = JsonConvert.DeserializeObject<List<MapRegion>>(ta.text); break;
                default: throw new ArgumentOutOfRangeException(nameof(subModule), subModule, null);
            }
        }
        private List<T> getConfig<T>()
        {
            var subModule = typeToEnum[typeof(T)];
            tryLoad(subModule);
            switch (subModule)
            { 
                case "mergeableitem": return mergeableitemList as List<T>;
                case "mergechain": return mergechainList as List<T>;
                case "mapregion": return mapregionList as List<T>;
                default: throw new ArgumentOutOfRangeException(nameof(subModule), subModule, null);
            }
        }
    }
}