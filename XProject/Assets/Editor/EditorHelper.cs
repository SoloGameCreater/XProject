using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using Editor.DataTableTools;

public class EditorHelper : EditorWindow
{
    [MenuItem("Helper/RunGame %#&p")]
    static void Init()
    {
        // 自动拆分一次配置表
        DataTableGeneratorMenu.GenerateDataTables();
        EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
        EditorApplication.isPlaying = true;
    }
    [MenuItem("Helper/Open C# Project")]
    static void OpenCSharpProject()
    {
        string path = "Assets/GameMain/Scripts/Main/Launching.cs";
        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript));
        AssetDatabase.OpenAsset(obj, 0);
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
            Debug.Log("Refreshing assets before entering play mode...");
            AssetDatabase.Refresh();
        }
    }
}