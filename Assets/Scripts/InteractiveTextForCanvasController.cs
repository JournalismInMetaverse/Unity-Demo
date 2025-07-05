// 3. InteractiveTextForCanvasController.cs
// A new text controller to work with the new canvas system.
// Place this on your main article text object.

using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InteractiveTextForCanvasController : MonoBehaviour, IPointerClickHandler
{
    [Header("Manager Reference")]
    [Tooltip("The manager that controls which canvas is focused.")]
    public FocusableCanvasManager focusableCanvasManager;

    [Header("Contextual Links")]
    [Tooltip("A list that links text IDs to the context canvas GameObjects.")]
    public List<ContextualCanvasLink> contextualLinks;
    
    private TextMeshProUGUI mainText;
    private Dictionary<string, GameObject> linkIdToCanvasMap = new Dictionary<string, GameObject>();
    private Camera mainCamera;

    [System.Serializable]
    public struct ContextualCanvasLink
    {
        public string linkID;
        public GameObject contextCanvas;
    }
    
    void Start()
    {
        mainText = GetComponent<TextMeshProUGUI>();
        mainCamera = Camera.main;

        // Populate our dictionary for fast lookups.
        foreach (var link in contextualLinks)
        {
            if (link.contextCanvas != null)
            {
                linkIdToCanvasMap[link.linkID] = link.contextCanvas;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(mainText, eventData.position, mainCamera);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = mainText.textInfo.linkInfo[linkIndex];
            string linkID = linkInfo.GetLinkID();

            if (linkIdToCanvasMap.TryGetValue(linkID, out GameObject canvasGameObject))
            {
                FocusableCanvas canvasToFocus = canvasGameObject.GetComponent<FocusableCanvas>();
                
                if(canvasToFocus != null && focusableCanvasManager != null)
                {
                    focusableCanvasManager.OnCanvasClicked(canvasToFocus);
                }
            }
        }
    }
}