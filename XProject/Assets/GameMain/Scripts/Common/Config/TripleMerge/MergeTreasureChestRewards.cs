using System.Collections.Generic;

namespace Config.TripleMerge
{
    public class MergeTreasureChestRewards
    {
        /// <summary>
        /// 第几次开启
        /// </summary>
        public int Index { get; set; }
        
        /// <summary>
        /// 开出奖励
        /// </summary>
        public List<int> Rewards { get; set; }
    }
}