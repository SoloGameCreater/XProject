using FishGameRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class FishGameUIPrefabRefiner
{
    private static readonly Color RootColor = new Color(0.03f, 0.08f, 0.12f, 0.97f);
    private static readonly Color CardColor = new Color(0.06f, 0.14f, 0.20f, 0.88f);
    private static readonly Color CardInnerColor = new Color(0.10f, 0.20f, 0.27f, 0.78f);
    private static readonly Color AccentColor = new Color(0.20f, 0.72f, 0.86f, 0.92f);
    private static readonly Color MutedTextColor = new Color(0.72f, 0.83f, 0.88f, 1f);

    private const string TargetFolderPath = "Assets/ExtraRes/Prefabs/UI/UIFishGame";
    private const string PrefabPath = TargetFolderPath + "/FishGameMainUI.prefab";

    [MenuItem("Tools/UI/刷新 FishGame UI 布局")]
    private static void RebuildPrefabMenu()
    {
        RebuildPrefabAsset();
    }

    public static void RebuildPrefabAsset()
    {
        EnsureFolder(TargetFolderPath);

        var root = new GameObject("FishGameMainUI", typeof(RectTransform), typeof(Image));
        try
        {
            SetFullStretch(root.GetComponent<RectTransform>());
            root.GetComponent<Image>().color = RootColor;
            SetLayerRecursively(root, 5);

            CreateBackdrop(root.transform);
            CreateHeader(root.transform);
            CreateHud(root.transform);
            CreateStatusPanel(root.transform);
            CreateInventory(root.transform);
            CreateControls(root.transform);

            PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"FishGame UI 布局已刷新: {PrefabPath}");
        }
        finally
        {
            Object.DestroyImmediate(root);
        }
    }

    private static void CreateBackdrop(Transform parent)
    {
        var topGlow = CreateImage("TopGlow", parent, new Color(0.08f, 0.33f, 0.42f, 0.22f));
        SetTopStretch(topGlow, 0f, 0f, 0f, 300f);

        var rightGlow = CreateImage("RightGlow", parent, new Color(0.14f, 0.44f, 0.48f, 0.16f));
        SetVerticalStretchRight(rightGlow, 0f, 0f, 0f, 460f);

        var bottomFog = CreateImage("BottomFog", parent, new Color(0f, 0f, 0f, 0.18f));
        SetBottomStretch(bottomFog, 0f, 0f, 0f, 240f);

        var horizon = CreateImage("HorizonLine", parent, new Color(0.42f, 0.84f, 0.98f, 0.20f));
        SetRect(horizon, new Vector2(24f, -170f), new Vector2(1240f, 2f), new Vector2(0f, 1f));
    }

    private static void CreateHeader(Transform parent)
    {
        var headerPanel = CreatePanel("HeaderPanel", parent, CardColor);
        SetTopStretch(headerPanel, 24f, 24f, 24f, 140f);

        var titleGroup = CreateContainer("TitleGroup", headerPanel);
        SetStretch(titleGroup, new Vector2(24f, 18f), new Vector2(470f, 18f));

        var eyebrowText = CreateText("EyebrowText", titleGroup, "RIVER LICENSE / SOLO MODE", 14, TextAnchor.UpperLeft, FontStyle.Bold, AccentColor);
        SetRect((RectTransform)eyebrowText.transform, new Vector2(0f, -2f), new Vector2(320f, 22f), new Vector2(0f, 1f));

        var titleText = CreateText("TitleText", titleGroup, "Fish Game", 46, TextAnchor.MiddleLeft, FontStyle.Bold, Color.white);
        SetRect((RectTransform)titleText.transform, new Vector2(0f, -46f), new Vector2(340f, 56f), new Vector2(0f, 1f));

        var equipText = CreateText("EquipText", titleGroup, "装备配置 / Basic Rod / Basic Mayfly", 20, TextAnchor.MiddleLeft, FontStyle.Normal, MutedTextColor);
        SetRect((RectTransform)equipText.transform, new Vector2(0f, -98f), new Vector2(560f, 28f), new Vector2(0f, 1f));

        var stateBadge = CreatePanel("StateBadge", headerPanel, new Color(0.08f, 0.20f, 0.26f, 0.95f));
        SetRect(stateBadge, new Vector2(-256f, -24f), new Vector2(160f, 48f), new Vector2(1f, 1f));
        var stateLabel = CreateText("StateLabel", stateBadge, "当前阶段", 14, TextAnchor.MiddleLeft, FontStyle.Normal, MutedTextColor);
        SetRect((RectTransform)stateLabel.transform, new Vector2(14f, -7f), new Vector2(78f, 18f), new Vector2(0f, 1f));
        var stateText = CreateText("StateText", stateBadge, "空闲", 20, TextAnchor.MiddleRight, FontStyle.Bold, Color.white);
        SetRect((RectTransform)stateText.transform, new Vector2(-14f, -8f), new Vector2(78f, 24f), new Vector2(1f, 1f));

        var scoreCard = CreatePanel("ScoreCard", headerPanel, new Color(0.10f, 0.19f, 0.15f, 0.96f));
        SetRect(scoreCard, new Vector2(-24f, -24f), new Vector2(208f, 92f), new Vector2(1f, 1f));
        var scoreCaption = CreateText("ScoreCaption", scoreCard, "累计得分", 14, TextAnchor.UpperLeft, FontStyle.Normal, new Color(0.82f, 0.93f, 0.88f, 1f));
        SetRect((RectTransform)scoreCaption.transform, new Vector2(16f, -14f), new Vector2(90f, 18f), new Vector2(0f, 1f));
        var scoreText = CreateText("ScoreText", scoreCard, "累计得分\n0", 26, TextAnchor.LowerLeft, FontStyle.Bold, Color.white);
        SetRect((RectTransform)scoreText.transform, new Vector2(16f, 14f), new Vector2(168f, 54f), new Vector2(0f, 0f));
    }

    private static void CreateHud(Transform parent)
    {
        var hudPanel = CreatePanel("HudPanel", parent, CardColor);
        SetRect(hudPanel, new Vector2(24f, -188f), new Vector2(760f, 254f), new Vector2(0f, 1f));

        var sectionTitle = CreateText("SectionTitle", hudPanel, "Fight Metrics", 24, TextAnchor.MiddleLeft, FontStyle.Bold, Color.white);
        SetRect((RectTransform)sectionTitle.transform, new Vector2(20f, -22f), new Vector2(280f, 32f), new Vector2(0f, 1f));

        var sectionDesc = CreateText("SectionDesc", hudPanel, "用张力和距离控制节奏，而不是一直按住收线。", 16, TextAnchor.MiddleLeft, FontStyle.Normal, MutedTextColor);
        SetRect((RectTransform)sectionDesc.transform, new Vector2(20f, -52f), new Vector2(680f, 24f), new Vector2(0f, 1f));

        CreateMetricRow(hudPanel, "StaminaRow", 92f, "鱼体力", "体力决定鱼还能挣扎多久。", "StaminaBar", new Color(0.31f, 0.78f, 0.31f));
        CreateMetricRow(hudPanel, "TensionRow", 150f, "线张力", "高张力会直接断线。", "TensionBar", new Color(0.86f, 0.26f, 0.26f));
        CreateMetricRow(hudPanel, "LineRow", 208f, "收线距离", "数值越低越接近上鱼。", "LineBar", new Color(0.31f, 0.63f, 1f));
    }

    private static void CreateMetricRow(RectTransform parent, string rowName, float topOffset, string label, string desc, string barName, Color fillColor)
    {
        var row = CreateContainer(rowName, parent);
        SetRect(row, new Vector2(20f, -topOffset), new Vector2(720f, 50f), new Vector2(0f, 1f));

        var labelText = CreateText("LabelText", row, label, 18, TextAnchor.UpperLeft, FontStyle.Bold, Color.white);
        SetRect((RectTransform)labelText.transform, new Vector2(0f, -2f), new Vector2(140f, 22f), new Vector2(0f, 1f));

        var descText = CreateText("DescText", row, desc, 14, TextAnchor.UpperLeft, FontStyle.Normal, MutedTextColor);
        SetRect((RectTransform)descText.transform, new Vector2(0f, -24f), new Vector2(300f, 18f), new Vector2(0f, 1f));

        var valueText = CreateText("ValueText", row, "--", 18, TextAnchor.UpperRight, FontStyle.Bold, Color.white);
        SetRect((RectTransform)valueText.transform, new Vector2(-4f, -6f), new Vector2(180f, 22f), new Vector2(1f, 1f));

        var bar = CreatePanel(barName, row, new Color(1f, 1f, 1f, 0.10f));
        SetRect(bar, new Vector2(320f, -9f), new Vector2(392f, 18f), new Vector2(0f, 1f));
        CreateBarFill(bar, fillColor);
    }

    private static void CreateStatusPanel(Transform parent)
    {
        var statusPanel = CreatePanel("StatusPanel", parent, CardColor);
        SetRect(statusPanel, new Vector2(24f, -462f), new Vector2(760f, 288f), new Vector2(0f, 1f));

        var waterCard = CreatePanel("WaterCard", statusPanel, CardInnerColor);
        SetStretch(waterCard, new Vector2(18f, 18f), new Vector2(18f, 18f));

        var waterTitle = CreateText("WaterTitle", waterCard, "Water Feed", 24, TextAnchor.MiddleLeft, FontStyle.Bold, Color.white);
        SetRect((RectTransform)waterTitle.transform, new Vector2(18f, -18f), new Vector2(220f, 30f), new Vector2(0f, 1f));

        var waterDesc = CreateText("WaterDesc", waterCard, "这里显示当前钓点、目标距离和遛鱼建议。", 16, TextAnchor.MiddleLeft, FontStyle.Normal, MutedTextColor);
        SetRect((RectTransform)waterDesc.transform, new Vector2(18f, -48f), new Vector2(540f, 22f), new Vector2(0f, 1f));

        var lineText = CreateText("LineText", waterCard, "当前水域 / 河岸试钓区 / 抛竿后将自动进入等待咬钩阶段", 18, TextAnchor.UpperLeft, FontStyle.Bold, Color.white);
        SetRect((RectTransform)lineText.transform, new Vector2(18f, -86f), new Vector2(686f, 26f), new Vector2(0f, 1f));

        var hintText = CreateText("HintText", waterCard, "点击“抛竿 / 收竿”开始，所有操作都通过底部控制台完成。", 20, TextAnchor.UpperLeft, FontStyle.Normal, MutedTextColor);
        SetRect((RectTransform)hintText.transform, new Vector2(18f, -122f), new Vector2(692f, 70f), new Vector2(0f, 1f));

        var tipBand = CreatePanel("TipBand", waterCard, new Color(1f, 1f, 1f, 0.05f));
        SetRect(tipBand, new Vector2(18f, -204f), new Vector2(692f, 38f), new Vector2(0f, 1f));
        var tipText = CreateText("TipText", tipBand, "策略提示 / 冲刺时先放线，疲劳后再连续收线。", 16, TextAnchor.MiddleLeft, FontStyle.Bold, new Color(0.88f, 0.94f, 0.96f, 1f));
        SetStretch((RectTransform)tipText.transform, new Vector2(14f, 6f), new Vector2(14f, 6f));

        var notifyPanel = CreatePanel("NotifyPanel", waterCard, new Color(0.02f, 0.05f, 0.08f, 0.84f));
        SetRect(notifyPanel, new Vector2(18f, 18f), new Vector2(692f, 50f), new Vector2(0f, 0f));
        var notifyText = CreateText("NotifyText", notifyPanel, string.Empty, 20, TextAnchor.MiddleCenter, FontStyle.Bold, Color.white);
        SetStretch((RectTransform)notifyText.transform, new Vector2(12f, 6f), new Vector2(12f, 6f));
    }

    private static void CreateInventory(Transform parent)
    {
        var inventoryPanel = CreatePanel("InventoryPanel", parent, CardColor);
        SetVerticalStretchRight(inventoryPanel, 24f, 188f, 276f, 420f);

        var inventoryTitle = CreateText("InventoryTitle", inventoryPanel, "Catch Ledger", 24, TextAnchor.MiddleLeft, FontStyle.Bold, Color.white);
        SetRect((RectTransform)inventoryTitle.transform, new Vector2(20f, -20f), new Vector2(220f, 30f), new Vector2(0f, 1f));

        var inventoryDesc = CreateText("InventoryDesc", inventoryPanel, "已钓获鱼种、重量和估值会累计在这里。", 16, TextAnchor.MiddleLeft, FontStyle.Normal, MutedTextColor);
        SetRect((RectTransform)inventoryDesc.transform, new Vector2(20f, -50f), new Vector2(340f, 22f), new Vector2(0f, 1f));

        var body = CreatePanel("Body", inventoryPanel, CardInnerColor);
        SetStretch(body, new Vector2(16f, 16f), new Vector2(16f, 88f));

        var inventoryText = CreateText("InventoryText", body, "背包为空", 18, TextAnchor.UpperLeft, FontStyle.Normal, Color.white);
        SetStretch((RectTransform)inventoryText.transform, new Vector2(18f, 18f), new Vector2(18f, 18f));
    }

    private static void CreateControls(Transform parent)
    {
        var controlsPanel = CreatePanel("ControlsPanel", parent, CardColor);
        SetRect(controlsPanel, new Vector2(-24f, 24f), new Vector2(420f, 228f), new Vector2(1f, 0f));

        var controlsTitle = CreateText("ControlsTitle", controlsPanel, "Operations Dock", 24, TextAnchor.MiddleLeft, FontStyle.Bold, Color.white);
        SetRect((RectTransform)controlsTitle.transform, new Vector2(20f, -18f), new Vector2(220f, 32f), new Vector2(0f, 1f));

        var controlsDesc = CreateText("ControlsDesc", controlsPanel, "把出售、购买、升级与鱼饵切换集中到右下角，避免和主界面信息区重叠。", 15, TextAnchor.MiddleLeft, FontStyle.Normal, MutedTextColor);
        SetRect((RectTransform)controlsDesc.transform, new Vector2(20f, -52f), new Vector2(380f, 22f), new Vector2(0f, 1f));

        var primaryActions = CreateContainer("PrimaryActions", controlsPanel);
        SetRect(primaryActions, new Vector2(20f, -86f), new Vector2(380f, 44f), new Vector2(0f, 1f));
        CreateButton("CastButton", primaryActions, "抛竿 / 收竿", new Vector2(0f, 0f), new Vector2(182f, 44f), new Color(0.18f, 0.56f, 0.67f));
        CreateButton("SwitchButton", primaryActions, "进入三消", new Vector2(198f, 0f), new Vector2(182f, 44f), new Color(0.76f, 0.46f, 0.14f));

        var lureActions = CreateContainer("LureActions", controlsPanel);
        SetRect(lureActions, new Vector2(-400f, -173f), new Vector2(380f, 42f), new Vector2(0f, 1f));
        CreateButton("PrevLureButton", lureActions, "上一种鱼饵", new Vector2(0f, 0f), new Vector2(182f, 42f), new Color(0.14f, 0.34f, 0.48f));
        CreateButton("NextLureButton", lureActions, "下一种鱼饵", new Vector2(198f, 0f), new Vector2(182f, 42f), new Color(0.14f, 0.34f, 0.48f));

        var fightActions = CreateContainer("FightActions", controlsPanel);
        SetRect(fightActions, new Vector2(20f, 42f), new Vector2(380f, 44f), new Vector2(0f, 0f));
        CreateButton("ReelButton", fightActions, "按住收线", new Vector2(0f, 0f), new Vector2(182f, 44f), new Color(0.15f, 0.49f, 0.24f));
        CreateButton("ReleaseButton", fightActions, "按住放线", new Vector2(198f, 0f), new Vector2(182f, 44f), new Color(0.15f, 0.31f, 0.59f));

        var actionHint = CreateText("ActionHint", controlsPanel, "Idle Operations / 待命阶段的出售、鱼饵和升级操作都固定在这里", 15, TextAnchor.MiddleLeft, FontStyle.Bold, new Color(0.86f, 0.95f, 1f, 1f));
        SetRect((RectTransform)actionHint.transform, new Vector2(20f, 16f), new Vector2(380f, 20f), new Vector2(0f, 0f));
    }

    private static RectTransform CreateContainer(string name, Transform parent)
    {
        var container = new GameObject(name, typeof(RectTransform));
        container.transform.SetParent(parent, false);
        container.layer = 5;
        return container.GetComponent<RectTransform>();
    }

    private static RectTransform CreateImage(string name, Transform parent, Color color)
    {
        var imageObject = new GameObject(name, typeof(RectTransform), typeof(Image));
        imageObject.transform.SetParent(parent, false);
        imageObject.layer = 5;
        imageObject.GetComponent<Image>().color = color;
        return imageObject.GetComponent<RectTransform>();
    }

    private static RectTransform CreatePanel(string name, Transform parent, Color color)
    {
        return CreateImage(name, parent, color);
    }

    private static Text CreateText(string name, Transform parent, string content, int fontSize, TextAnchor alignment, FontStyle fontStyle, Color color)
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
        text.color = color;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;
        return text;
    }

    private static void CreateBarFill(RectTransform background, Color fillColor)
    {
        var fillObject = new GameObject("Fill", typeof(RectTransform), typeof(Image));
        fillObject.transform.SetParent(background, false);
        fillObject.layer = 5;
        var fillRect = fillObject.GetComponent<RectTransform>();
        fillRect.anchorMin = new Vector2(0f, 0f);
        fillRect.anchorMax = new Vector2(0f, 1f);
        fillRect.pivot = new Vector2(0f, 0.5f);
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;
        fillRect.sizeDelta = Vector2.zero;
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
        image.raycastTarget = true;

        var button = buttonObject.GetComponent<Button>();
        button.targetGraphic = image;

        var label = CreateText("Label", buttonObject.transform, text, 18, TextAnchor.MiddleCenter, FontStyle.Bold, Color.white);
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

    private static void SetTopStretch(RectTransform rect, float left, float right, float top, float height)
    {
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.offsetMin = new Vector2(left, -top - height);
        rect.offsetMax = new Vector2(-right, -top);
    }

    private static void SetBottomStretch(RectTransform rect, float left, float right, float bottom, float height)
    {
        rect.anchorMin = new Vector2(0f, 0f);
        rect.anchorMax = new Vector2(1f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);
        rect.offsetMin = new Vector2(left, bottom);
        rect.offsetMax = new Vector2(-right, bottom + height);
    }

    private static void SetVerticalStretchRight(RectTransform rect, float right, float top, float bottom, float width)
    {
        rect.anchorMin = new Vector2(1f, 0f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.offsetMin = new Vector2(-right - width, bottom);
        rect.offsetMax = new Vector2(-right, -top);
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
