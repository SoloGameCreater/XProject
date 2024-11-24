using System.Collections.Generic;

[System.Serializable]
public class VersionItemInfo
{
    public Dictionary<string, GameAssetBundleInfo> AssetBundles;

    public string Version;

    public bool UpdateWholeGroup;

    public VersionItemInfo()
    {
        AssetBundles = new Dictionary<string, GameAssetBundleInfo>();
    }
}