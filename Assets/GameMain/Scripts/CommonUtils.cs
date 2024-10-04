using GameFramework.Localization;

public static class CommonUtils
{
    public static string GetFontAssetNameWithLanguage(Language language)
    {
        return language switch
               {
                   Language.ChineseSimplified => "zh",
                   Language.English => "LiberationSans",
                   _ => "LiberationSans"
               };
    }
}