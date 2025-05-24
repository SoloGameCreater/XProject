/************************************************
 * Config class is : MergeableItem
 ************************************************/

using System;
using System.Collections.Generic;

namespace Config.TripleMerge
{
    public class MergeableItem
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 1宝箱
        /// 2合成物
        /// 3万能牌
        /// </summary>
        public int ItemType { get; set; }
        /// <summary>
        /// 初始是否解锁
        /// </summary>
        public bool IsInitUnlock { get; set; }
        /// <summary>
        /// 合成链id
        /// </summary>
        public int ChainId { get; set; }
        /// <summary>
        /// 产出权重
        /// </summary>
        public int ProduceWeight { get; set; }
        /// <summary>
        /// 预制体
        /// </summary>
        public string Prefab { get; set; }
        /// <summary>
        /// 图集
        /// </summary>
        public string Atlas { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 是否能与万能牌进行合成
        /// </summary>
        public bool CanMergeWithOmnipotentCard { get; set; }

    }
}