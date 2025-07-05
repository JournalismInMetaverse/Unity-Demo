// 1. SceneStateManager.cs (UPDATED)
// This manager now correctly handles the full, multi-state start sequence as requested.

using UnityEngine;
using System.Collections.Generic;

public class SceneStateManager : MonoBehaviour
{
    [Header("Core Scene Objects")]
    [Tooltip("The canvas or parent object holding the start button and initial summary. This is VISIBLE at the start.")]
    public GameObject summaryCanvas;
    [Tooltip("The parent GameObject holding all main content (article, context windows, etc.) that is initially HIDDEN.")]
    public GameObject mainContent;
    [Tooltip("The button used to start the main experience. It should be a child of the Summary Canvas.")]
    public GameObject startButton;

    [Header("Interactive Images")]
    [Tooltip("A list of the two special InteractiveImage components in the scene.")]
    public List<InteractiveImage> interactiveImages;

    // --- Private State ---
    private bool isFocusedState = false;

    void Start()
    {
        // Initial setup: show the summary, hide the main content.
        if (summaryCanvas != null) summaryCanvas.SetActive(true);
        if (mainContent != null) mainContent.SetActive(false);

        // Tell each image to register this manager
        foreach (var image in interactiveImages)
        {
            image.RegisterManager(this);
        }
    }

    /// <summary>
    /// Called by the Start Button's OnClick() event.
    /// </summary>
    public void StartExperience()
    {
        // BUG FIX: The summary canvas should remain visible, so we no longer hide it here.
        // if (summaryCanvas != null) summaryCanvas.SetActive(false); 
        if (mainContent != null) mainContent.SetActive(true);

        // Tell both images to move to their smaller, secondary positions and become clickable.
        foreach (var image in interactiveImages)
        {
            image.MoveToSecondary();
            image.SetInteractable(true);
        }

        // Hide the start button
        startButton.SetActive(false);
    }

    /// <summary>
    /// Called by an InteractiveImage when it is clicked.
    /// </summary>
    public void OnImageClicked(InteractiveImage clickedImage)
    {
        // Case 1: An image is clicked while it is already focused. (Un-focusing)
        if (isFocusedState)
        {
            // Show the main content and summary canvas again
            if (mainContent != null) mainContent.SetActive(true);
            if (summaryCanvas != null) summaryCanvas.SetActive(true);
            
            // Tell both images to move to their secondary (small) positions
            foreach (var image in interactiveImages)
            {
                image.MoveToSecondary();
            }
            isFocusedState = false;
        }
        // Case 2: An image is clicked while not in focus. (Focusing)
        else
        {
            // Hide the main content AND the summary canvas
            if (mainContent != null) mainContent.SetActive(false);
            if (summaryCanvas != null) summaryCanvas.SetActive(false);

            // Tell both images to move to their primary (large) positions
            foreach (var image in interactiveImages)
            {
                image.MoveToPrimary();
            }
            isFocusedState = true;
        }
    }
}