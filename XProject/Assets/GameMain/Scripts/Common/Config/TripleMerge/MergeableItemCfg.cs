using System.Collections.Generic;

namespace Config.TripleMerge
{
    public class MergeableItemCfg
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
        /// 1景观|2人物|3动物|4精灵果|; 5礼盒|6装修币箱|7体力宝箱|; 8金币宝箱|9城堡升级装修币|10万能牌|; 11元素宝箱|12上锁宝箱
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
        /// 合成链ID
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
        /// SPINE资源
        /// </summary>
        public string SpinePrefab { get; set; }
        
        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }
        
        /// <summary>
        /// 出售价格(装修币)
        /// </summary>
        public int SalePrice { get; set; }
        
        /// <summary>
        /// 购买价格(装修币)
        /// </summary>
        public int PurchasePrice { get; set; }
        
        /// <summary>
        /// 计费点ID
        /// </summary>
        public int ShopId { get; set; }
        
        /// <summary>
        /// 气球价格(装修币)
        /// </summary>
        public int BalloonPrice { get; set; }
        
        /// <summary>
        /// 气球价格(关联商品)
        /// </summary>
        public int BalloonShopId { get; set; }
        
        /// <summary>
        /// 是否能与万能牌进行合成
        /// </summary>
        public bool CanMergeWithOmnipotentCard { get; set; }
        
        /// <summary>
        /// 使用方式（0无法使用，1点击使用，2自动使用）
        /// </summary>
        public int UseWay { get; set; }
        
        /// <summary>
        /// 产出物品（物品ID，数量）
        /// </summary>
        public List<int> Produce { get; set; }
        
        /// <summary>
        /// 产出间隔(S)
        /// </summary>
        public int ProduceInterval { get; set; }
        
        /// <summary>
        /// 图标飞行目的地
        /// </summary>
        public int FlyTo { get; set; }
        
        /// <summary>
        /// 气泡Y轴位置（距离贴图顶点的偏移量）
        /// </summary>
        public float BubblePos { get; set; }
        
        /// <summary>
        /// 是否显示闪光特效
        /// </summary>
        public bool IsDisplayLightVfx { get; set; }
        
        /// <summary>
        /// 上锁宝箱开出奖励
        /// </summary>
        public string LockableBoxRewards { get; set; }
        
        /// <summary>
        /// 初次合成时的引导组
        /// </summary>
        public int FirstTimeMergedGuidanceId { get; set; }
    }
}