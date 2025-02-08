using System.Collections.Generic;
using Extension;
using Framework;
using UnityEngine;

namespace TripleMerge
{
    public class TripleMergeMapManager : TripleMergeComponent
    {
        // map root prefab name
        const string MapRootPrefabAssetName = "MapRoot";
        const string AreaPrefabAssetName = "AreaRoot";

        public bool IsMerging { get; set; }
        public GameObject MapRoot { get; private set; }
        public TripleMergeCameraComponent CameraController { get; private set; }
        
        public OnCellObject SelectedCellItem { private set; get; }

        protected override void OnInitialize()
        {
            GameObject mapPrefab = ResourcesManager.Instance.LoadResource<GameObject>($"TripleMerge/Prefabs/{MapRootPrefabAssetName}");
            MapRoot = GameObject.Instantiate(mapPrefab, TripleMergeSystem.Instance.Gameplay.MergeRoot);
            CameraController = MapRoot.transform.Find("CameraControl").gameObject.GetOrCreateComponent<TripleMergeCameraComponent>();
            LoadMap();
        }

        protected override void OnDispose()
        {
        }

        private void LoadMap()
        {
            // 初始化区域
            InitArea();
        }
        private void InitArea()
        {
            GameObject areaPrefab = ResourcesManager.Instance.LoadResource<GameObject>($"TripleMerge/Prefabs/Area/{AreaPrefabAssetName}");
            var areaNode = GameObject.Instantiate(areaPrefab, MapRoot.transform.Find("Area")).AddComponent<MapAreaComponent>();
            areaNode.Initialize();
        }

        public void SelectCellItemChanged(OnCellObject cellObject)
        {
            SelectedCellItem = cellObject;
        }
    }
}