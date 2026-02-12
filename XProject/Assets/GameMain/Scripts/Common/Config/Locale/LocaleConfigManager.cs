using System.Collections.Generic;
using System.Text.RegularExpressions;
using BaseModule;
using Framework;
using Newtonsoft.Json;
using Rijndael;
using UnityEngine;

namespace Localizetion
{
    public class LocaleConfigManager : Manager<LocaleConfigManager>
    {
        Dictionary<string, Dictionary<string, string>> localeConfigs =
            new Dictionary<string, Dictionary<string, string>>();

        ////////////////
        // 备份版多语言，从app里读取的
        Dictionary<string, Dictionary<string, string>> bakLocaleConfigs =
            new Dictionary<string, Dictionary<string, string>>();

        private bool loadedFromApp = false;

        private HashSet<string> uniqueKeys = new HashSet<string>()
        {
            "UI_common_time_d",
            "UI_common_time_h",
            "UI_common_time_m",
            "UI_common_time_s",
            "UI_lava_finished",
        };

        private Dictionary<string, string> hotKeys = new Dictionary<string, string>();
        private Dictionary<string, string> hotValues = new Dictionary<string, string>();

        public void InitConfigs()
        {
            ClearHotKeys();

            foreach (string locale in Locale.supportedLocale)
            {
                if (localeConfigs.ContainsKey(locale))
                    continue;

                var configPath = "Configs/LocaleConfig/locale_" + locale;
                var ta = ResourcesManager.Instance.LoadResource<TextAsset>(configPath, addToCache: false);
                var content = ta.text;

                //DebugUtil.Log(ta);
                var localeConfig = JsonConvert.DeserializeObject<List<LocaleItemConfig>>(content);

                //DebugUtil.Log("localeConfigs count : " + localeConfig.Count);
                if (localeConfig != null && localeConfig.Count > 0)
                {
                    int cntTemp = 0;
                    Dictionary<string, string> configs = new Dictionary<string, string>();
                    foreach (LocaleItemConfig c in localeConfig)
                    {
#if UNITY_EDITOR
                        cntTemp++;
#endif
                        if (string.IsNullOrEmpty(c.Key))
                        {
                            DebugUtil.LogError($"language is {locale} key is {c.Key} value is {c.Value} index is {cntTemp}");
                            continue;
                        }

                        configs[c.Key] = c.Value;
                    }

                    localeConfigs[locale] = configs;
                }

                OpUtils.ReleaseRes(configPath, ta);
            }

            // 远端的多语言读取完后，更新下loading条上的tips
            GameTextUtils.PickupLocale();
        }

        // 对嵌套的key进行处理
        string ReplaceKeyVlaue(Dictionary<string, Dictionary<string, string>> tempLocaleConfigs, string locale,
                               string key, bool dec = false)
        {
            string value = tempLocaleConfigs[locale][key];
            if (string.IsNullOrEmpty(value))
                return "";

#if ENCRY_IOS && !UNITY_EDITOR
            if (dec)
            {
                var is_exist = hotValues.TryGetValue(key, out string hk); 
                if (string.IsNullOrEmpty(hk))
                {
                    value = ExternalTextCryptoAdapter.DecryptText(value);
                    if (is_exist)
                        hotValues[key] = value;
                }
                else
                {
                    value = hk;
                }
            }
#endif
            MatchCollection Matches = Regex.Matches(value, "%{(.+?)}");

            foreach (Match match in Matches)
                value = value.Replace("%{" + match.Groups[1].Value + "}",
                    GetLocalString(match.Groups[1].Value, locale));

            return value;
        }

        public string GetLocalString(string key, string locale)
        {
            var replaceLocalConfig = ReplaceLocalConfigManager.Instance.GetLocaleConfigs();
            if (replaceLocalConfig.ContainsKey(locale)
             && replaceLocalConfig[locale].ContainsKey(key)) //是否有热更的内容
            {
                return ReplaceKeyVlaue(replaceLocalConfig, locale, key);
            }

#if ENCRY_IOS && !UNITY_EDITOR
            string enc_key = null;
            if (localeConfigs.ContainsKey(locale) && (enc_key =
            ConvertKey(localeConfigs[locale], key)) != null) //从远端多语言里get到了
            {
                return ReplaceKeyVlaue(localeConfigs, locale, enc_key, true);
            }
#else
            if (localeConfigs.ContainsKey(locale) && localeConfigs[locale].ContainsKey(key)) //从远端多语言里get到了
            {
                return ReplaceKeyVlaue(localeConfigs, locale, key);
            }
#endif


            if (bakLocaleConfigs.ContainsKey(locale) && bakLocaleConfigs[locale].ContainsKey(key)) //从备份版多语言里get到了
            {
                return ReplaceKeyVlaue(bakLocaleConfigs, locale, key);
            }


            return key;
        }

        public Dictionary<string, string> GetCurrenLocalFileDic()
        {
            var local = LocalizationManager.Instance.GetCurrentLocale();

            var result = localeConfigs[local];
#if ENCRY_IOS && !UNITY_EDITOR
            Dictionary<string, string> dec_result = new Dictionary<string, string>();
            foreach (var kv in result)
            {
                var k = ExternalTextCryptoAdapter.DecryptText(kv.Key);
                var v = ExternalTextCryptoAdapter.DecryptText(kv.Value);
                dec_result[k] = v;
            }

            result = dec_result;
#endif
            var backLocalDic = bakLocaleConfigs[local];
            result.Merge(backLocalDic);


            return result;
        }

        public string ConvertKey(Dictionary<string, string> dict, string key)
        {
            string enc_key = key;
#if ENCRY_IOS && !UNITY_EDITOR
            var is_exist = hotKeys.TryGetValue(key, out string hk); 
            if (string.IsNullOrEmpty(hk))
            {
                enc_key = ExternalTextCryptoAdapter.EncryptText(enc_key);
                if (is_exist)
                {
                    hotKeys[key] = enc_key;
                    hotValues.TryAdd(enc_key, "");
                }
            }
            else
            {
                enc_key = hk;
            }
#endif
            if (dict.ContainsKey(enc_key))
                return enc_key;
            return null;
        }

        public void ClearHotKeys()
        {
            hotKeys.Clear();
            hotValues.Clear();
            foreach (var key in uniqueKeys)
            {
                hotKeys.TryAdd(key, "");
            }
        }
    }
}
