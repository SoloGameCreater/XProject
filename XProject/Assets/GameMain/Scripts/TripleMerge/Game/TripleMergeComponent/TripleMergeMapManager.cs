using Framework;
using UnityEngine;

namespace TripleMerge
{
    public class TripleMergeMapManager : TripleMergeComponent
    {
        // map root prefab name
        const string MapRootPrefabAssetName = "MergeMapRoot";
        const string RegionPrefabAssetName = "Region_1";

        public GameObject CellNode { get; private set; }
        public bool IsMerging { get; set; }
        public GameObject MapRoot { get;private set; }

        protected override void OnInitialize()
        {
            GameObject mapPrefab = ResourcesManager.Instance.LoadResource<GameObject>($"TripleMerge/Prefabs/{MapRootPrefabAssetName}");
            GameObject regionPrefab = ResourcesManager.Instance.LoadResource<GameObject>($"TripleMerge/Prefabs/Region/{RegionPrefabAssetName}");
            MapRoot = GameObject.Instantiate(mapPrefab, TripleMergeSystem.Instance.Root.transform);
            CellNode = GameObject.Instantiate(regionPrefab, MapRoot.transform.Find("ItemNode"));

            LoadMap();
        }

        protected override void OnDispose()
        {
            
        }

        private void LoadMap()
        {
            
        }
    }
}