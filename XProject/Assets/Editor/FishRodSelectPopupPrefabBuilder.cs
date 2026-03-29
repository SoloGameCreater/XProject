using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class FishRodSelectPopupPrefabBuilder
{
    private static readonly Color OverlayColor = new(0f, 0f, 0f, 0.6f);
    private static readonly Color DialogColor = new(0.03f, 0.08f, 0.12f, 0.97f);
    private static readonly Color TitleBarColor = new(0.06f, 0.14f, 0.20f, 0.88f);
    private static readonly Color AccentColor = new(0.20f, 0.72f, 0.86f, 0.92f);
    private static readonly Color MutedTextColor = new(0.72f, 0.83f, 0.88f, 1f);
    private static readonly Color CloseBtnColor = new(0.58f, 0.22f, 0.22f);

    private const string TargetFolderPath = "Assets/ExtraRes/Prefabs/UI/UIFishGame";
    private const string PrefabPath = TargetFolderPath + "/FishRodSelectPopup.prefab";

    [MenuItem("Tools/UI/生成 FishRodSelect 弹窗 Prefab")]
    public static void BuildPrefab()
    {
        EnsureFolder(TargetFolderPath);

        var root = new GameObject("FishRodSelectPopup", typeof(RectTransform), typeof(Image));
        try
        {
            SetFullStretch(root.GetComponent<RectTransform>());
            root.GetComponent<Image>().color = OverlayColor;
            SetLayerRecursively(root, 5);

            CreateDialog(root.transform);

            PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"FishRodSelect 弹窗 Prefab 已生成: {PrefabPath}");
        }
        finally
        {
            Object.DestroyImmediate(root);
        }
    }

    private static void CreateDialog(Transform parent)
    {
        var dialog = CreatePanel("DialogPanel", parent, DialogColor);
        SetRect(dialog, Vector2.zero, new Vector2(820f, 620f), new Vector2(0.5f, 0.5f));

        CreateTitleBar(dialog);
        CreateScrollView(dialog);
    }

    private static void CreateTitleBar(RectTransform dialog)
    {
        var titleBar = CreatePanel("TitleBar", dialog, TitleBarColor);
        titleBar.anchorMin = new Vector2(0f, 1f);
        titleBar.anchorMax = new Vector2(1f, 1f);
        titleBar.pivot = new Vector2(0.5f, 1f);
        titleBar.offsetMin = new Vector2(0f, -56f);
        titleBar.offsetMax = Vector2.zero;

        var titleText = CreateText("TitleText", titleBar, "选择鱼竿", 26, TextAnchor.MiddleLeft, FontStyle.Bold, Color.white);
        SetRect((RectTransform)titleText.transform, new Vector2(20f, 0f), new Vector2(240f, 56f), new Vector2(0f, 0.5f));

        var coinText = CreateText("CoinText", titleBar, "金币: 0", 20, TextAnchor.MiddleRight, FontStyle.Normal, AccentColor);
        SetRect((RectTransform)coinText.transform, new Vector2(-100f, 0f), new Vector2(200f, 56f), new Vector2(1f, 0.5f));

        CreateCloseButton(titleBar);
    }

    private static void CreateCloseButton(RectTransform titleBar)
    {
        var btnObj = new GameObject("CloseButton", typeof(RectTransform), typeof(Image), typeof(Button));
        btnObj.transform.SetParent(titleBar, false);
        btnObj.layer = 5;

        var btnRect = btnObj.GetComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(1f, 0.5f);
        btnRect.anchorMax = new Vector2(1f, 0.5f);
        btnRect.pivot = new Vector2(1f, 0.5f);
        btnRect.anchoredPosition = new Vector2(-12f, 0f);
        btnRect.sizeDelta = new Vector2(72f, 36f);

        var btnImage = btnObj.GetComponent<Image>();
        btnImage.color = CloseBtnColor;
        btnImage.raycastTarget = true;

        var button = btnObj.GetComponent<Button>();
        button.targetGraphic = btnImage;

        var label = CreateText("Label", btnObj.transform, "关闭", 18, TextAnchor.MiddleCenter, FontStyle.Bold, Color.white);
        SetStretch((RectTransform)label.transform, Vector2.zero, Vector2.zero);
    }

    private static void CreateScrollView(RectTransform dialog)
    {
        var scrollObj = new GameObject("ScrollView", typeof(RectTransform), typeof(ScrollRect));
        scrollObj.transform.SetParent(dialog, false);
        scrollObj.layer = 5;

        var scrollRect = scrollObj.GetComponent<RectTransform>();
        scrollRect.anchorMin = Vector2.zero;
        scrollRect.anchorMax = Vector2.one;
        scrollRect.offsetMin = new Vector2(16f, 16f);
        scrollRect.offsetMax = new Vector2(-16f, -56f);

        var viewportObj = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(Mask));
        viewportObj.transform.SetParent(scrollObj.transform, false);
        viewportObj.layer = 5;

        var viewportRect = viewportObj.GetComponent<RectTransform>();
        SetFullStretch(viewportRect);

        var viewportImage = viewportObj.GetComponent<Image>();
        viewportImage.color = new Color(1f, 1f, 1f, 0.01f);
        viewportImage.raycastTarget = true;

        var mask = viewportObj.GetComponent<Mask>();
        mask.showMaskGraphic = false;

        var contentObj = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
        contentObj.transform.SetParent(viewportObj.transform, false);
        contentObj.layer = 5;

        var contentRect = contentObj.GetComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0f, 1f);
        contentRect.anchorMax = new Vector2(1f, 1f);
        contentRect.pivot = new Vector2(0.5f, 1f);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = new Vector2(0f, 0f);

        var vlg = contentObj.GetComponent<VerticalLayoutGroup>();
        vlg.spacing = 8f;
        vlg.padding = new RectOffset(0, 0, 8, 8);
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.childControlWidth = true;
        vlg.childControlHeight = false;

        var csf = contentObj.GetComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        var scroll = scrollObj.GetComponent<ScrollRect>();
        scroll.viewport = viewportRect;
        scroll.content = contentRect;
        scroll.horizontal = false;
        scroll.vertical = true;
        scroll.movementType = ScrollRect.MovementType.Clamped;
        scroll.scrollSensitivity = 30f;
    }

    private static RectTransform CreatePanel(string name, Transform parent, Color color)
    {
        var obj = new GameObject(name, typeof(RectTransform), typeof(Image));
        obj.transform.SetParent(parent, false);
        obj.layer = 5;
        obj.GetComponent<Image>().color = color;
        return obj.GetComponent<RectTransform>();
    }

    private static Text CreateText(string name, Transform parent, string content, int fontSize, TextAnchor alignment, FontStyle fontStyle, Color color)
    {
        var obj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        obj.transform.SetParent(parent, false);
        obj.layer = 5;
        var text = obj.GetComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.text = content;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.fontStyle = fontStyle;
        text.color = color;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;
        return text;
    }

    private static void SetFullStretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
    }

    private static void SetRect(RectTransform rect, Vector2 anchoredPosition, Vector2 size, Vector2 anchor)
    {
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = anchor;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = size;
    }

    private static void SetStretch(RectTransform rect, Vector2 offsetMin, Vector2 offsetMax)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = offsetMin;
        rect.offsetMax = -offsetMax;
    }

    private static void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform child in go.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
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
