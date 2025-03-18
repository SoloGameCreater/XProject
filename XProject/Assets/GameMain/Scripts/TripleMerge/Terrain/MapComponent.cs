using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace TripleMerge
{
    public class MapAreaComponent : MonoBehaviour
    {
        /// <summary>
        /// 相机在地图上的默认初始点位
        /// </summary>
        public Transform CameraInitPoint { private set; get; }

        /// <summary>
        /// 相机在地图上的最小位置点位
        /// </summary>
        public Transform CameraMinPoint { private set; get; }

        /// <summary>
        /// 相机在地图上的最大位置点位
        /// </summary>
        public Transform CameraMaxPoint { private set; get; }
        /// <summary>
        /// 相机的size控制
        /// </summary>
        public float CameraMinScaler { private set; get; }
        public float CameraMaxScaler { private set; get; }
        public Dictionary<Vector2Int, MergeableCell> MergeableCellsDictionary { private set; get; } = new();
        
        //public Dictionary<int, MapAreaRegion> AreaRegionDictionary { private set; get; } = new();
        public void Initialize()
        {
            InitRegions();
            
            InitMergeableRegion();
        }

        public void InitMapCamera(Transform cameraRoot)
        {
            CameraMinPoint = cameraRoot.Find("MinPosition");
            CameraMaxPoint = cameraRoot.Find("MaxPosition");
            CameraInitPoint = cameraRoot.Find("InitPosition");
            CameraMinScaler = 3f;
            CameraMaxScaler = 9f;
        }
        private void InitRegions()
        {
            
        }
        private void InitMergeableRegion()
        {
            MergeableCellsDictionary.Clear();

            var mergeableRegion = transform.Find("LogicNode/MergeableRegion/Region").GetComponent<MergeableRegion>();
            if (mergeableRegion == null) return;

            // 初始化所有区域状态
            var mergeableCells = mergeableRegion.Initialize(this);
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