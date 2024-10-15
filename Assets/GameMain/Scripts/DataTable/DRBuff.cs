//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-10-15 16:28:45.481
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    /// <summary>
    /// Buff表。
    /// </summary>
    public class DRBuff : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取buff编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取增减。
        /// </summary>
        public int Addition
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取乘除。
        /// </summary>
        public int Ratio
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否是增加值。
        /// </summary>
        public int IsAdd
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取碰撞特效编号。
        /// </summary>
        public int EffectId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取碰撞声音编号。
        /// </summary>
        public int SoundId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取权重。
        /// </summary>
        public int Weight
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            Addition = int.Parse(columnStrings[index++]);
            Ratio = int.Parse(columnStrings[index++]);
            IsAdd = int.Parse(columnStrings[index++]);
            EffectId = int.Parse(columnStrings[index++]);
            SoundId = int.Parse(columnStrings[index++]);
            Weight = int.Parse(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    Addition = binaryReader.Read7BitEncodedInt32();
                    Ratio = binaryReader.Read7BitEncodedInt32();
                    IsAdd = binaryReader.Read7BitEncodedInt32();
                    EffectId = binaryReader.Read7BitEncodedInt32();
                    SoundId = binaryReader.Read7BitEncodedInt32();
                    Weight = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
