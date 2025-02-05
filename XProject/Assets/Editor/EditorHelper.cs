using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using DataTableTools;

public class EditorHelper : EditorWindow
{
    [MenuItem("Helper/Open C# Project")]
    static void OpenCSharpProject()
    {
        string path = "Assets/GameMain/Scripts/Main/Launching.cs";
        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript));
        AssetDatabase.OpenAsset(obj, 0);
    }
    [MenuItem("Helper/打开对应场景/游戏场景")]
    static void OpenGameScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
    }
    [MenuItem("Helper/打开对应场景/编辑器场景")]
    static void OpenEditorToolScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MergeScene.unity");
        var rootObj = GameObject.Find("MergeMaoRoot");
        if (rootObj != null)
        {
            DestroyImmediate(rootObj);
        }

        var mapPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/ExtraRes/TripleMerge/Prefabs/MergeMapRoot.prefab");
        PrefabUtility.InstantiatePrefab(mapPrefab);
    }
}

[InitializeOnLoad]
public static class AutoRefreshOnPlay
{
    static AutoRefreshOnPlay()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            // 自动拆分一次配置表
            DataTableGeneratorMenu.GenerateDataTables();
            AssetDatabase.Refresh();
            Debug.Log("Refreshing assets before entering play mode...");
        }
    }
}