using System.Collections.Generic;
using UnityEngine;

namespace BaseModule
{
    public static class Locale
    {
        public const string NONE = "none";
        public const string ENGLISH = "en";
        public const string FRENCH = "fr";
        public const string GERMAN = "de";
        public const string PORTUGUESE = "pt";
        public const string JAPANESE = "jp";
        public const string KOREA = "kr";
        public const string CHINESE_SIMPLIFIED = "zh";
        public const string CHINESE_TRADITION = "zht";
        public const string SPANISH = "es";
        public const string ITALIAN = "it";
        public const string INDONESIAN = "id";
        public const string RUSSIAN = "ru";
        public const string VIETNAMESE = "vi";
        public const string TURKISH = "tr";
        public const string THAI = "th";
        public const string HINDI = "hi";
        public const string Dutch = "nl";// 荷兰
        
        public readonly static List<string> supportedLocale = new List<string> {
            Locale.ENGLISH,
            Locale.GERMAN,
            Locale.FRENCH,
            Locale.KOREA,
            Locale.JAPANESE,
            Locale.SPANISH,
            Locale.PORTUGUESE,
            Locale.CHINESE_SIMPLIFIED,
            Locale.CHINESE_TRADITION,
            Locale.RUSSIAN,
        };
        
        #region 匹配操作系统的语言
        public static string GetSystemLanguage()
        {
            string osLanguage = GetSystemLanguage(Application.systemLanguage);
            return GetLocal(osLanguage);
        }

        public static string GetSystemLanguage(SystemLanguage language)
        {
            switch (language)
            {
                case SystemLanguage.Afrikaans: return "af";
                case SystemLanguage.Arabic: return "ar";
                case SystemLanguage.Basque: return "eu";
                case SystemLanguage.Belarusian: return "be";
                case SystemLanguage.Bulgarian: return "bg";
                case SystemLanguage.Catalan: return "ca";
                case SystemLanguage.Chinese: return "zh";
                case SystemLanguage.ChineseSimplified: return "zh";
                case SystemLanguage.ChineseTraditional: return "zht";
                case SystemLanguage.Czech: return "cs";
                case SystemLanguage.Danish: return "da";
                case SystemLanguage.Dutch: return "nl";
                case SystemLanguage.English: return "en";
                case SystemLanguage.Estonian: return "et";
                case SystemLanguage.Faroese: return "fo";
                case SystemLanguage.Finnish: return "fi";
                case SystemLanguage.French: return "fr";
                case SystemLanguage.German: return "de";
                case SystemLanguage.Greek: return "el";
                case SystemLanguage.Hebrew: return "he";
                case SystemLanguage.Icelandic: return "is";
                case SystemLanguage.Indonesian: return "id";
                case SystemLanguage.Japanese: return "jp";
                case SystemLanguage.Korean: return "kr";
                case SystemLanguage.Latvian: return "lv";
                case SystemLanguage.Lithuanian: return "lt";
                case SystemLanguage.Norwegian: return "no";
                case SystemLanguage.Polish: return "pl";
                case SystemLanguage.Portuguese: return "pt";
                case SystemLanguage.Romanian: return "ro";
                case SystemLanguage.Russian: return "ru";
                case SystemLanguage.SerboCroatian: return "hr";
                case SystemLanguage.Slovak: return "sk";
                case SystemLanguage.Slovenian: return "sl";
                case SystemLanguage.Spanish: return "es";
                case SystemLanguage.Swedish: return "sv";
                case SystemLanguage.Thai: return "th";
                case SystemLanguage.Turkish: return "tr";
                case SystemLanguage.Ukrainian: return "uk";
                case SystemLanguage.Vietnamese: return "vi";
                case SystemLanguage.Hungarian: return "hu";
                case SystemLanguage.Italian: return "it";
                case SystemLanguage.Unknown: return "en";
            }

            return "en";
        }

        // 过滤不支持的语言
        public static string GetLocal(string language)
        {
            if (Locale.supportedLocale.Contains(language))
                return language;

            return Locale.ENGLISH;
        }

        //public void OnNotify(ProfileReplacedEvent message)
        //{
        //    this.MatchLanguage();
        //    //OnLocalization();
        //}

        //public void OnNotify(ProfileFetchedEvent message)
        //{
        //    this.MatchLanguage();
        //    //OnLocalization();
        //}

        #endregion
    }
}