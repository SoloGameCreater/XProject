using System;
using System.Collections.Generic;
using BaseModule;
using Localizetion;

namespace Framework
{
    public static class GameTextUtils
    {
        private static Dictionary<string, List<string>> localTipsDict = new Dictionary<string, List<string>>();
        private static readonly List<string> tips = new List<string> {
            "&key.UI_tips_text1",
            "&key.UI_tips_text2",
            "&key.UI_tips_text3",
            "&key.UI_tips_text4",
            "&key.UI_tips_text5",
            "&key.UI_tips_text6",
            "&key.UI_tips_text7",
            "&key.UI_tips_text8",
            "&key.UI_tips_text9",
            "&key.UI_tips_text10",
            "&key.UI_tips_text11",
            "&key.UI_tips_text12"
           };

        public static void PickupLocale()
        {
            localTipsDict.Clear();
            foreach (string locale in Locale.supportedLocale)
            {
                List<string> localeTips = new List<string>();
                foreach (string key in tips)
                {
                    string strNotFind = key.Substring(5);
                    string text = LocalizationManager.Instance.GetLocalizedString(key, locale);
                    localeTips.Add(text);
                }
                localTipsDict.Add(locale, localeTips);
            }
        }

        public static string GetRandomTip()
        {
            if (localTipsDict.Count == 0)
            {
                return "";
            }

            System.Random rand = new System.Random();
            int idx = rand.Next(tips.Count);

            return localTipsDict[LocalizationManager.Instance.GetCurrentLocale()][idx];
        }
    }
}
