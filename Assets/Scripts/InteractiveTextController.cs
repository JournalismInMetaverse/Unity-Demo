// InteractiveTextController.cs (Simplified for Click-to-Focus only)

using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems; // Required for click handling

// The script now implements IPointerClickHandler to receive clicks.
public class InteractiveTextController : MonoBehaviour, IPointerClickHandler
{
    [Header("Manager Reference")]
    [Tooltip("The manager that controls which panel is focused.")]
    public FocusablePanelManager focusablePanelManager;

    [Header("Contextual Links")]
    [Tooltip("A list that links text IDs to the pop-up panel GameObjects.")]
    public List<ContextualLink> contextualLinks;
    
    // --- Private Variables ---
    private TextMeshProUGUI mainText;
    private Dictionary<string, GameObject> linkIdToPanelMap = new Dictionary<string, GameObject>();
    private Camera mainCamera;

    [System.Serializable]
    public struct ContextualLink
    {
        public string linkID;
        public GameObject contextPanel;
    }
    
    void Start()
    {
        // Automatically get the TextMeshPro component from this same GameObject
        mainText = GetComponent<TextMeshProUGUI>();
        if (mainText == null)
        {
            Debug.LogError("InteractiveTextController needs to be on the same GameObject as a TextMeshProUGUI component.");
            return;
        }

        mainCamera = Camera.main;

        // Populate our dictionary for fast lookups. The script no longer hides the panels.
        foreach (var link in contextualLinks)
        {
            if (link.contextPanel != null)
            {
                linkIdToPanelMap[link.linkID] = link.contextPanel;
            }
        }
    }

    /// <summary>
    /// This method is called automatically when this UI element is clicked.
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        // Find which link, if any, was under the pointer when the click happened.
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(mainText, eventData.position, mainCamera);

        // If a link was clicked...
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = mainText.textInfo.linkInfo[linkIndex];
            string linkID = linkInfo.GetLinkID();

            // Find the corresponding panel GameObject from our dictionary.
            if (linkIdToPanelMap.TryGetValue(linkID, out GameObject panelGameObject))
            {
                // Get the FocusablePanel component from that GameObject.
                FocusablePanel panelToFocus = panelGameObject.GetComponent<FocusablePanel>();
                
                // If the panel and the manager exist, tell the manager to focus it.
                if(panelToFocus != null && focusablePanelManager != null)
                {
                    focusablePanelManager.OnPanelClicked(panelToFocus);
                }
            }
        }
    }
}
