using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class CaseOpeningUILayoutBuilder
{
    private const string CanvasName = "Canvas";

    [MenuItem("Tools/Case Opening/Create UI Layout")]
    public static void CreateUILayout()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject(CanvasName, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
        if (scaler == null) scaler = canvas.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        scaler.matchWidthOrHeight = 0.5f;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Stretch(canvasRect);

        Image background = GetOrCreateImage(canvas.transform, "ui_panel_background");
        SetRect(background.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
        background.color = new Color(0.055f, 0.065f, 0.085f, 1f);
        background.raycastTarget = false;

        RectTransform header = GetOrCreatePanel(canvas.transform, "ui_panel_header", new Color(0.08f, 0.09f, 0.12f, 1f));
        SetRect(header, new Vector2(0f, 0.88f), new Vector2(1f, 1f), new Vector2(32f, -12f), new Vector2(-32f, -12f), Vector2.zero, Vector2.zero);

        RectTransform casePanel = GetOrCreatePanel(canvas.transform, "ui_panel_case", new Color(0.09f, 0.10f, 0.13f, 1f));
        SetRect(casePanel, new Vector2(0.06f, 0.48f), new Vector2(0.29f, 0.84f), Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
        CreateText(casePanel, "ui_text_case_name", "GLOVE CASE", 30, TextAlignmentOptions.Center,
            new Vector2(0.05f, 0.82f), new Vector2(0.95f, 0.98f));
        Image caseImage = GetOrCreateImage(casePanel, "ui_image_case");
        SetRect(caseImage.rectTransform, new Vector2(0.12f, 0.12f), new Vector2(0.88f, 0.78f), Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
        caseImage.color = new Color(0.18f, 0.20f, 0.25f, 1f);
        caseImage.preserveAspect = true;
        caseImage.raycastTarget = false;

        RectTransform roulettePanel = GetOrCreatePanel(canvas.transform, "ui_panel_roulette", new Color(0.07f, 0.08f, 0.105f, 1f));
        SetRect(roulettePanel, new Vector2(0.33f, 0.48f), new Vector2(0.94f, 0.80f), Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
        if (roulettePanel.GetComponent<RectMask2D>() == null) roulettePanel.gameObject.AddComponent<RectMask2D>();
        Image rouletteBackground = GetOrCreateImage(roulettePanel, "ui_image_roulette_background");
        Stretch(rouletteBackground.rectTransform);
        rouletteBackground.color = new Color(0.12f, 0.13f, 0.17f, 1f);
        rouletteBackground.raycastTarget = false;

        RectTransform content = GetOrCreateRect(roulettePanel, "ui_content_roulette");
        SetRect(content, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f));
        content.sizeDelta = new Vector2(2400f, 230f);

        RectTransform actions = GetOrCreatePanel(canvas.transform, "ui_panel_actions", new Color(0.08f, 0.09f, 0.12f, 1f));
        SetRect(actions, new Vector2(0.33f, 0.20f), new Vector2(0.94f, 0.42f), Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
        CreateButton(actions, "ui_button_open", "OPEN CASE", new Vector2(0.55f, 0.15f), new Vector2(0.95f, 0.85f));
        CreateText(actions, "ui_text_balance_value", "$0.00", 24, TextAlignmentOptions.MidlineLeft,
            new Vector2(0.05f, 0.15f), new Vector2(0.48f, 0.85f));

        RectTransform inventory = GetOrCreatePanel(canvas.transform, "ui_panel_inventory", new Color(0.075f, 0.085f, 0.11f, 1f));
        SetRect(inventory, new Vector2(0.06f, 0.06f), new Vector2(0.94f, 0.16f), Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
        CreateText(inventory, "ui_text_inventory_title", "INVENTORY", 20, TextAlignmentOptions.MidlineLeft,
            new Vector2(0.02f, 0.1f), new Vector2(0.98f, 0.9f));

        RectTransform result = GetOrCreatePanel(canvas.transform, "ui_panel_result", new Color(0.015f, 0.02f, 0.03f, 0.94f));
        Stretch(result);
        CreateText(result, "ui_text_result_title", "YOU RECEIVED", 32, TextAlignmentOptions.Center,
            new Vector2(0.2f, 0.76f), new Vector2(0.8f, 0.9f));
        Image resultImage = GetOrCreateImage(result, "ui_image_result_item");
        SetRect(resultImage.rectTransform, new Vector2(0.4f, 0.36f), new Vector2(0.6f, 0.72f), Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
        resultImage.color = new Color(0.18f, 0.20f, 0.25f, 1f);
        resultImage.preserveAspect = true;
        resultImage.raycastTarget = false;
        CreateText(result, "ui_text_result_name_value", "ITEM NAME", 24, TextAlignmentOptions.Center,
            new Vector2(0.25f, 0.25f), new Vector2(0.75f, 0.34f));
        CreateButton(result, "ui_button_close", "CONTINUE", new Vector2(0.4f, 0.12f), new Vector2(0.6f, 0.21f));
        result.gameObject.SetActive(false);

        EditorUtility.SetDirty(canvas.gameObject);
        Debug.Log("Case Opening UI layout created. Assign your sprites and connect gameplay references.");
    }

    private static RectTransform GetOrCreatePanel(Transform parent, string name, Color color)
    {
        Image image = GetOrCreateImage(parent, name);
        image.color = color;
        image.raycastTarget = false;
        return image.rectTransform;
    }

    private static Image GetOrCreateImage(Transform parent, string name)
    {
        Transform existing = parent.Find(name);
        if (existing != null)
        {
            Image found = existing.GetComponent<Image>();
            if (found != null) return found;
            return existing.gameObject.AddComponent<Image>();
        }
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        go.transform.SetParent(parent, false);
        return go.GetComponent<Image>();
    }

    private static RectTransform GetOrCreateRect(Transform parent, string name)
    {
        Transform existing = parent.Find(name);
        if (existing != null) return existing.GetComponent<RectTransform>();
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go.GetComponent<RectTransform>();
    }

    private static TextMeshProUGUI CreateText(Transform parent, string name, string value, float size, TextAlignmentOptions alignment, Vector2 anchorMin, Vector2 anchorMax)
    {
        Transform existing = parent.Find(name);
        GameObject go = existing != null ? existing.gameObject : new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);
        TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
        if (text == null) text = go.AddComponent<TextMeshProUGUI>();
        text.text = value;
        text.fontSize = size;
        text.alignment = alignment;
        text.color = Color.white;
        text.raycastTarget = false;
        SetRect(text.rectTransform, anchorMin, anchorMax, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
        return text;
    }

    private static Button CreateButton(Transform parent, string name, string label, Vector2 anchorMin, Vector2 anchorMax)
    {
        Transform existing = parent.Find(name);
        GameObject go = existing != null ? existing.gameObject : new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        go.transform.SetParent(parent, false);
        Image image = go.GetComponent<Image>();
        if (image == null) image = go.AddComponent<Image>();
        image.color = new Color(0.75f, 0.48f, 0.18f, 1f);
        Button button = go.GetComponent<Button>();
        if (button == null) button = go.AddComponent<Button>();
        button.targetGraphic = image;
        SetRect(go.GetComponent<RectTransform>(), anchorMin, anchorMax, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
        CreateText(go.transform, name + "_value", label, 22, TextAlignmentOptions.Center, Vector2.zero, Vector2.one);
        return button;
    }

    private static void SetRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax, Vector2 pivot, Vector2 anchoredPosition)
    {
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;
        rect.pivot = pivot == Vector2.zero ? new Vector2(0.5f, 0.5f) : pivot;
        rect.anchoredPosition = anchoredPosition;
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.pivot = new Vector2(0.5f, 0.5f);
    }
}
