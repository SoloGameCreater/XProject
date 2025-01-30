using Framework;
using UnityEngine;

namespace TripleMerge
{
    public class TripleMergeMapManager : TripleMergeComponent
    {
        // map root prefab name
        const string MapRootPrefabAssetName = "MergeMapRoot";
        
        private GameObject _mapRoot;
        protected override void OnInitialize()
        {
            GameObject match3DScenePrefab = ResourcesManager.Instance.LoadResource<GameObject>($"TripleMerge/Prefabs/{MapRootPrefabAssetName}");
            _mapRoot = GameObject.Instantiate(match3DScenePrefab, TripleMergeSystem.Instance.Root.transform);
        }

        protected override void OnDispose()
        {
            
        }

        private void LoadMap()
        {
            
        }
    }
}