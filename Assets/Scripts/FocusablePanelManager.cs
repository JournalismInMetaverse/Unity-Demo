// 1. FocusablePanelManager.cs (SIMPLIFIED)
// All text highlighting logic has been removed.

using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class FocusablePanelManager : MonoBehaviour
{
    [Header("Focus Point")]
    [Tooltip("An empty GameObject marking the position, rotation, and scale for a focused panel.")]
    public Transform focusMarker;

    // --- Private variables ---
    private List<FocusablePanel> allPanels = new List<FocusablePanel>();
    private FocusablePanel currentlyFocusedPanel = null;

    void Start()
    {
        allPanels.AddRange(FindObjectsOfType<FocusablePanel>());
        foreach (var panel in allPanels)
        {
            panel.RegisterManager(this);
        }
    }

    /// <summary>
    /// This is the primary entry point, called by both panel clicks and text link clicks.
    /// </summary>
    public void OnPanelClicked(FocusablePanel clickedPanel)
    {
        // Case 1: The clicked panel is already focused. Send it home and show all others.
        if (clickedPanel == currentlyFocusedPanel)
        {
            clickedPanel.MoveToStart();
            currentlyFocusedPanel = null;
            ShowAllPanels();
        }
        // Case 2: A new panel is being focused.
        else
        {
            // If another panel was focused, send it home first.
            if (currentlyFocusedPanel != null)
            {
                currentlyFocusedPanel.MoveToStart();
            }

            clickedPanel.gameObject.SetActive(true);
            HideAllPanelsExcept(clickedPanel);
            clickedPanel.MoveToFocus(focusMarker);
            currentlyFocusedPanel = clickedPanel;
        }
    }
    
    // --- Helper methods for showing/hiding panels ---
    private void ShowAllPanels()
    {
        foreach (var panel in allPanels) { panel.gameObject.SetActive(true); }
    }
    private void HideAllPanelsExcept(FocusablePanel exception)
    {
        foreach (var panel in allPanels) { if (panel != exception) { panel.gameObject.SetActive(false); } }
    }
}