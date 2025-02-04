using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.Pool;

namespace TripleMerge
{
    public class TripleMergeMapManager : TripleMergeComponent
    {
        // map root prefab name
        const string MapRootPrefabAssetName = "MergeMapRoot";
        const string RegionPrefabAssetName = "Region_1";

        public GameObject CellNode { get; private set; }
        public bool IsMerging { get; set; }
        public GameObject MapRoot { get; private set; }
        public Dictionary<Vector2Int, MergeableCell> MergeableCellsDictionary { private set; get; } = new();

        protected override void OnInitialize()
        {
            GameObject mapPrefab = ResourcesManager.Instance.LoadResource<GameObject>($"TripleMerge/Prefabs/{MapRootPrefabAssetName}");
            GameObject regionPrefab = ResourcesManager.Instance.LoadResource<GameObject>($"TripleMerge/Prefabs/Region/{RegionPrefabAssetName}");
            MapRoot = GameObject.Instantiate(mapPrefab, TripleMergeSystem.Instance.Root.transform);
            CellNode = GameObject.Instantiate(regionPrefab, MapRoot.transform.Find("CellNode"));

            LoadMap();
        }

        protected override void OnDispose()
        {
        }

        private void LoadMap()
        {
            MergeableCellsDictionary.Clear();

            var mergeableRegion = CellNode.GetComponent<MergeableRegion>();
            if (mergeableRegion == null) return;

            // 初始化所有区域状态
            var mergeableCells = mergeableRegion.Initialize();
            foreach (var mergeableCell in mergeableCells)
            {
                MergeableCellsDictionary[mergeableCell.MapCoordinate] = mergeableCell;
            }

            ListPool<MergeableCell>.Release(mergeableCells);
            // 加载地块数据
            foreach (var mergeableCell in MergeableCellsDictionary.Values)
            {
                mergeableCell.LoadData();
            }

            // 设置地块对应的上下左右坐标
            foreach (var keyValuePair in MergeableCellsDictionary)
            {
                var coordinate = keyValuePair.Key;
                var cell = keyValuePair.Value;

                // query left
                if (cell.Left == null)
                {
                    if (MergeableCellsDictionary.TryGetValue(coordinate + new Vector2Int(-1, 0), out var leftCell))
                    {
                        cell.Left = leftCell;
                        leftCell.Right = cell;
                    }
                }

                // query right
                if (cell.Right == null)
                {
                    if (MergeableCellsDictionary.TryGetValue(coordinate + new Vector2Int(1, 0), out var rightCell))
                    {
                        cell.Right = rightCell;
                        rightCell.Left = cell;
                    }
                }

                // query above
                if (cell.Above == null)
                {
                    if (MergeableCellsDictionary.TryGetValue(coordinate + new Vector2Int(0, 1), out var aboveCell))
                    {
                        cell.Above = aboveCell;
                        aboveCell.Below = cell;
                    }
                }

                // query below
                if (cell.Below == null)
                {
                    if (MergeableCellsDictionary.TryGetValue(coordinate + new Vector2Int(0, -1), out var belowCell))
                    {
                        cell.Below = belowCell;
                        belowCell.Above = cell;
                    }
                }
            }
        }
    }
}