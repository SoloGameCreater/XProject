using System.Collections.Generic;
using DG.Tweening;
using Framework;
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

        /// <summary>
        /// 产生净化值并分配到未净化的格子中
        /// </summary>
        /// <param name="purificationNum">需要产生的净化值数量</param>
        /// <param name="position">净化值产生的位置</param>
        /// <param name="skipProgress">是否跳过进度动画</param>
        /// <param name="isFromBubble">净化值是否来自泡泡系统</param>
        public void ProducePurification(int purificationNum, Vector3 position, bool skipProgress = false, bool isFromBubble = false)
        {
            // 获取对象池中的列表用于存储未净化的格子
            var unPurifiedCells = ListPool<MergeableCell>.Get();

            // 收集所有未净化的格子
            foreach (var mergeableCell in MergeableCellsDictionary.Values)
            {
                if (mergeableCell.CellStatus != MergeableCell.ECellStatus.UnPurified)
                {
                    continue;
                }

                unPurifiedCells.Add(mergeableCell);
            }

            // 设置最大处理格子数量
            const int maxNum = 5;
            
            // 如果有未净化的格子，进行优先级排序和筛选
            if (unPurifiedCells.Count > 0)
            {
                // 按净化优先级排序
                unPurifiedCells.Sort((left, right) => left.PurifiedPriority - right.PurifiedPriority);
                var lackNum = 0;
                var highPriority = unPurifiedCells[0].PurifiedPriority;
                var lastCellIndex = 0;

                // 遍历格子，计算所需净化值并筛选同优先级的格子
                for (var i = 0; i < unPurifiedCells.Count; i++)
                {
                    var cell = unPurifiedCells[i];
                    lackNum += cell.LackOfPurificationNum;
                    if (lackNum >= purificationNum)
                    {
                        if (cell.PurifiedPriority != highPriority)
                        {
                            break;
                        }
                    }

                    lastCellIndex = i;
                    if (lastCellIndex >= maxNum - 1)
                    {
                        break;
                    }
                }

                // 移除超出范围的格子
                for (var i = unPurifiedCells.Count - 1; i > lastCellIndex; i--)
                {
                    unPurifiedCells.Remove(unPurifiedCells[i]);
                }
            }

            // 获取实际可处理的格子数量
            var pureCellNum = unPurifiedCells.Count;
            
            // 如果没有可净化的格子，处理剩余净化值
            if (pureCellNum == 0)
            {
                if (!isFromBubble)
                {
                    // todo 将净化值添加到泡泡系统中
                    //ThreeMergeSystem.Instance.Gameplay.MapManager.PurificationBubble.AddPurification(purificationNum, position, skipProgress);
                }
                else
                {
                    // 显示无可用土地的提示
                    DebugUtil.LogWarning("无可用土地");
                }

                return;
            }

            // 调整处理格子数量
            if (purificationNum < pureCellNum)
            {
                pureCellNum = Mathf.Min(purificationNum, unPurifiedCells.Count);
            }

            // 计算实际需要的净化值总量
            var lackOfNum = 0;
            for (int i = 0; i < pureCellNum; i++)
            {
                lackOfNum += unPurifiedCells[i].LackOfPurificationNum;
            }

            // 处理多余的净化值
            if (lackOfNum < purificationNum)
            {
                var extraNum = purificationNum - lackOfNum;
                purificationNum -= extraNum;

                if (isFromBubble)
                {
                    if (extraNum > 0)
                    {
                        // todo 将多余的净化值存储到泡泡系统中
                        //ThreeMergeSystem.Instance.Gameplay.MapManager.PurificationBubble.SetPurificationValue(extraNum);
                    }
                }
                else
                {
                    // todo 将多余的净化值返还给泡泡系统
                    //ThreeMergeSystem.Instance.Gameplay.MapManager.PurificationBubble.AddPurification(-extraNum, position, true);
                }
            }
            else
            {
                if (isFromBubble)
                {
                    // todo 将净化值返还给泡泡系统
                    //ThreeMergeSystem.Instance.Gameplay.MapManager.PurificationBubble.AddPurification(-purificationNum, position, true);
                }
            }

            // 设置净化值移动速度
            //const float speed = 13.5f;

            // 初始化净化值分配相关变量
            int produceNum = purificationNum;
            int usedNum = 0;
            bool isProduceNumEqualsLackOfNum = produceNum >= lackOfNum;
            float minDuration = float.MaxValue;
            MergeableCell minDurationCell = null;

            // 分配净化值到各个格子
            for (int i = 1; i <= pureCellNum; i++)
            {
                var targetCell = unPurifiedCells[0];
                unPurifiedCells.Remove(targetCell);

                // 计算当前格子分配的净化值数量
                var randomPurificationNum = 0;
                if (isProduceNumEqualsLackOfNum)
                {
                    randomPurificationNum = targetCell.LackOfPurificationNum;
                }
                else
                {
                    randomPurificationNum = i < pureCellNum ? Random.Range(1, purificationNum - (pureCellNum - i)) : purificationNum;
                    if (randomPurificationNum > targetCell.LackOfPurificationNum)
                    {
                        randomPurificationNum = targetCell.LackOfPurificationNum;
                    }
                }

                // 更新净化值计数
                purificationNum -= randomPurificationNum;
                usedNum += randomPurificationNum;

                // 添加净化值到目标格子
                var isBecomeMergeableThisTime = targetCell.AddPurification(randomPurificationNum);

                // 处理净化值到达效果
                if (skipProgress)
                {
                    targetCell.OnPurificationArrive(isBecomeMergeableThisTime, true);
                }
                else
                {
                    // todo 创建净化值移动特效
                    // var purificationEffect = ThreeMergeEffectPool.Get(ThreeMergeEffectPool.VfxType.PurifyValueFly);
                    // purificationEffect.transform.position = position;
                    // var duration = Vector2.Distance(targetCell.transform.position, position) / speed;
                    
                    // // 记录最短移动时间
                    // if (minDurationCell == null || duration < minDuration)
                    // {
                    //     minDurationCell = targetCell;
                    //     minDuration = duration;
                    // }

                    // // 设置净化值移动动画
                    // purificationEffect.transform.DOMove(targetCell.transform.position, duration).OnComplete(() =>
                    // {
                    //     ThreeMergeEffectPool.Recycle(purificationEffect);
                    //     targetCell.OnPurificationArrive(isBecomeMergeableThisTime, false);
                    // });
                }
            }

            // 播放完成音效
            if (minDurationCell != null)
            {
                minDurationCell.transform.DOScaleX(1, minDuration).OnComplete(() =>
                {
                    DebugUtil.Log("播放净化完成音效");
                    //ThreeMergeAudioPlayer.PlayAudioOnce("sfx_merge3_energy_complete"); 
                });
            }

            // 播放移动音效
            if (!skipProgress)
            {
                DebugUtil.Log("播放净化移动音效");
                //ThreeMergeAudioPlayer.PlayAudio("sfx_merge3_energy_fly");
            }

            // 释放对象池中的列表
            ListPool<MergeableCell>.Release(unPurifiedCells);

            // 处理剩余的净化值
            var remainNum = produceNum - usedNum;
            if (remainNum > 0)
            {
                Debug.Log($"本次分配给三个格子过后，还剩余有{remainNum}点净化值");
                ProducePurification(remainNum, position, skipProgress, isFromBubble);
            }
        }

    }
}