
using UnityEngine;

namespace TripleMerge
{
    public class MergeableObject : OnCellObject
    {
        public bool IsMerging { get; set; }

        public bool IsGray { get; set; }
        protected override void OnCfgDataUpdated()
        {
            Debug.LogWarning("配置更新");
        }
    }
}