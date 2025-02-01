using Framework;
using UnityEngine;

namespace TripleMerge
{
    public class TripleMergeMapManager : TripleMergeComponent
    {
        // map root prefab name
        const string MapRootPrefabAssetName = "MergeMapRoot";
        
        public GameObject MapRoot { get;private set; }

        protected override void OnInitialize()
        {
            GameObject match3DScenePrefab = ResourcesManager.Instance.LoadResource<GameObject>($"TripleMerge/Prefabs/{MapRootPrefabAssetName}");
            MapRoot = GameObject.Instantiate(match3DScenePrefab, TripleMergeSystem.Instance.Root.transform);
        }

        protected override void OnDispose()
        {
            
        }

        private void LoadMap()
        {
            
        }
    }
}