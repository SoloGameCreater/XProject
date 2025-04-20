namespace TripleMerge
{
    public enum TripleMergeItemType
    {
        /// <summary>
        /// 宝箱
        /// </summary>
        TreasureChest = 1,
        /// <summary>
        /// 一般合成物
        /// </summary>
        MergeableNormal = 2,
        /// <summary>
        /// 万能卡
        /// </summary>
        UniversalCard = 3,
    }
    public static class Const
    {
        public const string AtlasName = "MergeItemAtlas";
        // 地图UI默认参考相机大小
        public const float BASE_CAMERA_SIZE = 9;
    }
}