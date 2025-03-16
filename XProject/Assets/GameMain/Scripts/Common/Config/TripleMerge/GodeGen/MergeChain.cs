/************************************************
 * Config class is : MergeChain
 ************************************************/

using System;
using System.Collections.Generic;

namespace Config.TripleMerge
{
    public class MergeChain
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 合成链
        /// </summary>
        public List<int> Chain { get; set; }
        /// <summary>
        /// 在三合商城中出现的顺序
        /// (0为第一个，不希望在商城中出现则配置为-1)
        /// </summary>
        public int ShopShowUpIndex { get; set; }

    }
}