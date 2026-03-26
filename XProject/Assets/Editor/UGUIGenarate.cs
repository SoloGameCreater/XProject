using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// School UI prefab generation tool.
/// Usage:
/// 1. Place this file under Assets/Editor or a normal scripts folder.
/// 2. Select a parent node in the Hierarchy.
/// 3. Run Tools/School UI/Create Under Selected And Save Prefab.
/// 4. A UI hierarchy will be created under the selected node and saved as a prefab.
/// </summary>
public class SchoolGameUIPrefabTool
{
    [Header("Optional References")]
    public TMP_FontAsset fontAsset;
    public Sprite whiteSprite;

    [Header("State")]
    public string selectedGirl = "Yui";
    public string currentLocation = "School";
    public string currentTime = "Morning";
    public int money = 1930;
    public int combo = 0;
    public int maxCombo = 0;
    public string actionText = "Idle";

    [Header("Palette")]
    public Color rootPanelColor = Hex("2C2C44");
    public Color darkPanelColor = Hex("211D46");
    public Color darkPanelBorder = Hex("0A0815");
    public Color pinkPanelColor = Hex("7D3755");
    public Color pinkPanelBorder = Hex("2C1322");
    public Color creamPanelColor = Hex("E7C0A8");
    public Color creamPanelBorder = Hex("5B3E32");
    public Color lineBlack = Color.black;
    public Color lightTextColor = Color.white;
    public Color accentPink = Hex("F96AA3");
    public Color accentBlue = Hex("72C5FF");
    public Color accentOrange = Hex("D58C21");
    public Color hintPanelColor = Hex("2A2242");
    public Color mapColor = Hex("161A44");
    public Color selectedOutlineColor = Hex("FFF36D");

    private RectTransform _uiRoot;
    private TMP_Text _locationLabel;
    private TMP_Text _timeTopLabel;
    private TMP_Text _timeBottomLabel;
    private TMP_Text _moneyLabel;
    private TMP_Text _comboLabel;
    private TMP_Text _maxComboLabel;
    private TMP_Text _hintLabel;
    private GirlCardRefs[] _girlCards;

    [Serializable]
    private class GirlData
    {
        public string name;
        public int hp;
        public int mp;
        public Color portraitColor;

        public GirlData(string name, int hp, int mp, Color portraitColor)
        {
            this.name = name;
            this.hp = hp;
            this.mp = mp;
            this.portraitColor = portraitColor;
        }
    }

    private class GirlCardRefs
    {
        public string name;
        public Image rootImage;
        public Outline selectedOutline;
    }

    public void BuildUnder(Transform parent, bool clearOld)
    {
        EnsureResources();

        if (clearOld)
        {
            var old = parent.Find("SchoolGameUIRoot");
            if (old != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying) UnityEngine.Object.DestroyImmediate(old.gameObject);
                else UnityEngine.Object.Destroy(old.gameObject);
#else
                UnityEngine.Object.Destroy(old.gameObject);
#endif
            }
        }

        var rootGo = new GameObject("SchoolGameUIRoot", typeof(RectTransform));
        rootGo.transform.SetParent(parent, false);
        _uiRoot = rootGo.GetComponent<RectTransform>();
        SetFullStretch(_uiRoot);

        CreatePatternBackground();
        CreateTopLeftArea();
        CreateRightStatusPanel();
        CreateLeftCharacterPanel();
        CreateCenterComboArea();
        CreateBottomTimeArea();
        CreateInteractivePlaceholder();
        CreateHintArea();

        RefreshView();
    }

    private void EnsureResources()
    {
        if (whiteSprite == null)
        {
            whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        }
    }

    private void CreatePatternBackground()
    {
        var bg = CreateImage("PatternBG", _uiRoot, new Color(0.13f, 0.11f, 0.21f, 0.22f),
            new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f),
            Vector2.zero, Vector2.zero);
        bg.raycastTarget = false;
    }

    private void CreateTopLeftArea()
    {
        var locationBtn = CreateButton("LocationButton", _uiRoot,
            new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
            new Vector2(220f, 64f), new Vector2(12f, -12f),
            creamPanelColor, creamPanelBorder, 4f,
            HandleLocationClick);
        _locationLabel = CreateText("Label", locationBtn.transform, currentLocation, 34, FontStyles.Bold,
            Color.black, TextAlignmentOptions.Center,
            new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f),
            Vector2.zero, Vector2.zero);
        AddTextOutline(_locationLabel, creamPanelBorder, 0.15f);

        var dateLabel = CreateText("DateLabel", _uiRoot, "Aug 12 (Fri) 18 days left", 22, FontStyles.Bold,
            lightTextColor, TextAlignmentOptions.Left,
            new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
            new Vector2(360f, 32f), new Vector2(8f, -78f));
        AddTextOutline(dateLabel, lineBlack, 0.2f);

        CreateText("StartLabel", _uiRoot, "START", 84, FontStyles.Bold,
            new Color(0.77f, 0.62f, 0.25f, 0.3f), TextAlignmentOptions.Center,
            new Vector2(0.37f, 1f), new Vector2(0.63f, 1f), new Vector2(0.5f, 1f),
            new Vector2(300f, 70f), new Vector2(0f, -46f));

        var timeTopBtn = CreateButton("TimeTopButton", _uiRoot,
            new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
            new Vector2(260f, 54f), new Vector2(-320f, -88f),
            new Color(0, 0, 0, 0), new Color(0, 0, 0, 0), 0f,
            HandleTimeClick);
        _timeTopLabel = CreateText("Label", timeTopBtn.transform, "Time: 0.22", 46, FontStyles.Bold,
            lightTextColor, TextAlignmentOptions.Center,
            new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f),
            Vector2.zero, Vector2.zero);
        AddTextOutline(_timeTopLabel, lineBlack, 0.2f);

        var sideTag = CreatePanel("SideTag", _uiRoot,
            Hex("7181D7"), darkPanelBorder,
            new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
            new Vector2(84f, 210f), new Vector2(-290f, -170f), 4f);
        CreateImage("Inner", sideTag, Hex("D8DEFF"),
            new Vector2(0.24f, 0.07f), new Vector2(0.76f, 0.81f), new Vector2(0.5f, 0.5f),
            Vector2.zero, Vector2.zero, true, lineBlack, 2f).raycastTarget = false;
        CreateText("HLabel", sideTag, "H", 12, FontStyles.Bold, lightTextColor, TextAlignmentOptions.Center,
            new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(0f, 1f),
            new Vector2(22f, 18f), new Vector2(4f, -6f), Hex("F47DAC"), lineBlack, 2f);
        CreateText("FriendLabel", sideTag, "F", 12, FontStyles.Bold, lightTextColor, TextAlignmentOptions.Center,
            new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(0f, 0f),
            new Vector2(22f, 18f), new Vector2(6f, 6f), Hex("5C69DA"), lineBlack, 2f);
    }

    private void CreateRightStatusPanel()
    {
        var panel = CreatePanel("RightStatusPanel", _uiRoot,
            new Color(0.07f, 0.06f, 0.11f, 0.9f), lineBlack,
            new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(1f, 0.5f),
            new Vector2(346f, 0f), new Vector2(0f, 0f), 4f);

        var title = CreateText("GirlsTitle", panel, "- Girls -", 20, FontStyles.Bold,
            lightTextColor, TextAlignmentOptions.Center,
            new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
            new Vector2(-16f, 34f), new Vector2(0f, -10f),
            new Color(0.24f, 0.21f, 0.33f, 1f), lineBlack, 2f);
        AddTextOutline(title, lineBlack, 0.2f);

        var girls = new[]
        {
            new GirlData("Yui", 57, 118, Hex("6F7CF7")),
            new GirlData("Rio", 56, 100, Hex("C45A5F")),
            new GirlData("Miyuki", 60, 164, Hex("7C5FD6")),
        };
        _girlCards = new GirlCardRefs[girls.Length];

        float topStart = -58f;
        for (int i = 0; i < girls.Length; i++)
        {
            CreateGirlCard(panel, girls[i], i, topStart - i * 114f);
        }

        var moneyPanel = CreatePanel("MoneyPanel", panel,
            darkPanelColor, darkPanelBorder,
            new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0f),
            new Vector2(-24f, 180f), new Vector2(0f, 12f), 4f);

        var moneyButton = CreateButton("MoneyButton", moneyPanel,
            new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
            new Vector2(-16f, 38f), new Vector2(0f, -10f),
            new Color(0, 0, 0, 0), new Color(0, 0, 0, 0), 0f,
            HandleMoneyClick);
        _moneyLabel = CreateText("Label", moneyButton.transform, $"Cash: {money} Yen", 28, FontStyles.Bold,
            lightTextColor, TextAlignmentOptions.Left,
            new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f),
            Vector2.zero, Vector2.zero);
        AddTextOutline(_moneyLabel, lineBlack, 0.2f);

        var mapButton = CreateButton("MapButton", moneyPanel,
            new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(0f, 1f),
            new Vector2(112f, 112f), new Vector2(8f, -56f),
            mapColor, lineBlack, 2f,
            HandleMapClick);
        CreateImage("MapSquare1", mapButton.transform, Hex("7FB8F4"),
            new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
            new Vector2(48f, 48f), new Vector2(24f, -20f), true, lineBlack, 2f).raycastTarget = false;
        CreateImage("MapSquare2", mapButton.transform, Hex("B8DFFF"),
            new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
            new Vector2(16f, 16f), new Vector2(64f, -60f), true, lineBlack, 2f).raycastTarget = false;

        CreateColorGrid(moneyPanel);
        CreateBottomIcons(moneyPanel);
    }

    private void CreateGirlCard(RectTransform parent, GirlData girl, int index, float top)
    {
        var button = CreateButton($"GirlCard_{girl.name}", parent,
            new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
            new Vector2(-16f, 104f), new Vector2(0f, top),
            darkPanelColor, darkPanelBorder, 4f,
            () => HandleGirlClick(girl.name));

        var rootImage = button.GetComponent<Image>();
        var outline = button.gameObject.AddComponent<Outline>();
        outline.effectColor = new Color(0f, 0f, 0f, 0f);
        outline.effectDistance = new Vector2(2f, -2f);

        _girlCards[index] = new GirlCardRefs
        {
            name = girl.name,
            rootImage = rootImage,
            selectedOutline = outline,
        };

        CreateImage("Portrait", button.transform, girl.portraitColor,
            new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
            new Vector2(64f, 64f), new Vector2(10f, -10f), true, lineBlack, 2f).raycastTarget = false;

        var nameLabel = CreateText("NameLabel", button.transform, girl.name, 24, FontStyles.Bold,
            lightTextColor, TextAlignmentOptions.Left,
            new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
            new Vector2(120f, 28f), new Vector2(84f, -8f));
        AddTextOutline(nameLabel, lineBlack, 0.2f);

        var hpValue = CreateText("HPValue", button.transform, girl.hp.ToString(), 24, FontStyles.Bold,
            lightTextColor, TextAlignmentOptions.Right,
            new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
            new Vector2(56f, 28f), new Vector2(-10f, -8f));
        AddTextOutline(hpValue, lineBlack, 0.2f);

        CreateStatLine(button.transform, "❤", girl.hp.ToString(), 0.68f, accentPink, new Vector2(84f, -42f), "HP");
        CreateStatLine(button.transform, "∞", girl.mp.ToString(), 0.88f, accentBlue, new Vector2(84f, -66f), "MP");

        if (girl.name == "Miyuki")
        {
            var affinity = CreateText("AffinityMax", button.transform, "Affinity Maxed", 22, FontStyles.Bold,
                Hex("FFF2A8"), TextAlignmentOptions.Left,
                new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0f, 0f),
                new Vector2(190f, 30f), new Vector2(10f, 8f),
                accentOrange, lineBlack, 2f);
            AddTextOutline(affinity, lineBlack, 0.2f);
        }
    }

    private void CreateStatLine(Transform parent, string icon, string value, float fill, Color fillColor, Vector2 pos, string name)
    {
        var iconLabel = CreateText($"{name}Icon", parent, icon, 14, FontStyles.Bold,
            lightTextColor, TextAlignmentOptions.Center,
            new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
            new Vector2(18f, 16f), pos);
        AddTextOutline(iconLabel, lineBlack, 0.2f);

        var shell = CreateImage($"{name}BarShell", parent, Hex("2B2846"),
            new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
            new Vector2(150f, 12f), pos + new Vector2(22f, 2f), true, lineBlack, 2f);
        var fillImage = CreateImage($"{name}BarFill", shell.transform, fillColor,
            new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(0f, 0.5f),
            new Vector2(150f * fill, 0f), Vector2.zero);
        var fillRt = fillImage.rectTransform;
        fillRt.offsetMin = Vector2.zero;
        fillRt.offsetMax = new Vector2(-(150f - 150f * fill), 0f);
        fillImage.raycastTarget = false;

        var valueLabel = CreateText($"{name}Value", parent, value, 14, FontStyles.Bold,
            lightTextColor, TextAlignmentOptions.Right,
            new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
            new Vector2(40f, 16f), pos + new Vector2(180f, 0f));
        AddTextOutline(valueLabel, lineBlack, 0.2f);
    }

    private void CreateColorGrid(Transform parent)
    {
        var gridRoot = new GameObject("ColorGrid", typeof(RectTransform));
        gridRoot.transform.SetParent(parent, false);
        var rt = gridRoot.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.pivot = new Vector2(0f, 1f);
        rt.sizeDelta = new Vector2(-136f, 112f);
        rt.anchoredPosition = new Vector2(132f, -56f);

        CreateImage("Cell1", rt, Hex("815AA6"),
            new Vector2(0f, 1f), new Vector2(0.48f, 1f), new Vector2(0f, 1f),
            new Vector2(0f, 50f), new Vector2(0f, 0f), true, lineBlack, 2f).raycastTarget = false;
        CreateImage("Cell2", rt, Hex("22253F"),
            new Vector2(0.52f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
            new Vector2(0f, 50f), new Vector2(0f, 0f), true, lineBlack, 2f).raycastTarget = false;
        CreateImage("Cell3", rt, Hex("22253F"),
            new Vector2(0f, 1f), new Vector2(0.48f, 1f), new Vector2(0f, 1f),
            new Vector2(0f, 50f), new Vector2(0f, -58f), true, lineBlack, 2f).raycastTarget = false;
        CreateImage("Cell4", rt, Hex("5C9272"),
            new Vector2(0.52f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
            new Vector2(0f, 50f), new Vector2(0f, -58f), true, lineBlack, 2f).raycastTarget = false;
    }

    private void CreateBottomIcons(Transform parent)
    {
        Color[] colors = { Hex("DC8AB3"), Hex("92C9FF"), Hex("8F89EB") };
        for (int i = 0; i < colors.Length; i++)
        {
            CreateImage($"BottomIcon_{i}", parent, colors[i],
                new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f),
                new Vector2(40f, 40f), new Vector2(-12f - i * 44f, 12f), true, lineBlack, 2f).raycastTarget = false;
        }
    }

    private void CreateLeftCharacterPanel()
    {
        var panel = CreatePanel("MainCharacterPanel", _uiRoot,
            pinkPanelColor, pinkPanelBorder,
            new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f),
            new Vector2(282f, 245f), new Vector2(16f, 20f), 4f);

        var placeholder = CreateText("PlaceholderLabel", panel, "Main Character Placeholder\n<size=16>Character background removed, UI container only</size>", 22, FontStyles.Bold,
            lightTextColor, TextAlignmentOptions.Center,
            new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f),
            new Vector2(-16f, -16f), Vector2.zero,
            new Color(0.65f, 0.25f, 0.38f, 0.7f), lineBlack, 2f);
        AddTextOutline(placeholder, lineBlack, 0.2f);
    }

    private void CreateCenterComboArea()
    {
        var comboButton = CreateButton("ComboButton", _uiRoot,
            new Vector2(0.42f, 0f), new Vector2(0.42f, 0f), new Vector2(0f, 0f),
            new Vector2(330f, 120f), new Vector2(0f, 110f),
            new Color(0, 0, 0, 0), new Color(0, 0, 0, 0), 0f,
            HandleTrainClick);

        _comboLabel = CreateText("ComboLabel", comboButton.transform, "Combo:0", 56, FontStyles.Bold,
            lightTextColor, TextAlignmentOptions.Center,
            new Vector2(0f, 0.52f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f),
            Vector2.zero, Vector2.zero);
        _maxComboLabel = CreateText("MaxComboLabel", comboButton.transform, "Max Combo:0", 56, FontStyles.Bold,
            lightTextColor, TextAlignmentOptions.Center,
            new Vector2(0f, 0f), new Vector2(1f, 0.48f), new Vector2(0.5f, 0.5f),
            Vector2.zero, Vector2.zero);
        AddTextOutline(_comboLabel, lineBlack, 0.25f);
        AddTextOutline(_maxComboLabel, lineBlack, 0.25f);
    }

    private void CreateBottomTimeArea()
    {
        var bottomTimeButton = CreateButton("BottomTimeButton", _uiRoot,
            new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f),
            new Vector2(320f, 88f), new Vector2(-280f, 32f),
            new Color(0, 0, 0, 0), new Color(0, 0, 0, 0), 0f,
            HandleTimeClick);
        _timeBottomLabel = CreateText("Label", bottomTimeButton.transform, currentTime, 76, FontStyles.Bold,
            Color.black, TextAlignmentOptions.Center,
            new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f),
            Vector2.zero, Vector2.zero);
        AddTextOutline(_timeBottomLabel, Color.white, 0.35f);
    }

    private void CreateInteractivePlaceholder()
    {
        var placeholder = CreatePanel("InteractionPlaceholder", _uiRoot,
            new Color(1f, 1f, 1f, 0.1f), new Color(0f, 0f, 0f, 0.7f),
            new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
            new Vector2(88f, 110f), new Vector2(-360f, -174f), 2f);

        var label = CreateText("Label", placeholder, "Interaction Placeholder", 14, FontStyles.Bold,
            lightTextColor, TextAlignmentOptions.Center,
            new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f),
            new Vector2(-8f, -8f), Vector2.zero);
        AddTextOutline(label, lineBlack, 0.2f);
    }

    private void CreateHintArea()
    {
        var hint = CreatePanel("HintPanel", _uiRoot,
            hintPanelColor, lineBlack,
            new Vector2(0.37f, 0f), new Vector2(0.37f, 0f), new Vector2(0f, 0f),
            new Vector2(340f, 40f), new Vector2(0f, 16f), 2f);
        _hintLabel = CreateText("HintLabel", hint, $"Hint: {actionText}", 20, FontStyles.Bold,
            lightTextColor, TextAlignmentOptions.Left,
            new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f),
            new Vector2(-12f, -8f), Vector2.zero);
        AddTextOutline(_hintLabel, lineBlack, 0.2f);
    }

    private void HandleGirlClick(string girlName)
    {
        selectedGirl = girlName;
        actionText = $"Viewing {girlName}'s status";
        RefreshView();
    }

    private void HandleLocationClick()
    {
        currentLocation = currentLocation == "School" ? "Shopping District" : "School";
        actionText = $"Scene changed to {currentLocation}";
        RefreshView();
    }

    private void HandleTimeClick()
    {
        currentTime = currentTime == "Morning" ? "Afternoon" : currentTime == "Afternoon" ? "Evening" : "Morning";
        actionText = $"Time advanced to {currentTime}";
        RefreshView();
    }

    private void HandleTrainClick()
    {
        combo += 1;
        maxCombo = Mathf.Max(maxCombo, combo);
        actionText = "Training completed, combo increased";
        RefreshView();
    }

    private void HandleMapClick()
    {
        actionText = "Opened the map panel";
        RefreshView();
    }

    private void HandleMoneyClick()
    {
        money += 100;
        actionText = "Picked up 100 Yen. The value updates here and in the prefab.";
        RefreshView();
    }

    private void RefreshView()
    {
        if (_locationLabel != null) _locationLabel.text = currentLocation;
        if (_timeTopLabel != null) _timeTopLabel.text = "Time: 0.22";
        if (_timeBottomLabel != null) _timeBottomLabel.text = currentTime;
        if (_moneyLabel != null) _moneyLabel.text = $"Cash: {money} Yen";
        if (_comboLabel != null) _comboLabel.text = $"Combo:{combo}";
        if (_maxComboLabel != null) _maxComboLabel.text = $"Max Combo:{maxCombo}";
        if (_hintLabel != null) _hintLabel.text = $"Hint: {actionText}";

        if (_girlCards == null) return;
        foreach (var card in _girlCards)
        {
            bool isSelected = card.name == selectedGirl;
            if (card.rootImage != null)
            {
                card.rootImage.color = isSelected ? Hex("2D2857") : darkPanelColor;
            }
            if (card.selectedOutline != null)
            {
                card.selectedOutline.effectColor = isSelected ? selectedOutlineColor : new Color(0f, 0f, 0f, 0f);
            }
        }
    }

    private RectTransform CreatePanel(string name, Transform parent, Color color, Color borderColor,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 size, Vector2 pos, float border)
    {
        var image = CreateImage(name, parent, color, anchorMin, anchorMax, pivot, size, pos, true, borderColor, border);
        return image.rectTransform;
    }

    private Image CreateImage(string name, Transform parent, Color color,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 size, Vector2 pos,
        bool withBorder = false, Color? borderColor = null, float borderSize = 2f)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = pivot;
        rt.sizeDelta = size;
        rt.anchoredPosition = pos;

        var image = go.GetComponent<Image>();
        image.sprite = whiteSprite;
        image.type = Image.Type.Sliced;
        image.color = color;

        if (withBorder && borderSize > 0f)
        {
            var outline = go.AddComponent<Outline>();
            outline.effectColor = borderColor ?? Color.black;
            outline.effectDistance = new Vector2(borderSize, -borderSize);
        }

        return image;
    }

    private Button CreateButton(string name, Transform parent,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 size, Vector2 pos,
        Color color, Color borderColor, float border,
        UnityAction onClick)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = pivot;
        rt.sizeDelta = size;
        rt.anchoredPosition = pos;

        var image = go.GetComponent<Image>();
        image.sprite = whiteSprite;
        image.type = Image.Type.Sliced;
        image.color = color;

        if (border > 0f)
        {
            var outline = go.AddComponent<Outline>();
            outline.effectColor = borderColor;
            outline.effectDistance = new Vector2(border, -border);
        }

        var button = go.GetComponent<Button>();
        var colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(1f, 1f, 1f, 0.92f);
        colors.pressedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
        colors.selectedColor = Color.white;
        colors.disabledColor = new Color(0.6f, 0.6f, 0.6f, 0.5f);
        button.colors = colors;
        button.targetGraphic = image;
        button.onClick.AddListener(onClick);
        return button;
    }

    private TMP_Text CreateText(string name, Transform parent, string text, float fontSize, FontStyles style,
        Color color, TextAlignmentOptions alignment,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 size, Vector2 pos,
        Color? bgColor = null, Color? bgBorder = null, float bgBorderSize = 0f)
    {
        RectTransform textParent;
        if (bgColor.HasValue)
        {
            textParent = CreatePanel(name + "_BG", parent, bgColor.Value, bgBorder ?? lineBlack,
                anchorMin, anchorMax, pivot, size, pos, bgBorderSize);
            size = Vector2.zero;
            pos = Vector2.zero;
            anchorMin = Vector2.zero;
            anchorMax = Vector2.one;
            pivot = new Vector2(0.5f, 0.5f);
        }
        else
        {
            var holder = new GameObject(name + "_Holder", typeof(RectTransform));
            holder.transform.SetParent(parent, false);
            textParent = holder.GetComponent<RectTransform>();
            textParent.anchorMin = anchorMin;
            textParent.anchorMax = anchorMax;
            textParent.pivot = pivot;
            textParent.sizeDelta = size;
            textParent.anchoredPosition = pos;
        }

        var go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(textParent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        var tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.fontStyle = style;
        tmp.color = color;
        tmp.alignment = alignment;
        tmp.enableWordWrapping = false;
        if (fontAsset != null) tmp.font = fontAsset;
        return tmp;
    }

    private void AddTextOutline(TMP_Text text, Color color, float thickness)
    {
        if (text == null) return;
        text.outlineColor = color;
        text.outlineWidth = thickness;
    }

    public static void SetFullStretch(RectTransform rectTransform)
    {
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    private static Color Hex(string hex)
    {
        if (!hex.StartsWith("#")) hex = "#" + hex;
        return ColorUtility.TryParseHtmlString(hex, out var color) ? color : Color.white;
    }
}

#if UNITY_EDITOR
public static class SchoolGameUIPrefabToolEditor
{
    private const string DefaultFolder = "Assets/GeneratedUI";
    private const string BuiltinUiSpritePath = "UI/Skin/UISprite.psd";

    [MenuItem("Tools/School UI/Create Under Selected And Save Prefab", priority = 1000)]
    public static void CreateUnderSelectedAndSavePrefab()
    {
        var selected = Selection.activeTransform;
        if (selected == null)
        {
            EditorUtility.DisplayDialog("School UI", "Please select a parent node in the Hierarchy first.", "OK");
            return;
        }

        var root = EnsureUiParent(selected);
        Undo.RegisterFullObjectHierarchyUndo(root.gameObject, "Create School UI Prefab");

        var tool = new SchoolGameUIPrefabTool();
        var tmpFont = FindAnyTMPFont();
        ApplyDefaultResources(tool, tmpFont);

        tool.BuildUnder(root, true);
        EditorUtility.SetDirty(root.gameObject);

        EnsureFolder(DefaultFolder);
        var defaultPath = AssetDatabase.GenerateUniqueAssetPath($"{DefaultFolder}/{root.name}.prefab");
        var savePath = EditorUtility.SaveFilePanelInProject(
            "Save School UI Prefab",
            root.name,
            "prefab",
            "Choose a prefab save location",
            defaultPath);

        if (string.IsNullOrEmpty(savePath))
        {
            Selection.activeGameObject = root.gameObject;
            return;
        }

        var prefab = SaveCleanPrefab(root.gameObject, savePath);

        if (prefab != null)
        {
            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);
        }
    }

    [MenuItem("Tools/School UI/Create Under Selected And Save Prefab", true)]
    public static bool ValidateCreateUnderSelectedAndSavePrefab()
    {
        return Selection.activeTransform != null;
    }

    [MenuItem("Tools/School UI/Rebuild On Selected", priority = 1001)]
    public static void RebuildOnSelected()
    {
        var selected = Selection.activeTransform;
        if (selected == null)
        {
            EditorUtility.DisplayDialog("School UI", "Please select a node first.", "OK");
            return;
        }

        var root = FindUiRoot(selected);
        if (root == null)
        {
            EditorUtility.DisplayDialog("School UI", "No SchoolGameUI_PrefabRoot was found on the selected object or its parent/child nodes.", "OK");
            return;
        }

        Undo.RegisterFullObjectHierarchyUndo(root.gameObject, "Rebuild School UI");

        var tool = new SchoolGameUIPrefabTool();
        ApplyDefaultResources(tool, FindAnyTMPFont());
        tool.BuildUnder(root, true);
        EditorUtility.SetDirty(root.gameObject);
    }

    private static Transform EnsureUiParent(Transform selected)
    {
        if (selected.name == "SchoolGameUI_PrefabRoot")
        {
            return selected;
        }

        var existing = selected.Find("SchoolGameUI_PrefabRoot");
        if (existing != null) return existing;

        var rootGo = new GameObject("SchoolGameUI_PrefabRoot", typeof(RectTransform));
        Undo.RegisterCreatedObjectUndo(rootGo, "Create School UI Root");
        rootGo.transform.SetParent(selected, false);

        var rt = rootGo.GetComponent<RectTransform>();
        SchoolGameUIPrefabTool.SetFullStretch(rt);

        return rootGo.transform;
    }

    private static TMP_FontAsset FindAnyTMPFont()
    {
        if (TMP_Settings.defaultFontAsset != null)
        {
            return TMP_Settings.defaultFontAsset;
        }

        var guids = AssetDatabase.FindAssets("t:TMP_FontAsset");
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);
            if (font != null) return font;
        }
        return null;
    }

    private static Transform FindUiRoot(Transform selected)
    {
        var current = selected;
        while (current != null)
        {
            if (current.name == "SchoolGameUI_PrefabRoot")
            {
                return current;
            }

            current = current.parent;
        }

        return selected.Find("SchoolGameUI_PrefabRoot");
    }

    private static void ApplyDefaultResources(SchoolGameUIPrefabTool tool, TMP_FontAsset fallbackFont)
    {
        if (tool.fontAsset == null && fallbackFont != null)
        {
            tool.fontAsset = fallbackFont;
        }

        if (tool.whiteSprite == null || !AssetDatabase.Contains(tool.whiteSprite))
        {
            var builtInSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(BuiltinUiSpritePath);
            if (builtInSprite != null)
            {
                tool.whiteSprite = builtInSprite;
            }
        }
    }

    private static GameObject SaveCleanPrefab(GameObject sourceRoot, string savePath)
    {
        var prefabSource = UnityEngine.Object.Instantiate(sourceRoot);
        prefabSource.name = sourceRoot.name;
        prefabSource.transform.SetParent(null, false);

        try
        {
            var prefab = PrefabUtility.SaveAsPrefabAsset(prefabSource, savePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return prefab;
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(prefabSource);
        }
    }

    private static void EnsureFolder(string folderPath)
    {
        if (AssetDatabase.IsValidFolder(folderPath)) return;

        var parts = folderPath.Split('/');
        var current = parts[0];
        for (int i = 1; i < parts.Length; i++)
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
#endif
