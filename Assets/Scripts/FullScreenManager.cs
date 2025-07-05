// 1. FullScreenManager.cs (UPDATED)
// This manager now provides a marker for where the controls should go.

using UnityEngine;
using System.Collections.Generic;

public class FullScreenManager : MonoBehaviour
{
    [Header("Full-Screen Display")]
    [Tooltip("The GameObject that will display the video in full-screen (e.g., the curved mesh).")]
    public GameObject fullScreenDisplayObject;
    [Tooltip("An empty GameObject marking the position, rotation, and scale for the full-screen video.")]
    public Transform fullScreenMarker;
    [Tooltip("An empty GameObject marking where the video controls should be placed in full-screen mode.")]
    public Transform fullScreenControlsMarker; // NEW: Marker for the controls

    [Header("Scene Content to Hide")]
    [Tooltip("A list of all other root GameObjects in the scene that should be hidden during full-screen mode.")]
    public List<GameObject> otherSceneContent;

    // --- Private State ---
    private VideoPlayerInstance currentFullScreenInstance = null;

    void Start()
    {
        // Ensure the shared full-screen display is hidden initially
        if (fullScreenDisplayObject != null)
        {
            fullScreenDisplayObject.SetActive(false);
        }
    }

    public void RequestFullScreen(VideoPlayerInstance requestingInstance)
    {
        if (currentFullScreenInstance == requestingInstance)
        {
            ExitFullScreen();
        }
        else
        {
            if (currentFullScreenInstance != null)
            {
                currentFullScreenInstance.OnExitFullScreen();
            }
            EnterFullScreen(requestingInstance);
        }
    }

    private void EnterFullScreen(VideoPlayerInstance instance)
    {
        foreach (var content in otherSceneContent)
        {
            content.SetActive(false);
        }

        if (fullScreenDisplayObject != null && fullScreenMarker != null)
        {
            fullScreenDisplayObject.SetActive(true);
            fullScreenDisplayObject.transform.position = fullScreenMarker.position;
            fullScreenDisplayObject.transform.rotation = fullScreenMarker.rotation;
            fullScreenDisplayObject.transform.localScale = fullScreenMarker.localScale;
        }

        // Tell the specific video player instance to enter full-screen mode
        // NEW: We now pass the controls marker as well.
        instance.OnEnterFullScreen(fullScreenDisplayObject.GetComponentInChildren<Renderer>(), fullScreenControlsMarker);
        currentFullScreenInstance = instance;
    }

    private void ExitFullScreen()
    {
        if (currentFullScreenInstance == null) return;

        if (fullScreenDisplayObject != null)
        {
            fullScreenDisplayObject.SetActive(false);
        }

        currentFullScreenInstance.OnExitFullScreen();
        currentFullScreenInstance = null;

        foreach (var content in otherSceneContent)
        {
            content.SetActive(true);
        }
    }
}