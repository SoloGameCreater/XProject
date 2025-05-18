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

    }
}