// 1. FocusableImageManager.cs
// This is the central manager. Place it on an empty GameObject in your scene.
// THIS SCRIPT HAS NOT CHANGED.

using UnityEngine;
using System.Collections.Generic;

public class FocusableImageManager : MonoBehaviour
{
    [Header("Focus Point")]
    [Tooltip("An empty GameObject marking the position, rotation, and scale for a focused panel.")]
    public Transform focusMarker;

    private List<FocusableImagePanel> allPanels = new List<FocusableImagePanel>();
    private FocusableImagePanel currentlyFocusedPanel = null;

    void Start()
    {
        // Automatically find and register all panels in the scene
        allPanels.AddRange(FindObjectsOfType<FocusableImagePanel>());
        foreach (var panel in allPanels)
        {
            panel.RegisterManager(this);
        }
    }

    /// <summary>
    /// Called by a FocusableImagePanel when it is clicked. This is the core logic.
    /// </summary>
    public void OnPanelClicked(FocusableImagePanel clickedPanel)
    {
        // If the clicked panel is already focused, send it home.
        if (clickedPanel == currentlyFocusedPanel)
        {
            clickedPanel.MoveToStart();
            currentlyFocusedPanel = null; // No panel is focused now
        }
        // If a different panel is clicked...
        else
        {
            // If another panel was already focused, send it home first.
            if (currentlyFocusedPanel != null)
            {
                currentlyFocusedPanel.MoveToStart();
            }
            
            // Tell the clicked panel to move to the focus point and become the new focused panel.
            clickedPanel.MoveToFocus(focusMarker);
            currentlyFocusedPanel = clickedPanel;
        }
    }
}