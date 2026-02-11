using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 生成纯 UGUI 结构的背包预制体（不挂行为脚本，不绑定事件）。
/// </summary>
public static class UGUIInventoryPrefabGenerator
{
    private const string OutputRootFolder = "Assets/ExtraRes/Prefabs/UI";
    private const string UiPrefix = "Inventory";
    private static readonly Vector2 DefaultViewSize = new Vector2(1920f, 1080f);
    private static readonly string OutputFolder = OutputRootFolder + "/" + UiPrefix;
    private static readonly string ViewPrefabPath = OutputFolder + "/" + UiPrefix + "View.prefab";
    private static readonly string CellPrefabPath = OutputFolder + "/" + UiPrefix + "ItemCell.prefab";

    [MenuItem("Tools/UI/生成背包UI预制体(纯结构)")]
    public static void GenerateFromMenu()
    {
        GenerateInternal();
        EditorUtility.DisplayDialog("生成完成", $"已生成:\n{ViewPrefabPath}\n{CellPrefabPath}", "确定");
    }

    /// <summary>
    /// 批处理入口：Unity.exe -batchmode -quit -projectPath . -executeMethod UGUIInventoryPrefabGenerator.GenerateFromBatch
    /// </summary>
    public static void GenerateFromBatch()
    {
        GenerateInternal();
    }

    private static void GenerateInternal()
    {
        EnsureAssetFolder(OutputFolder);

        GameObject itemCellRoot = CreateInventoryItemCell("InventoryItemCell");
        PrefabUtility.SaveAsPrefabAsset(itemCellRoot, CellPrefabPath);
        Object.DestroyImmediate(itemCellRoot);

        GameObject viewRoot = CreateInventoryView();
        PrefabUtility.SaveAsPrefabAsset(viewRoot, ViewPrefabPath);
        Object.DestroyImmediate(viewRoot);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[UGUIInventoryPrefabGenerator] 生成完成: {ViewPrefabPath}, {CellPrefabPath}");
    }

    private static GameObject CreateInventoryView()
    {
        GameObject root = CreateRectObject("InventoryView", null);
        SetCenter(root.GetComponent<RectTransform>(), DefaultViewSize);

        GameObject rootNode = CreateRectObject("Root", root.transform);
        SetStretch(rootNode.GetComponent<RectTransform>());

        GameObject header = CreateRectObject("Header", rootNode.transform);
        RectTransform headerRt = header.GetComponent<RectTransform>();
        headerRt.anchorMin = new Vector2(0f, 1f);
        headerRt.anchorMax = new Vector2(1f, 1f);
        headerRt.pivot = new Vector2(0.5f, 1f);
        headerRt.anchoredPosition = Vector2.zero;
        headerRt.sizeDelta = new Vector2(0f, 120f);
        header.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.35f);

        GameObject title = CreateRectObject("Txt_Title", header.transform);
        RectTransform titleRt = title.GetComponent<RectTransform>();
        titleRt.anchorMin = new Vector2(0f, 0f);
        titleRt.anchorMax = new Vector2(1f, 1f);
        titleRt.offsetMin = new Vector2(24f, 16f);
        titleRt.offsetMax = new Vector2(-180f, -16f);
        CreateText(title, "背包");

        GameObject closeBtn = CreateButton("Btn_Close", header.transform, "关闭");
        RectTransform closeBtnRt = closeBtn.GetComponent<RectTransform>();
        closeBtnRt.anchorMin = new Vector2(1f, 0.5f);
        closeBtnRt.anchorMax = new Vector2(1f, 0.5f);
        closeBtnRt.pivot = new Vector2(1f, 0.5f);
        closeBtnRt.anchoredPosition = new Vector2(-24f, 0f);
        closeBtnRt.sizeDelta = new Vector2(120f, 64f);

        GameObject footer = CreateRectObject("Footer", rootNode.transform);
        RectTransform footerRt = footer.GetComponent<RectTransform>();
        footerRt.anchorMin = new Vector2(0f, 0f);
        footerRt.anchorMax = new Vector2(1f, 0f);
        footerRt.pivot = new Vector2(0.5f, 0f);
        footerRt.anchoredPosition = Vector2.zero;
        footerRt.sizeDelta = new Vector2(0f, 100f);
        footer.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.3f);
        HorizontalLayoutGroup footerLayout = footer.AddComponent<HorizontalLayoutGroup>();
        footerLayout.padding = new RectOffset(24, 24, 16, 16);
        footerLayout.spacing = 12f;
        footerLayout.childAlignment = TextAnchor.MiddleRight;
        footerLayout.childControlWidth = false;
        footerLayout.childControlHeight = true;
        footerLayout.childForceExpandWidth = false;
        footerLayout.childForceExpandHeight = false;

        GameObject btnUse = CreateButton("Btn_Use", footer.transform, "使用");
        btnUse.GetComponent<RectTransform>().sizeDelta = new Vector2(160f, 68f);
        GameObject btnDrop = CreateButton("Btn_Drop", footer.transform, "丢弃");
        btnDrop.GetComponent<RectTransform>().sizeDelta = new Vector2(160f, 68f);

        GameObject body = CreateRectObject("Body", rootNode.transform);
        RectTransform bodyRt = body.GetComponent<RectTransform>();
        bodyRt.anchorMin = new Vector2(0f, 0f);
        bodyRt.anchorMax = new Vector2(1f, 1f);
        bodyRt.offsetMin = new Vector2(0f, 100f);
        bodyRt.offsetMax = new Vector2(0f, -120f);

        HorizontalLayoutGroup bodyLayout = body.AddComponent<HorizontalLayoutGroup>();
        bodyLayout.padding = new RectOffset(16, 16, 16, 16);
        bodyLayout.spacing = 12f;
        bodyLayout.childAlignment = TextAnchor.UpperLeft;
        bodyLayout.childControlWidth = false;
        bodyLayout.childControlHeight = true;
        bodyLayout.childForceExpandWidth = false;
        bodyLayout.childForceExpandHeight = true;

        GameObject leftTabs = CreateRectObject("LeftTabs", body.transform);
        leftTabs.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.2f);
        LayoutElement leftTabsLayout = leftTabs.AddComponent<LayoutElement>();
        leftTabsLayout.preferredWidth = 220f;
        leftTabsLayout.minWidth = 220f;

        VerticalLayoutGroup leftTabsGroup = leftTabs.AddComponent<VerticalLayoutGroup>();
        leftTabsGroup.padding = new RectOffset(12, 12, 12, 12);
        leftTabsGroup.spacing = 8f;
        leftTabsGroup.childAlignment = TextAnchor.UpperCenter;
        leftTabsGroup.childControlWidth = true;
        leftTabsGroup.childControlHeight = false;
        leftTabsGroup.childForceExpandWidth = true;
        leftTabsGroup.childForceExpandHeight = false;

        CreateToggle("Tgl_All", leftTabs.transform, "全部");
        CreateToggle("Tgl_Equip", leftTabs.transform, "装备");
        CreateToggle("Tgl_Consumable", leftTabs.transform, "消耗品");
        CreateToggle("Tgl_Material", leftTabs.transform, "材料");

        GameObject gridScroll = CreateRectObject("GridScroll", body.transform);
        gridScroll.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.15f);
        LayoutElement gridLayout = gridScroll.AddComponent<LayoutElement>();
        gridLayout.flexibleWidth = 1f;
        gridLayout.minWidth = 400f;

        ScrollRect scrollRect = gridScroll.AddComponent<ScrollRect>();
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;
        scrollRect.inertia = true;

        GameObject viewport = CreateRectObject("Viewport", gridScroll.transform);
        SetStretch(viewport.GetComponent<RectTransform>());
        viewport.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.02f);
        viewport.AddComponent<RectMask2D>();

        GameObject content = CreateRectObject("Content", viewport.transform);
        RectTransform contentRt = content.GetComponent<RectTransform>();
        contentRt.anchorMin = new Vector2(0f, 1f);
        contentRt.anchorMax = new Vector2(0f, 1f);
        contentRt.pivot = new Vector2(0f, 1f);
        contentRt.anchoredPosition = new Vector2(12f, -12f);
        contentRt.sizeDelta = new Vector2(880f, 664f);

        GridLayoutGroup gridGroup = content.AddComponent<GridLayoutGroup>();
        gridGroup.cellSize = new Vector2(100f, 100f);
        gridGroup.spacing = new Vector2(8f, 8f);
        gridGroup.padding = new RectOffset(12, 12, 12, 12);
        gridGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
        gridGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
        gridGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridGroup.constraintCount = 8;

        scrollRect.viewport = viewport.GetComponent<RectTransform>();
        scrollRect.content = contentRt;

        GameObject itemPrototype = CreateInventoryItemCell("ItemCell_Prototype");
        itemPrototype.transform.SetParent(content.transform, false);

        GameObject detailPanel = CreateRectObject("DetailPanel", body.transform);
        detailPanel.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.2f);
        LayoutElement detailLayout = detailPanel.AddComponent<LayoutElement>();
        detailLayout.preferredWidth = 340f;
        detailLayout.minWidth = 320f;

        VerticalLayoutGroup detailGroup = detailPanel.AddComponent<VerticalLayoutGroup>();
        detailGroup.padding = new RectOffset(12, 12, 12, 12);
        detailGroup.spacing = 8f;
        detailGroup.childAlignment = TextAnchor.UpperLeft;
        detailGroup.childControlWidth = true;
        detailGroup.childControlHeight = false;
        detailGroup.childForceExpandWidth = true;
        detailGroup.childForceExpandHeight = false;

        GameObject imgIcon = CreateRectObject("Img_Icon", detailPanel.transform);
        imgIcon.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.85f);
        imgIcon.AddComponent<LayoutElement>().preferredHeight = 220f;

        GameObject txtName = CreateRectObject("Txt_Name", detailPanel.transform);
        CreateText(txtName, "道具名称");
        txtName.AddComponent<LayoutElement>().preferredHeight = 36f;

        GameObject txtDesc = CreateRectObject("Txt_Desc", detailPanel.transform);
        CreateText(txtDesc, "道具描述");
        txtDesc.AddComponent<LayoutElement>().preferredHeight = 120f;

        GameObject groupStats = CreateRectObject("Group_Stats", detailPanel.transform);
        groupStats.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.06f);
        groupStats.AddComponent<LayoutElement>().preferredHeight = 200f;

        return root;
    }

    private static GameObject CreateInventoryItemCell(string name)
    {
        GameObject root = CreateRectObject(name, null);
        RectTransform rootRt = root.GetComponent<RectTransform>();
        rootRt.sizeDelta = new Vector2(100f, 100f);
        root.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.1f);

        GameObject imgBg = CreateRectObject("Img_BG", root.transform);
        SetStretch(imgBg.GetComponent<RectTransform>());
        imgBg.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.1f);

        GameObject iconItem = CreateRectObject("Icon_Item", root.transform);
        RectTransform iconRt = iconItem.GetComponent<RectTransform>();
        iconRt.anchorMin = new Vector2(0f, 0f);
        iconRt.anchorMax = new Vector2(1f, 1f);
        iconRt.offsetMin = new Vector2(8f, 8f);
        iconRt.offsetMax = new Vector2(-8f, -24f);
        iconItem.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.85f);

        GameObject txtCount = CreateRectObject("Txt_Count", root.transform);
        RectTransform txtCountRt = txtCount.GetComponent<RectTransform>();
        txtCountRt.anchorMin = new Vector2(1f, 0f);
        txtCountRt.anchorMax = new Vector2(1f, 0f);
        txtCountRt.pivot = new Vector2(1f, 0f);
        txtCountRt.anchoredPosition = new Vector2(-6f, 4f);
        txtCountRt.sizeDelta = new Vector2(64f, 24f);
        CreateText(txtCount, "99", TextAnchor.LowerRight);

        GameObject imgSelected = CreateRectObject("Img_Selected", root.transform);
        SetStretch(imgSelected.GetComponent<RectTransform>());
        imgSelected.AddComponent<Image>().color = new Color(1f, 0.85f, 0.2f, 0.25f);

        GameObject imgRarity = CreateRectObject("Img_RarityFrame", root.transform);
        SetStretch(imgRarity.GetComponent<RectTransform>());
        Image rarityImage = imgRarity.AddComponent<Image>();
        rarityImage.color = new Color(1f, 0.7f, 0.1f, 0.65f);
        rarityImage.type = Image.Type.Sliced;

        return root;
    }

    private static GameObject CreateRectObject(string name, Transform parent)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        if (parent != null)
        {
            go.transform.SetParent(parent, false);
        }

        return go;
    }

    private static GameObject CreateButton(string name, Transform parent, string label)
    {
        GameObject button = CreateRectObject(name, parent);
        button.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.25f);
        button.AddComponent<Button>();

        GameObject textGo = CreateRectObject("Txt_Label", button.transform);
        SetStretch(textGo.GetComponent<RectTransform>());
        CreateText(textGo, label, TextAnchor.MiddleCenter);
        return button;
    }

    private static GameObject CreateToggle(string name, Transform parent, string label)
    {
        GameObject toggle = CreateRectObject(name, parent);
        toggle.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.2f);
        toggle.AddComponent<LayoutElement>().preferredHeight = 52f;
        Toggle toggleComponent = toggle.AddComponent<Toggle>();
        toggleComponent.isOn = false;

        GameObject textGo = CreateRectObject("Txt_Label", toggle.transform);
        SetStretch(textGo.GetComponent<RectTransform>());
        CreateText(textGo, label, TextAnchor.MiddleCenter);
        return toggle;
    }

    private static void CreateText(GameObject target, string text, TextAnchor alignment = TextAnchor.MiddleLeft)
    {
        Text textComponent = target.AddComponent<Text>();
        textComponent.text = text;
        textComponent.alignment = alignment;
        textComponent.color = Color.white;
        textComponent.fontSize = 28;
        // Unity 6000 开始 Arial 内置字体不可用，优先使用 LegacyRuntime；旧版本回退 Arial。
        Font font = null;
        try
        {
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        }
        catch
        {
            // ignored
        }

        if (font == null)
        {
            font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        textComponent.font = font;
        textComponent.horizontalOverflow = HorizontalWrapMode.Overflow;
        textComponent.verticalOverflow = VerticalWrapMode.Truncate;
    }

    private static void SetStretch(RectTransform rectTransform)
    {
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    private static void SetCenter(RectTransform rectTransform, Vector2 size)
    {
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = size;
    }

    private static void EnsureAssetFolder(string folderPath)
    {
        string[] parts = folderPath.Split('/');
        if (parts.Length == 0 || parts[0] != "Assets")
        {
            Debug.LogError($"[UGUIInventoryPrefabGenerator] 非法路径: {folderPath}");
            return;
        }

        string current = "Assets";
        for (int i = 1; i < parts.Length; i++)
        {
            string next = $"{current}/{parts[i]}";
            if (!AssetDatabase.IsValidFolder(next))
            {
                AssetDatabase.CreateFolder(current, parts[i]);
            }

            current = next;
        }
    }
}
