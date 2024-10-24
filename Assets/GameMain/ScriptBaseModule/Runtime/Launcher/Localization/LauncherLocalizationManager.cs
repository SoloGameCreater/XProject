using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BaseModule
{
    public class LauncherLocalizationManager
    {
        private static LauncherLocalizationManager m_Instance;
        
        public static LauncherLocalizationManager Instance()
        {
            if (m_Instance == null)
            {
                m_Instance = new LauncherLocalizationManager();
            }
            return m_Instance;
        }
        
        private LocalizationConfigManager m_configManager;

        private string m_Language;

        // public LocalizationLanguage Language
        // {
        //     get => m_Language;
        //     set
        //     {
        //         if (m_Language != value)
        //         {
        //             m_Language = value;
        //             PlayerPrefs.SetString(CacheLocalizationSetKey, m_Language.ToString());
        //         }
        //     }
        // }
        
        public LauncherLocalizationManager()
        {
            m_configManager = new LocalizationConfigManager();
            //m_configManager.Init();
            //m_Language = MatchLanguage();
        }

        //先找设置的语言，再找系统语言，都没有找到就使用en
        // private string MatchLanguage()
        // {
        //     string languageStr = GameModule.Setting.GetString("LANGUAGE", "");
        //     if (string.IsNullOrEmpty(languageStr))
        //     {
        //         string userLanguage = Locale.GetSystemLanguage();
        //         languageStr = userLanguage;
        //     }
        //     else
        //     {
        //         if (!Locale.supportedLocale.Contains(languageStr))
        //         {
        //             string userLanguage = Locale.GetSystemLanguage();
        //             languageStr = userLanguage;
        //         }
        //     }
        //     return languageStr;
        // }

        public string GetText(string key)
        {
            string languageTex = m_configManager.GetText(key, m_Language);
            return languageTex;
        }

        public void SetMaterial(Text text)
        {
            // try
            // {
            //     string style = text.fontMaterial.name;
            //     style = style.Replace(" (Instance)", "");
            //     int tempIndex = style.IndexOf(" SDF ") + " SDF ".Length;
            //     style = style.Substring(tempIndex, style.Length - tempIndex);
            //
            //     TMP_FontAsset font = Resources.Load<TMP_FontAsset>($"Fonts/{m_Language}/LocaleFont_{m_Language} SDF");
            //     text.font = font;
            //
            //     Material material = Resources.Load<Material>($"Fonts/{m_Language}/LocaleFont_{m_Language} SDF {style}");
            //     text.fontMaterial = material;
            // }
            // catch (Exception e)
            // {
            //     Debug.LogError(e.ToString());
            // }
        }
    }
}