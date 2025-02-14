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

            // 更新配置文件版本并保存
            SaveFileManager.Instance.SaveToLocal();
        }
        
        public int GetValue()
        {
            return (int)Math.Round(4f * _encryptAmount + _decryptAmount);
        }
    }
}