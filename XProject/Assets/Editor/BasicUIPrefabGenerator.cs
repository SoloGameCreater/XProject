using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class BasicUIPrefabGenerator
{
    private const string TargetFolderPath = "Assets/ExtraRes/Prefabs/UI";
    private const string PrefabFileName = "BaseUIPrefab.prefab";

    [MenuItem("Tools/UI/生成基础UI Prefab")]
    private static void GeneratePrefab()
    {
        // 保证目标目录存在，避免保存 Prefab 时报路径错误。
        EnsureFolder(TargetFolderPath);

        var root = new GameObject("UI_BasePanel", typeof(RectTransform));
        try
        {
            SetFullStretch(root.GetComponent<RectTransform>());
            CreateBackground(root.transform);
            CreateTitle(root.transform);
            CreateCloseButton(root.transform);

            var prefabPath = AssetDatabase.GenerateUniqueAssetPath(TargetFolderPath + "/" + PrefabFileName);
            PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            Selection.activeObject = prefabAsset;
            EditorGUIUtility.PingObject(prefabAsset);
            Debug.Log($"基础 UI Prefab 已生成: {prefabPath}");
        }
        finally
        {
            Object.DestroyImmediate(root);
        }
    }

    private static void CreateBackground(Transform parent)
    {
        var bg = new GameObject("Background", typeof(RectTransform), typeof(Image));
        bg.transform.SetParent(parent, false);

        var rect = bg.GetComponent<RectTransform>();
        SetFullStretch(rect);

        var image = bg.GetComponent<Image>();
        image.color = new Color(0f, 0f, 0f, 0.5f);
    }

    private static void CreateTitle(Transform parent)
    {
        var title = new GameObject("Title", typeof(RectTransform), typeof(TextMeshProUGUI));
        title.transform.SetParent(parent, false);

        var rect = title.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0f, -80f);
        rect.sizeDelta = new Vector2(720f, 120f);

        var text = title.GetComponent<TextMeshProUGUI>();
        ApplyDefaultFont(text);
        text.text = "NEW UI";
        text.fontSize = 56;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        text.raycastTarget = false;
    }

    private static void CreateCloseButton(Transform parent)
    {
        var buttonRoot = new GameObject("CloseButton", typeof(RectTransform), typeof(Image), typeof(Button));
        buttonRoot.transform.SetParent(parent, false);

        var rect = buttonRoot.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.anchoredPosition = new Vector2(-40f, -40f);
        rect.sizeDelta = new Vector2(140f, 70f);

        var image = buttonRoot.GetComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0.25f);

        var button = buttonRoot.GetComponent<Button>();
        button.targetGraphic = image;

        var label = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
        label.transform.SetParent(buttonRoot.transform, false);

        var labelRect = label.GetComponent<RectTransform>();
        SetFullStretch(labelRect);

        var labelText = label.GetComponent<TextMeshProUGUI>();
        ApplyDefaultFont(labelText);
        labelText.text = "CLOSE";
        labelText.fontSize = 34;
        labelText.alignment = TextAlignmentOptions.Center;
        labelText.color = Color.white;
        labelText.raycastTarget = false;
    }

    private static void ApplyDefaultFont(TextMeshProUGUI text)
    {
        var defaultFont = TMP_Settings.defaultFontAsset;
        if (defaultFont == null)
        {
            Debug.LogWarning("未找到 TMP 默认字体资源，生成后请手动指定字体。");
            return;
        }

        text.font = defaultFont;
    }

    private static void SetFullStretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
    }

    private static void EnsureFolder(string folderPath)
    {
        if (AssetDatabase.IsValidFolder(folderPath))
        {
            return;
        }

        var parts = folderPath.Split('/');
        var current = parts[0];
        for (var i = 1; i < parts.Length; i++)
        {
            var next = current + "/" + parts[i];
            if (!AssetDatabase.IsValidFolder(next))
            {
                AssetDatabase.CreateFolder(current, parts[i]);
            }

            current = next;
        }
    }
}
