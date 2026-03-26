using FishGameRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class FishGameUIPrefabGenerator
{
    private const string TargetFolderPath = "Assets/ExtraRes/Prefabs/UI/UIFishGame";
    private const string PrefabPath = TargetFolderPath + "/FishGameMainUI.prefab";

    [MenuItem("Tools/UI/生成 FishGame UI Prefab")]
    private static void GeneratePrefab()
    {
        Debug.LogWarning("FishGameUIPrefabGenerator 已转发到 FishGameUIPrefabRefiner，请使用新的布局生成逻辑。");
        FishGameUIPrefabRefiner.RebuildPrefabAsset();
        return;

#if false
        EnsureFolder(TargetFolderPath);

        var root = new GameObject("FishGameMainUI", typeof(RectTransform), typeof(Image));
        try
        {
            SetFullStretch(root.GetComponent<RectTransform>());
            root.GetComponent<Image>().color = new Color(0.05f, 0.12f, 0.18f, 0.9f);
            SetLayerRecursively(root, 5);

            CreateTitle(root.transform);
            var hudPanel = CreatePanel("HudPanel", root.transform, new Color(0f, 0f, 0f, 0.28f));
            SetStretch(hudPanel, new Vector2(24f, 180f), new Vector2(408f, 24f));
            CreateHud(hudPanel);

            var inventoryPanel = CreatePanel("InventoryPanel", root.transform, new Color(0f, 0f, 0f, 0.32f));
            SetStretch(inventoryPanel, new Vector2(1536f, 180f), new Vector2(24f, 24f));
            CreateInventory(inventoryPanel);

            var notifyPanel = CreatePanel("NotifyPanel", root.transform, new Color(0f, 0f, 0f, 0.45f));
            SetRect(notifyPanel, new Vector2(0f, -120f), new Vector2(640f, 60f), new Vector2(0.5f, 1f));
            var notifyText = CreateText("NotifyText", notifyPanel, string.Empty, 24, TextAnchor.MiddleCenter, FontStyle.Bold);
            SetStretch((RectTransform)notifyText.transform, new Vector2(12f, 8f), new Vector2(12f, 8f));

            var controlPanel = CreatePanel("ControlPanel", root.transform, new Color(0f, 0f, 0f, 0.32f));
            SetRect(controlPanel, new Vector2(24f, 24f), new Vector2(1180f, 120f), new Vector2(0f, 0f));
            CreateControls(controlPanel);

            PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"FishGame UI Prefab 已生成: {PrefabPath}");
        }
        finally
        {
            Object.DestroyImmediate(root);
        }
#endif
    }

    private static void CreateTitle(Transform parent)
    {
        var title = CreateText("Title", parent, "Fish Game", 42, TextAnchor.MiddleLeft, FontStyle.Bold);
        SetRect((RectTransform)title.transform, new Vector2(24f, -24f), new Vector2(360f, 54f), new Vector2(0f, 1f));
    }

    private static void CreateHud(RectTransform parent)
    {
        var stateText = CreateText("StateText", parent, "状态：空闲", 28, TextAnchor.MiddleLeft, FontStyle.Bold);
        SetRect((RectTransform)stateText.transform, new Vector2(20f, -20f), new Vector2(340f, 32f), new Vector2(0f, 1f));
        var equipText = CreateText("EquipText", parent, "鱼竿：Basic Rod    鱼饵：Basic Mayfly", 22, TextAnchor.MiddleLeft, FontStyle.Normal);
        SetRect((RectTransform)equipText.transform, new Vector2(20f, -60f), new Vector2(580f, 28f), new Vector2(0f, 1f));
        var scoreText = CreateText("ScoreText", parent, "得分：0", 22, TextAnchor.MiddleLeft, FontStyle.Normal);
        SetRect((RectTransform)scoreText.transform, new Vector2(20f, -96f), new Vector2(360f, 28f), new Vector2(0f, 1f));
        var staminaLabel = CreateText("StaminaLabel", parent, "鱼体力", 20, TextAnchor.MiddleLeft, FontStyle.Normal);
        SetRect((RectTransform)staminaLabel.transform, new Vector2(20f, -146f), new Vector2(120f, 24f), new Vector2(0f, 1f));
        CreateBar("StaminaBar", parent, new Vector2(20f, -174f), new Color(0.31f, 0.78f, 0.31f));
        var tensionLabel = CreateText("TensionLabel", parent, "线张力", 20, TextAnchor.MiddleLeft, FontStyle.Normal);
        SetRect((RectTransform)tensionLabel.transform, new Vector2(20f, -222f), new Vector2(120f, 24f), new Vector2(0f, 1f));
        CreateBar("TensionBar", parent, new Vector2(20f, -250f), new Color(0.86f, 0.26f, 0.26f));
        var lineText = CreateText("LineText", parent, "收线距离：--", 20, TextAnchor.MiddleLeft, FontStyle.Normal);
        SetRect((RectTransform)lineText.transform, new Vector2(20f, -298f), new Vector2(360f, 24f), new Vector2(0f, 1f));
        CreateBar("LineBar", parent, new Vector2(20f, -326f), new Color(0.31f, 0.63f, 1f));
        var hintText = CreateText("HintText", parent, "点击“抛竿 / 收竿”开始，所有交互都通过 UI 完成。", 22, TextAnchor.MiddleLeft, FontStyle.Normal);
        SetRect((RectTransform)hintText.transform, new Vector2(20f, -378f), new Vector2(720f, 52f), new Vector2(0f, 1f));
    }

    private static void CreateInventory(RectTransform parent)
    {
        var title = CreateText("InventoryTitle", parent, "背包", 28, TextAnchor.MiddleLeft, FontStyle.Bold);
        SetRect((RectTransform)title.transform, new Vector2(20f, -20f), new Vector2(120f, 32f), new Vector2(0f, 1f));
        var content = CreateText("InventoryText", parent, "背包为空", 20, TextAnchor.UpperLeft, FontStyle.Normal);
        SetStretch((RectTransform)content.transform, new Vector2(20f, 20f), new Vector2(20f, 64f));
    }

    private static void CreateControls(RectTransform parent)
    {
        CreateButton("CastButton", parent, "抛竿 / 收竿", new Vector2(24f, 20f), new Vector2(170f, 40f), new Color(0.18f, 0.47f, 0.72f));
        CreateButton("PrevLureButton", parent, "上一种鱼饵", new Vector2(214f, 20f), new Vector2(150f, 40f), new Color(0.18f, 0.47f, 0.72f));
        CreateButton("NextLureButton", parent, "下一种鱼饵", new Vector2(384f, 20f), new Vector2(150f, 40f), new Color(0.18f, 0.47f, 0.72f));
        CreateButton("SwitchButton", parent, "进入三消", new Vector2(554f, 20f), new Vector2(180f, 40f), new Color(0.85f, 0.49f, 0.13f));
        CreateButton("ReelButton", parent, "按住收线", new Vector2(24f, 70f), new Vector2(260f, 34f), new Color(0.14f, 0.49f, 0.21f));
        CreateButton("ReleaseButton", parent, "按住放线", new Vector2(304f, 70f), new Vector2(260f, 34f), new Color(0.16f, 0.32f, 0.63f));
    }

    private static RectTransform CreatePanel(string name, Transform parent, Color color)
    {
        var panelObject = new GameObject(name, typeof(RectTransform), typeof(Image));
        panelObject.transform.SetParent(parent, false);
        panelObject.GetComponent<Image>().color = color;
        panelObject.layer = 5;
        return panelObject.GetComponent<RectTransform>();
    }

    private static Text CreateText(string name, Transform parent, string content, int fontSize, TextAnchor alignment, FontStyle fontStyle)
    {
        var textObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        textObject.transform.SetParent(parent, false);
        textObject.layer = 5;
        var text = textObject.GetComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.text = content;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.fontStyle = fontStyle;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;
        return text;
    }

    private static void CreateBar(string name, Transform parent, Vector2 anchoredPosition, Color fillColor)
    {
        var background = CreatePanel(name, parent, new Color(1f, 1f, 1f, 0.12f));
        SetRect(background, anchoredPosition, new Vector2(320f, 18f), new Vector2(0f, 1f));
        var fillObject = new GameObject("Fill", typeof(RectTransform), typeof(Image));
        fillObject.transform.SetParent(background, false);
        fillObject.layer = 5;
        var fillRect = fillObject.GetComponent<RectTransform>();
        fillRect.anchorMin = new Vector2(0f, 0f);
        fillRect.anchorMax = new Vector2(0f, 1f);
        fillRect.pivot = new Vector2(0f, 0.5f);
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;
        fillRect.sizeDelta = new Vector2(0f, 0f);
        fillObject.GetComponent<Image>().color = fillColor;
    }

    private static void CreateButton(string name, Transform parent, string text, Vector2 anchoredPosition, Vector2 size, Color color)
    {
        var buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(parent, false);
        buttonObject.layer = 5;
        var rect = buttonObject.GetComponent<RectTransform>();
        SetRect(rect, anchoredPosition, size, new Vector2(0f, 0f));
        var image = buttonObject.GetComponent<Image>();
        image.color = color;
        buttonObject.GetComponent<Button>().targetGraphic = image;
        var label = CreateText("Label", buttonObject.transform, text, 18, TextAnchor.MiddleCenter, FontStyle.Bold);
        SetStretch((RectTransform)label.transform, new Vector2(8f, 4f), new Vector2(8f, 4f));
        if (name == "ReelButton" || name == "ReleaseButton")
        {
            buttonObject.AddComponent<FishGameHoldButton>();
        }
    }

    private static void SetLayerRecursively(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
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
