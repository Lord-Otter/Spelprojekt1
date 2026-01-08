using UnityEngine;

public class ImageWindow : MonoBehaviour
{
    public TMPro.TextMeshProUGUI uiText; // Assign in Inspector
    private RectTransform rectTransform;
    private RectTransform imageTransform;

    void Awake()
    {
        if (uiText == null) uiText = GetComponent<TMPro.TextMeshProUGUI>();
        rectTransform = uiText.GetComponent<RectTransform>();
        imageTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Force Unity to recalculate preferred size
        //LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        // Apply preferred size
        rectTransform.sizeDelta = new Vector2(
            1000,
            uiText.preferredHeight
        );
        imageTransform.sizeDelta = rectTransform.sizeDelta;
    }
}
