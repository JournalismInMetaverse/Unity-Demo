// HyperlinkHandler.cs

using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

// This script must be on the same GameObject as the TextMeshProUGUI component.
public class HyperlinkHandler : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI pTextMeshPro;
    private Camera mainCamera;

    void Awake()
    {
        pTextMeshPro = GetComponent<TextMeshProUGUI>();
        mainCamera = Camera.main;
    }

    /// <summary>
    /// This method is called automatically when this UI element is clicked.
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        // Find which link, if any, was under the pointer when the click happened.
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, eventData.position, mainCamera);

        // If a link was clicked...
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
            string linkID = linkInfo.GetLinkID();

            // Check if the link ID starts with "http". If so, treat it as a URL.
            if (linkID.StartsWith("http://") || linkID.StartsWith("https://"))
            {
                // Open the URL in the system's default web browser.
                Application.OpenURL(linkID);
                Debug.Log($"Opening URL: {linkID}");
            }
        }
    }
}