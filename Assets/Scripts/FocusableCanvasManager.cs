// 1. FocusableCanvasManager.cs
// This is the central manager. Place it on an empty GameObject.

using UnityEngine;
using System.Collections.Generic;

public class FocusableCanvasManager : MonoBehaviour
{
    [Header("Focus Point")]
    [Tooltip("An empty GameObject marking the position, rotation, and scale for a focused canvas.")]
    public Transform focusMarker;

    private List<FocusableCanvas> allCanvases = new List<FocusableCanvas>();
    private FocusableCanvas currentlyFocusedCanvas = null;

    void Start()
    {
        // Automatically find and register all canvases in the scene
        allCanvases.AddRange(FindObjectsOfType<FocusableCanvas>());
        foreach (var canvas in allCanvases)
        {
            canvas.RegisterManager(this);
        }
    }

    /// <summary>
    /// Called by a FocusableCanvas or the text controller when a canvas should be focused.
    /// </summary>
    public void OnCanvasClicked(FocusableCanvas clickedCanvas)
    {
        // Case 1: The clicked canvas is already focused. Send it home and show all others.
        if (clickedCanvas == currentlyFocusedCanvas)
        {
            clickedCanvas.MoveToStart();
            currentlyFocusedCanvas = null;
            ShowAllCanvases();
        }
        // Case 2: A new canvas is being focused.
        else
        {
            // If another canvas was already focused, send it home first.
            if (currentlyFocusedCanvas != null)
            {
                currentlyFocusedCanvas.MoveToStart();
            }

            // Make sure the new canvas is visible before hiding the others.
            clickedCanvas.gameObject.SetActive(true);
            HideAllCanvasesExcept(clickedCanvas);
            
            // Tell the clicked canvas to move to the focus point.
            clickedCanvas.MoveToFocus(focusMarker);
            currentlyFocusedCanvas = clickedCanvas;
        }
    }

    private void ShowAllCanvases()
    {
        foreach (var canvas in allCanvases)
        {
            canvas.gameObject.SetActive(true);
        }
    }

    private void HideAllCanvasesExcept(FocusableCanvas exception)
    {
        foreach (var canvas in allCanvases)
        {
            if (canvas != exception)
            {
                canvas.gameObject.SetActive(false);
            }
        }
    }
}