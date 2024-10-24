using UnityEditor;
using UnityEngine;

namespace BaseModule
{
    public static class AltasConfigSet
    {
        [MenuItem("YooAsset/RefreshAtlasConfig")]
        public static void RefreshAtlasConfig()
        {
            AtlasConfigController asset = ScriptableObject.CreateInstance<AtlasConfigController>();
            asset.ParseAtlasPath("/Export/Activity");
            asset.ParseAtlasPath("/Export/SpriteAtlas");

            AssetDatabase.CreateAsset(asset, "Assets/Resources/Settings/AtlasConfigController.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}