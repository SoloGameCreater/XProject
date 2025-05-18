/************************************************
 * Config class is : ChestOutput
 ************************************************/

using System;
using System.Collections.Generic;

namespace Config.TripleMerge
{
    public class ChestOutput
    {
        /// <summary>
        /// 唯一标识（剧情阶段）
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 物品产出等级范围
        /// </summary>
        public List<int> ItemLevelRange { get; set; }
        /// <summary>
        /// 每个等级出现概率
        /// </summary>
        public List<int> ItemLevelRatio { get; set; }
        /// <summary>
        /// 宝箱产出数量
        /// </summary>
        public int OutputCount { get; set; }

    }
}