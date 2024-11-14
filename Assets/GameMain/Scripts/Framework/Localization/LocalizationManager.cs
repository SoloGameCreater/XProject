using System;
using System.Collections.Generic;
using System.Text;
using BaseModule;
using Framework;
using TMPro;
using UnityEngine;
using GameFramework;

namespace Localizetion
{
    public class LocalizationManager
    {
      // 拉取到远端存档后，切换语言的委托事件
        public delegate void LocalizationAction();
        public static event LocalizationAction OnLocalization;

        private string current_locale = Locale.ENGLISH;

        private Dictionary<string, Material> m_CacheMaterial = new Dictionary<string, Material>();
        private Dictionary<string, TMP_FontAsset> m_CacheFont = new Dictionary<string, TMP_FontAsset>();

        private bool isForceShowKey = false;

        private static readonly LocalizationManager instance = new LocalizationManager();
        static LocalizationManager() { }
        private LocalizationManager()
        {
            //EventManager.Instance.Subscribe<ProfileReplacedEvent>(this);
            //EventManager.Instance.Subscribe<ProfileFetchedEvent>(this);
        }
        public static LocalizationManager Instance
        {
            get { return instance; }
        }

        private static readonly StringBuilder m_builder = new StringBuilder();
        private static readonly object m_lock = new object(); // thread safe
        private static string Format(string s, params object[] values)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            if (values.Length < 1) return s; // params is empty

            int vi = 0, start = 0, now = 0, len = s.Length; // value index, current start position, current search position

            lock (m_lock)
            {
                m_builder.Clear();
                while (now < len)
                {
                    if (s[now++] != '%') continue;
                    var c = s[now++];
                    if (c == 's' || c == 'S')
                    {
                        var v = values[vi++];
                        m_builder.Append(s.Substring(start, now - start - 2)).Append(v == null ? string.Empty : v.ToString());
                        start = now;

                        if (vi >= values.Length) break;
                    }
                }
                if (start < len) m_builder.Append(s.Substring(start));

                return m_builder.ToString();
            }
        }

        // 多个%s情况下的不定参替换
        public string GetLocalizedStringWithFormats(string key, params string[] values)
        {
            var newStr = GetLocalizedString(key);

            if (newStr == string.Empty || newStr.Equals(key)) return newStr;

            newStr = Format(string.Format(newStr, values), values);

            return newStr;
        }

        public string GetLocalizedStringWithFormat(string key, string content)
        {
            return GetLocalizedStringWithFormats(key, new string[] { content });
        }

        public string GetLocalizedString(string key)
        {
            if(string.IsNullOrEmpty(key)) return null;
            
            string _key = key.Trim();

            if (String.IsNullOrEmpty(_key))
            {
                return String.Empty;
            }

            if (_key.StartsWith("&key", StringComparison.InvariantCulture))
            {
                _key = _key.Substring(5);
            }

            if (isForceShowKey)
                return _key;
            else
                return GetLocalizedString(_key, current_locale);
        }


        public string GetLocalizedString(string key, string givenLocale)
        {
            string _key = key.Trim();

            if (String.IsNullOrEmpty(_key))
            {
                return String.Empty;
            }

            if (_key.StartsWith("&key", StringComparison.InvariantCulture))
            {
                _key = _key.Substring(5);
            }
            return _key;
        }


        /// <summary>
        /// 匹配当前用户的语言
        /// 1、存档里Local字段为空:根据操作系统语言匹配多语言
        /// 2、存档里Local字段不为空:
        ///    2.1、Local字段在supportedLocale里，直接切换到存档里的语言
        ///    2.2、Local字段不在supportedLocale里，执行1
        /// </summary>
        public void MatchLanguage()
        {
            string userLanguage = Locale.GetSystemLanguage();
            SetCurrentLocale(userLanguage);
        }

        public bool SetCurrentLocale(string locale)
        {
            if (string.IsNullOrEmpty(locale))
            {
                return false;
            }

            if (current_locale == locale)
            {
                return false;
            }

            if (!Locale.supportedLocale.Contains(locale))
            {
                return false;
            }

            current_locale = locale;
            return true;
        }

        public string GetCurrentLocale()
        {
            return current_locale;
        }

        public TMP_FontAsset GetLocaleFont()
        {
            return GetFont(current_locale);
        }

        /// <summary>
        /// 获取字体
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public TMP_FontAsset GetFont(string language)
        {
            string locale = CommonUtils.FirstCharToUpper(language);
            string key = string.Format("LocaleFont_{0} SDF", locale);
            TMP_FontAsset fontAsset = null;
            if(!m_CacheFont.TryGetValue(key, out fontAsset))
            {
                try
                {
                    string path = string.Format("Fonts/{0}/{1}", locale, key);
                    fontAsset = ResourcesManager.Instance.LoadResource<TMP_FontAsset>(path);
                    //fontAsset = Resources.Load<TMP_FontAsset>(path);
                    m_CacheFont.Add(key, fontAsset);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            return fontAsset;
        }

        /// <summary>
        /// 获取材质
        /// </summary>
        /// <param name="language"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        /// todo:默认材质的加载路径，目前不对
        public Material GetMaterial(string language, string suffix)
        {
            string locale = CommonUtils.FirstCharToUpper(language);
            string key = string.Format("LocaleFont_{0} SDF {1}", locale, suffix);
            Material mat = null;

            if (m_CacheMaterial.TryGetValue(key, out mat))
            {
                return mat;
            }
            else
            {
                try
                {
                    string path = string.Format("Fonts/{0}/{1}", locale, key);
                    mat = ResourcesManager.Instance.LoadResource<Material>(path);
                    //mat = Resources.Load<Material>(path);
                    m_CacheMaterial.Add(key, mat);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                return mat;
            }
        }

        public Material GetLocaleMaterial(string suffix)
        {
            return GetMaterial(current_locale, suffix);
        }
    }
}