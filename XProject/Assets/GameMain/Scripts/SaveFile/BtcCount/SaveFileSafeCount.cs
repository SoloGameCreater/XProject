using System;
using Newtonsoft.Json;

namespace SaveFile
{
    [Serializable]
    public class SaveFileSafeCount : SaveFileBase
    {
        // 加密数量
        [JsonProperty] private float _encryptAmount;

        // 解密数量
        [JsonProperty] private int _decryptAmount;

        public void SetValue(int value)
        {
            if (GetValue() == value)
            {
                return;
            }

            if (value <= 0)
            {
                _encryptAmount = 0.0f;
                _decryptAmount = 0;
            }
            else
            {
                _decryptAmount = (int)Math.Floor(UnityEngine.Random.Range(0.0f, 1.0f) * value);
                ;
                _encryptAmount = (value - _decryptAmount) / 4f;
            }

            // 标记存档脏数据，具体何时落盘由触发点策略决定
            SaveFileManager.Instance.MarkDirty();
        }
        
        public int GetValue()
        {
            return (int)Math.Round(4f * _encryptAmount + _decryptAmount);
        }
    }
}
