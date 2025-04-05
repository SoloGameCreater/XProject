/************************************************
 * Config class is : MapRegion
 ************************************************/

using System;
using System.Collections.Generic;

namespace Config.TripleMerge
{
    public class MapRegion
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 前置解锁地段
        /// </summary>
        public int PreviousRegion { get; set; }
        /// <summary>
        /// 随地段解锁的三合物品列表
        /// </summary>
        public List<int> UnlockMergeableItems { get; set; }
        /// <summary>
        /// 随地段解锁的三合物品奖励数量
        /// </summary>
        public List<int> UnlockMergeableItemNums { get; set; }
        /// <summary>
        /// 解锁后获得的奖励道具
        /// </summary>
        public List<int> UnlockRewardItems { get; set; }
        /// <summary>
        /// 解锁后获得的奖励道具数量
        /// </summary>
        public List<int> UnlockRewardItemNums { get; set; }
        /// <summary>
        /// 是否为付费地段
        /// </summary>
        public bool IsPlayful { get; set; }
        /// <summary>
        /// 当前是否生效（可解锁）
        /// </summary>
        public bool IsValid { get; set; }

    }
}