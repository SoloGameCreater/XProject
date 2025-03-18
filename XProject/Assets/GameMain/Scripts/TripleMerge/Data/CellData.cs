using Newtonsoft.Json;
using System.Collections.Generic;

namespace TripleMerge
{
    /// <summary>
    /// 地图数据类
    /// </summary>
    [System.Serializable]
    public class MapData : UnityEngine.ISerializationCallbackReceiver
    {
        // 由于Unity的JsonUtility不支持直接序列化Dictionary，我们需要一个自定义的序列化器
        [System.Serializable]
        public class CellDictionary
        {
            public string key;
            public CellData value;
        }

        public List<CellDictionary> cellList = new List<CellDictionary>();

        // 这个字段不会被序列化，仅用于内部处理
        [System.NonSerialized]
        public Dictionary<string, CellData> cells = new Dictionary<string, CellData>();

        // 在序列化前将Dictionary转换为List
        public void OnBeforeSerialize()
        {
            cellList.Clear();
            if (cells != null)
            {
                foreach (var pair in cells)
                {
                    cellList.Add(new CellDictionary { key = pair.Key, value = pair.Value });
                }
            }
        }

        // 在反序列化后将List转换回Dictionary
        public void OnAfterDeserialize()
        {
            cells = new Dictionary<string, CellData>();
            foreach (var item in cellList)
            {
                if (item.key != null && item.value != null)
                {
                    cells[item.key] = item.value;
                }
            }
        }
    }

    /// <summary>
    /// 单元格数据类
    /// </summary>
    [System.Serializable]
    public class CellData
    {
        public int belongRegionId;
        public int cellStatus;
        public int purifiedPriority;
        public int requiredPurifiedNum;
        public int initialPlacedItemId;
        public int[] mapCoordinate; // [x, y]
    }
}