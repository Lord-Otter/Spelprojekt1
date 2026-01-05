using TMPro;
using UnityEngine;

public class Unstuck : MonoBehaviour
{
    [SerializeField] private GameObject onTextObject;
    [SerializeField] private GameObject offTextObject;

    private Collider2D[] colliders;
    private TextMeshPro onText;
    private TextMeshPro offText;

    private bool isNoClipActive;

    private void Awake()
    {
        colliders = GameObject.Find("Player").GetComponentsInChildren<Collider2D>();

        onText = onTextObject.GetComponent<TextMeshPro>();
        offText = offTextObject.GetComponent<TextMeshPro>();

        SetNoClip(false);
    }
    public void ToggleNoClip()
    {
        SetNoClip(!isNoClipActive);
    }

    // Explicit control (other scripts, triggers, cutscenes, etc.)
    public void SetNoClip(bool enabled)
    {
        isNoClipActive = enabled;

        foreach (var col in colliders)
        {
            col.enabled = !enabled;
        }

        onTextObject.SetActive(enabled);
        offTextObject.SetActive(!enabled);
    }

    public void EnableNoClip() => SetNoClip(true);
    public void DisableNoClip() => SetNoClip(false);

    public bool IsNoClipActive => isNoClipActive;
}