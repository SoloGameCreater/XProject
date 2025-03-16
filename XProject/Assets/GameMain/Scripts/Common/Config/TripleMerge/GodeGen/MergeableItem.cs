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
        /// 品质
        /// </summary>
        public int Quality { get; set; }
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
        /// 物品所占三合地块大小
        /// </summary>
        public List<int> CellSize { get; set; }
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
        /// <summary>
        /// 使用方式（0无法使用，1点击使用，2自动使用）
        /// </summary>
        public int UseWay { get; set; }
        /// <summary>
        /// 产出间隔(s)
        /// </summary>
        public int ProduceInterval { get; set; }
        /// <summary>
        /// 图标飞行目的地
        /// </summary>
        public int FlyTo { get; set; }
        /// <summary>
        /// 是否显示闪光特效
        /// </summary>
        public bool IsDisplayLightVfx { get; set; }
        /// <summary>
        /// 上锁宝箱开出奖励
        /// </summary>
        public string LockableBoxRewards { get; set; }

    }
}