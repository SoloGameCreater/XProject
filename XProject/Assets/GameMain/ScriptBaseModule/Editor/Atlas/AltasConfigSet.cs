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
            asset.ParseAtlasPath("/ExtraRes/SpriteAtlas");

            AssetDatabase.CreateAsset(asset, $"Assets/{AtlasConfigController.AtlasConfigPath}.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}