using System.Collections;
using TMPro;
using UnityEngine;

public class Unstuck : MonoBehaviour
{
    [SerializeField] private Collider2D[] colliders; // Array to store colliders
    [SerializeField] private GameObject onTextObject;
    [SerializeField] private GameObject offTextObject;

    private TextMeshPro onText;
    private TextMeshPro offText;
    private bool isNoClipActive = false; // Flag to track no-clip status

    void Awake()
    {
        // Find all colliders attached to the player (both on the player and its children)
        colliders = GameObject.Find("Player").GetComponentsInChildren<Collider2D>();
        onText = onTextObject.GetComponent<TextMeshPro>();
        offText = offTextObject.GetComponent<TextMeshPro>();
    }

    // This method toggles the no-clip mode when called (via a button press)
    public void ToggleNoClip()
    {
        // Toggle the no-clip state
        isNoClipActive = !isNoClipActive;

        if (isNoClipActive)
        {
            // Disable all colliders to simulate no-clip
            foreach (var collider in colliders)
            {
                offTextObject.SetActive(false);
                onTextObject.SetActive(true);
                collider.enabled = false;
            }
        }
        else
        {
            // Enable all colliders to turn off no-clip
            foreach (var collider in colliders)
            {
                offTextObject.SetActive(true);
                onTextObject.SetActive(false);
                collider.enabled = true;
            }
        }
    }
}