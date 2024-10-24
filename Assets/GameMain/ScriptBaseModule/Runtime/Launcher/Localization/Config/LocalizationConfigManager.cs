using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace BaseModule
{
    public class LocalizationConfigManager
    {
        private List<LocalizationConfig> localizationConfig;

        public void Init()
        {
            var textAsset = Resources.Load<TextAsset>("Launcher/Config/Localization/localization");
#if ENCRY_IOS && !UNITY_EDITOR
            var dec_txt = DragonU3DSDK.Asset.EncryptDecrypt.Decrypt(textAsset.text);
            localizationConfig = JsonConvert.DeserializeObject<List<LocalizationConfig>>(dec_txt);
#else
            localizationConfig = JsonConvert.DeserializeObject<List<LocalizationConfig>>(textAsset.text);
#endif

            Resources.UnloadAsset(textAsset);
        }

        public string GetText(string key, string language)
        {
            LocalizationConfig config = localizationConfig.Find(x => x.Key == key);
            if (config == null) return String.Empty;
            string text = config.en;
            switch (language)
            {
                case Locale.ENGLISH:
                    text = config.en;
                    break;
                case Locale.FRENCH:
                    text = config.fr;
                    break;
                case Locale.GERMAN:
                    text = config.de;
                    break;
                case Locale.PORTUGUESE:
                    text = config.pt;
                    break;
                case Locale.JAPANESE:
                    text = config.jp;
                    break;
                case Locale.KOREA:
                    text = config.kr;
                    break;
                case Locale.CHINESE_SIMPLIFIED:
                    text = config.zh;
                    break;
                case Locale.CHINESE_TRADITION:
                    text = config.zht;
                    break;
                case Locale.SPANISH:
                    text = config.es;
                    break;
                case Locale.ITALIAN:
                    text = config.it;
                    break;
                case Locale.INDONESIAN:
                    text = config.id;
                    break;
                case Locale.RUSSIAN:
                    text = config.ru;
                    break;
                case Locale.VIETNAMESE:
                    text = config.vi;
                    break;
                case Locale.TURKISH:
                    text = config.tr;
                    break;
                case Locale.THAI:
                    text = config.th;
                    break;
                case Locale.Dutch:
                    text = config.nl;
                    break;
            }

            return text;
        }
    }
}