using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class UiSchemaHierarchyImporter
{
    private const string MenuPath = "GameObject/Generate UI From Schema File...";
    private const string LastSchemaDirectoryKey = "UiSchemaHierarchyImporter.LastSchemaDirectory";
    private static readonly Regex NumberTokenRegex = new Regex(@"x\d+|\d+(?:[/:]\d+)*", RegexOptions.Compiled);

    [MenuItem(MenuPath, false, 2101)]
    private static void ImportFromSchema(MenuCommand command)
    {
        GameObject host = command.context as GameObject;

        if (host == null)
        {
            host = Selection.activeGameObject;
        }

        if (host == null)
        {
            EditorUtility.DisplayDialog(
                "UI Schema Import",
                "Select a Hierarchy node first, then use the context menu on that node.",
                "OK");
            return;
        }

        string filePath = EditorUtility.OpenFilePanel("Select UI Schema File", GetInitialDirectory(), string.Empty);
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        if (!File.Exists(filePath))
        {
            EditorUtility.DisplayDialog("UI Schema Import", "The selected file does not exist.", "OK");
            return;
        }

        SaveLastDirectory(filePath);

        string jsonText = File.ReadAllText(filePath, Encoding.UTF8);
        UiSchemaRoot schema = null;

        try
        {
            schema = JsonUtility.FromJson<UiSchemaRoot>(jsonText);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }

        if (schema == null || schema.screen == null || schema.screen.referenceSize == null)
        {
            EditorUtility.DisplayDialog(
                "UI Schema Import",
                "The selected file is not a valid UI schema JSON file.",
                "OK");
            return;
        }

        Transform generationParent = PrepareGenerationParent(host, schema.screen.referenceSize);
        if (generationParent == null)
        {
            EditorUtility.DisplayDialog(
                "UI Schema Import",
                "Could not prepare a valid parent for the generated UI.",
                "OK");
            return;
        }

        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Generate UI From Schema");

        Rect rootBounds = new Rect(0f, 0f, schema.screen.referenceSize.width, schema.screen.referenceSize.height);
        GameObject rootObject = CreateRootScreen(schema.screen, generationParent, rootBounds);

        Undo.CollapseUndoOperations(undoGroup);

        Selection.activeGameObject = rootObject;
        EditorGUIUtility.PingObject(rootObject);
    }

    [MenuItem(MenuPath, true)]
    private static bool ValidateImportFromSchema()
    {
        return true;
    }

    private static Transform PrepareGenerationParent(GameObject host, UiSchemaReferenceSize referenceSize)
    {
        RectTransform hostRect = host.GetComponent<RectTransform>();
        if (hostRect != null)
        {
            return hostRect;
        }

        Canvas parentCanvas = host.GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            return host.transform;
        }

        GameObject canvasObject = new GameObject("GeneratedUiCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Undo.RegisterCreatedObjectUndo(canvasObject, "Create UI Canvas");
        GameObjectUtility.SetParentAndAlign(canvasObject, host);

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(referenceSize.width, referenceSize.height);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();
        canvasRect.anchorMin = Vector2.zero;
        canvasRect.anchorMax = Vector2.one;
        canvasRect.offsetMin = Vector2.zero;
        canvasRect.offsetMax = Vector2.zero;

        return canvasRect;
    }

    private static GameObject CreateRootScreen(UiSchemaScreen screen, Transform parent, Rect rootBounds)
    {
        GameObject screenObject = new GameObject(screen.name ?? "UIScreen", typeof(RectTransform));
        Undo.RegisterCreatedObjectUndo(screenObject, "Create UI Screen");
        GameObjectUtility.SetParentAndAlign(screenObject, parent.gameObject);

        RectTransform screenRect = screenObject.GetComponent<RectTransform>();
        screenRect.anchorMin = Vector2.zero;
        screenRect.anchorMax = Vector2.one;
        screenRect.pivot = new Vector2(0.5f, 0.5f);
        screenRect.offsetMin = Vector2.zero;
        screenRect.offsetMax = Vector2.zero;

        if (screen.children != null)
        {
            foreach (UiSchemaNode child in screen.children)
            {
                CreateNodeRecursive(child, screenRect, rootBounds, rootBounds);
            }
        }

        return screenObject;
    }

    private static void CreateNodeRecursive(UiSchemaNode node, Transform parent, Rect parentBounds, Rect inheritedBounds)
    {
        if (node == null)
        {
            return;
        }

        Rect nodeBounds = GetEffectiveBounds(node, inheritedBounds);
        GameObject nodeObject = new GameObject(node.name ?? node.type ?? "Node", typeof(RectTransform));
        Undo.RegisterCreatedObjectUndo(nodeObject, "Create UI Node");
        GameObjectUtility.SetParentAndAlign(nodeObject, parent.gameObject);

        RectTransform rectTransform = nodeObject.GetComponent<RectTransform>();
        ApplyRectTransform(rectTransform, nodeBounds, parentBounds);
        ApplyVisualByType(nodeObject, node, nodeBounds);

        if (node.children != null)
        {
            foreach (UiSchemaNode child in node.children)
            {
                CreateNodeRecursive(child, rectTransform, nodeBounds, nodeBounds);
            }
        }
    }

    private static Rect GetEffectiveBounds(UiSchemaNode node, Rect fallbackBounds)
    {
        if (node != null && node.boundsPx != null)
        {
            return new Rect(node.boundsPx.x, node.boundsPx.y, Mathf.Max(1f, node.boundsPx.width), Mathf.Max(1f, node.boundsPx.height));
        }

        return fallbackBounds;
    }

    private static void ApplyRectTransform(RectTransform rectTransform, Rect nodeBounds, Rect parentBounds)
    {
        float localX = nodeBounds.x - parentBounds.x;
        float localY = nodeBounds.y - parentBounds.y;

        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(0f, 1f);
        rectTransform.pivot = new Vector2(0f, 1f);
        rectTransform.anchoredPosition = new Vector2(localX, -localY);
        rectTransform.sizeDelta = new Vector2(nodeBounds.width, nodeBounds.height);
    }

    private static void ApplyVisualByType(GameObject target, UiSchemaNode node, Rect nodeBounds)
    {
        string nodeType = string.IsNullOrEmpty(node.type) ? "Panel" : node.type;

        switch (nodeType)
        {
            case "Button":
                AddImage(target, new Color(0.70f, 0.56f, 0.42f, 1f));
                target.AddComponent<Button>();
                AddTextChild(target.transform, "Label", GetDisplayText(node), TextAnchor.MiddleCenter, 18, Color.black);
                break;

            case "Label":
                AddTextComponent(target, GetDisplayText(node), TextAnchor.MiddleLeft, 18, Color.white);
                break;

            case "ProgressBar":
                BuildProgressBar(target, node, nodeBounds);
                break;

            case "Portrait":
                AddImage(target, new Color(0.48f, 0.42f, 0.56f, 0.92f));
                AddTextChild(target.transform, "Placeholder", GetPlaceholderText(node), TextAnchor.MiddleCenter, 12, Color.white);
                break;

            case "Icon":
                AddImage(target, new Color(0.36f, 0.52f, 0.72f, 0.92f));
                AddTextChild(target.transform, "Placeholder", GetPlaceholderText(node), TextAnchor.MiddleCenter, 10, Color.white);
                break;

            case "DialogBox":
                AddImage(target, new Color(0.12f, 0.12f, 0.12f, 0.72f));
                break;

            case "Panel":
                AddImage(target, GetPanelColor(node.name));
                break;

            case "Screen":
            case "List":
            default:
                break;
        }
    }

    private static void BuildProgressBar(GameObject target, UiSchemaNode node, Rect nodeBounds)
    {
        AddImage(target, new Color(0.10f, 0.10f, 0.14f, 0.88f));

        GameObject fillObject = new GameObject("Fill", typeof(RectTransform), typeof(Image));
        Undo.RegisterCreatedObjectUndo(fillObject, "Create Progress Bar Fill");
        GameObjectUtility.SetParentAndAlign(fillObject, target);

        RectTransform fillRect = fillObject.GetComponent<RectTransform>();
        Image fillImage = fillObject.GetComponent<Image>();
        fillImage.color = GetProgressFillColor(node.name);

        float fillAmount = GetFillAmount(node.text);
        bool isVertical = nodeBounds.height > nodeBounds.width;

        if (isVertical)
        {
            fillRect.anchorMin = new Vector2(0f, 0f);
            fillRect.anchorMax = new Vector2(1f, 0f);
            fillRect.pivot = new Vector2(0.5f, 0f);
            fillRect.anchoredPosition = Vector2.zero;
            fillRect.sizeDelta = new Vector2(0f, nodeBounds.height * fillAmount);
        }
        else
        {
            fillRect.anchorMin = new Vector2(0f, 0f);
            fillRect.anchorMax = new Vector2(0f, 1f);
            fillRect.pivot = new Vector2(0f, 0.5f);
            fillRect.anchoredPosition = Vector2.zero;
            fillRect.sizeDelta = new Vector2(nodeBounds.width * fillAmount, 0f);
        }

        string text = GetDisplayText(node);
        if (!string.IsNullOrEmpty(text))
        {
            AddTextChild(target.transform, "Value", text, TextAnchor.MiddleCenter, 12, Color.white);
        }
    }

    private static Image AddImage(GameObject target, Color color)
    {
        Image image = target.GetComponent<Image>();
        if (image == null)
        {
            image = target.AddComponent<Image>();
        }

        image.color = color;
        return image;
    }

    private static void AddTextComponent(GameObject target, string content, TextAnchor alignment, int fontSize, Color color)
    {
        Text text = target.GetComponent<Text>();
        if (text == null)
        {
            text = target.AddComponent<Text>();
        }

        text.text = content;
        text.alignment = alignment;
        text.fontSize = fontSize;
        text.color = color;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.font = GetBuiltinFont();
    }

    private static void AddTextChild(Transform parent, string name, string content, TextAnchor alignment, int fontSize, Color color)
    {
        if (string.IsNullOrEmpty(content))
        {
            return;
        }

        GameObject textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
        Undo.RegisterCreatedObjectUndo(textObject, "Create Text Child");
        GameObjectUtility.SetParentAndAlign(textObject, parent.gameObject);

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        AddTextComponent(textObject, content, alignment, fontSize, color);
    }

    private static string GetDisplayText(UiSchemaNode node)
    {
        string source = node != null ? node.text : null;
        string fallback = HumanizeName(node != null ? node.name : "Label");

        if (string.IsNullOrEmpty(source))
        {
            return fallback;
        }

        if (IsAscii(source))
        {
            return source.Trim();
        }

        MatchCollection matches = NumberTokenRegex.Matches(source);
        if (matches.Count == 0)
        {
            return fallback;
        }

        List<string> tokens = new List<string>();
        foreach (Match match in matches)
        {
            if (!string.IsNullOrEmpty(match.Value))
            {
                tokens.Add(match.Value);
            }
        }

        if (tokens.Count == 0)
        {
            return fallback;
        }

        return (fallback + " " + string.Join(" ", tokens)).Trim();
    }

    private static string GetPlaceholderText(UiSchemaNode node)
    {
        if (!string.IsNullOrEmpty(node.imagePlaceholder))
        {
            return HumanizeName(node.imagePlaceholder);
        }

        return HumanizeName(node.name);
    }

    private static bool IsAscii(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return true;
        }

        for (int index = 0; index < value.Length; index++)
        {
            char character = value[index];
            if (character > 127)
            {
                return false;
            }
        }

        return true;
    }

    private static float GetFillAmount(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            Match match = Regex.Match(text, @"(\d+(?:\.\d+)?)\s*/\s*(\d+(?:\.\d+)?)");
            if (match.Success)
            {
                float currentValue;
                float maxValue;

                if (float.TryParse(match.Groups[1].Value, out currentValue) &&
                    float.TryParse(match.Groups[2].Value, out maxValue) &&
                    maxValue > 0f)
                {
                    return Mathf.Clamp01(currentValue / maxValue);
                }
            }
        }

        return 0.65f;
    }

    private static Color GetPanelColor(string nodeName)
    {
        if (string.IsNullOrEmpty(nodeName))
        {
            return new Color(0.16f, 0.18f, 0.22f, 0.35f);
        }

        if (nodeName.IndexOf("Scene", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            return new Color(0.14f, 0.18f, 0.22f, 0.15f);
        }

        if (nodeName.IndexOf("Map", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            return new Color(0.18f, 0.18f, 0.24f, 0.42f);
        }

        return new Color(0.18f, 0.18f, 0.22f, 0.45f);
    }

    private static Color GetProgressFillColor(string nodeName)
    {
        if (string.IsNullOrEmpty(nodeName))
        {
            return new Color(0.90f, 0.68f, 0.34f, 1f);
        }

        if (nodeName.IndexOf("Mood", StringComparison.OrdinalIgnoreCase) >= 0 ||
            nodeName.IndexOf("Action", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            return new Color(0.78f, 0.44f, 0.76f, 1f);
        }

        if (nodeName.IndexOf("Gauge", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            return new Color(0.54f, 0.72f, 0.96f, 1f);
        }

        return new Color(0.92f, 0.70f, 0.38f, 1f);
    }

    private static string HumanizeName(string raw)
    {
        if (string.IsNullOrEmpty(raw))
        {
            return "Node";
        }

        string cleaned = raw.Replace("_", " ").Replace("-", " ");
        cleaned = Regex.Replace(cleaned, "([a-z0-9])([A-Z])", "$1 $2");
        cleaned = Regex.Replace(cleaned, @"\s+", " ").Trim();

        cleaned = RemoveSuffix(cleaned, " Label");
        cleaned = RemoveSuffix(cleaned, " Button");
        cleaned = RemoveSuffix(cleaned, " Panel");
        cleaned = RemoveSuffix(cleaned, " Dialog");
        cleaned = RemoveSuffix(cleaned, " Dialog Box");
        cleaned = RemoveSuffix(cleaned, " Portrait");
        cleaned = RemoveSuffix(cleaned, " Icon");
        cleaned = RemoveSuffix(cleaned, " Bar");
        cleaned = RemoveSuffix(cleaned, " List");
        cleaned = RemoveSuffix(cleaned, " Screen");

        cleaned = cleaned.Replace(" Hp ", " HP ");
        cleaned = cleaned.Replace(" Hp", " HP");
        cleaned = cleaned.Replace("Ui", "UI");

        return cleaned.Trim();
    }

    private static string RemoveSuffix(string source, string suffix)
    {
        if (source.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
        {
            return source.Substring(0, source.Length - suffix.Length);
        }

        return source;
    }

    private static Font GetBuiltinFont()
    {
        Font font = TryGetBuiltinFont("LegacyRuntime.ttf");
        if (font != null)
        {
            return font;
        }

        font = TryGetBuiltinFont("Arial.ttf");
        if (font != null)
        {
            return font;
        }

        throw new InvalidOperationException("Could not load a built-in Unity font. Tried LegacyRuntime.ttf and Arial.ttf.");
    }

    private static Font TryGetBuiltinFont(string fontName)
    {
        try
        {
            return Resources.GetBuiltinResource<Font>(fontName);
        }
        catch (ArgumentException)
        {
            return null;
        }
    }

    private static string GetInitialDirectory()
    {
        if (EditorPrefs.HasKey(LastSchemaDirectoryKey))
        {
            string savedDirectory = EditorPrefs.GetString(LastSchemaDirectoryKey);
            if (!string.IsNullOrEmpty(savedDirectory) && Directory.Exists(savedDirectory))
            {
                return savedDirectory;
            }
        }

        return Application.dataPath;
    }

    private static void SaveLastDirectory(string filePath)
    {
        string directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            EditorPrefs.SetString(LastSchemaDirectoryKey, directory);
        }
    }

    [Serializable]
    private class UiSchemaRoot
    {
        public string schemaVersion;
        public UiSchemaScreen screen;
    }

    [Serializable]
    private class UiSchemaScreen
    {
        public string name;
        public string type;
        public UiSchemaReferenceSize referenceSize;
        public string anchor;
        public List<UiSchemaNode> children;
    }

    [Serializable]
    private class UiSchemaNode
    {
        public string name;
        public string type;
        public string text;
        public string anchor;
        public string imagePlaceholder;
        public UiSchemaBounds boundsPx;
        public List<UiSchemaNode> children;
    }

    [Serializable]
    private class UiSchemaBounds
    {
        public float x;
        public float y;
        public float width;
        public float height;
    }

    [Serializable]
    private class UiSchemaReferenceSize
    {
        public float width;
        public float height;
    }
}
