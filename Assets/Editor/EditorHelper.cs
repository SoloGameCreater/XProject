using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

public class EditorHelper : EditorWindow
{
    [MenuItem("Helper/RunGame %#&p")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        //EditorHelper window = (EditorHelper)EditorWindow.GetWindow(typeof(EditorHelper));
        string curScene = EditorSceneManager.GetActiveScene().name;
        //EditorApplication.SaveScene();
        EditorSceneManager.OpenScene("Assets/Launcher.unity");
        EditorApplication.isPlaying = true;
        UnityEngine.Debug.Log(curScene);
    }
    [MenuItem("Helper/Open C# Project")]
    static void OpenCSharpProject()
    {
        string path = "Assets/GameMain/Scripts/Base/GameEntry.cs";
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