using System.Collections.Generic;
using Config.TripleMerge;
using Extension;
using Framework;
using UnityEngine;
using UnityEngine.Pool;

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
        public MapAreaComponent MapArea { get; private set; }
        public OnCellObject SelectedCellItem { get; private set;  }
        
        public Camera MapCamera { get; private set; }
        protected override void OnInitialize()
        {
            GameObject mapPrefab = ResourcesManager.Instance.LoadResource<GameObject>($"TripleMerge/Prefabs/{MapRootPrefabAssetName}");
            MapRoot = GameObject.Instantiate(mapPrefab, TripleMergeSystem.Instance.Gameplay.MergeRoot);
            CameraController = MapRoot.transform.Find("CameraControl").gameObject.GetOrCreateComponent<TripleMergeCameraComponent>();
            MapCamera = Camera.main;
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
            MapArea = areaNode;
        }

        public void SelectCellItemChanged(OnCellObject cellObject)
        {
            SelectedCellItem = cellObject;
        }
        public bool TryGetNearestEmptyCell(out MergeableCell targetCell)
        {
            return TryGetNearestEmptyCellFromTargetPosition(MapCamera.ViewportToWorldPoint(new Vector2(0.5f, 0.5f)), out targetCell);
        }

        public bool TryGetNearestEmptyCellFromTargetPosition(Vector3 position, out MergeableCell targetCell)
        {
            targetCell = null;

            var allEmptyCells = ListPool<MergeableCell>.Get();

            foreach (var mergeableCell in MapArea.MergeableCellsDictionary.Values)
            {
                //todo 现在还没有自动合成功能
                if (mergeableCell.CellStatus != MergeableCell.ECellStatus.Mergeable /*|| mergeableCell.IsAnyItemFlyingTo*/)
                {
                    continue;
                }

                allEmptyCells.Add(mergeableCell);
            }

            if (allEmptyCells.Count == 0)
            {
                ListPool<MergeableCell>.Release(allEmptyCells);
                return false;
            }

            allEmptyCells.Sort((left, right) =>
            {
                var leftPointDistance = Vector2.Distance(position, left.transform.position);
                var rightPointDistance = Vector2.Distance(position, right.transform.position);

                if (leftPointDistance < rightPointDistance)
                {
                    return -1;
                }

                return leftPointDistance > rightPointDistance ? 1 : 0;
            });

            targetCell = allEmptyCells[0];
            ListPool<MergeableCell>.Release(allEmptyCells);
            return true;
        }
    }
}