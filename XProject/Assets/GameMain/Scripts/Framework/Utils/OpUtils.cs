using System;
using Framework;
using UnityEngine;
using Object = UnityEngine.Object;

public class OpUtils
{
    public static void UnloadSpriteAtlas(string atlasName)
    {
        if (string.IsNullOrEmpty(atlasName)) return;
        try
        {
            ResourcesManager.Instance.UnloadSpriteAtlasImmediateVariant(atlasName);
        }
        catch (Exception e)
        {
            DebugUtil.LogError(atlasName);
            DebugUtil.LogError(e.ToString());
        }
    }

    public static void UnloadObjFromBundleManager(string path)
    {
        if (string.IsNullOrEmpty(path)) return;
        try
        {
            ResourcesManager.Instance.ReleaseRes(path.ToLower(), true);
        }
        catch (Exception e)
        {
            DebugUtil.LogError(path);
            DebugUtil.LogError(e.ToString());
        }
    }

    public static void ReleaseRes(string path, object obj)
    {
        try
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            ResourcesManager.Instance.ReleaseRes(path.ToLower(), true);
            if (obj is GameObject)
            {
                GameObject.Destroy(obj as GameObject);
            }
            else if (obj is Component)
            {
                GameObject.Destroy(obj as Component);
            }
            else if (obj is AssetBundle)
            {
                GameObject.Destroy(obj as AssetBundle);
            }
            else
            {
                Resources.UnloadAsset(obj as Object);
            }
        }
        catch (Exception e)
        {
            DebugUtil.LogError(path);
            DebugUtil.LogError(e.ToString());
        }
    }
}