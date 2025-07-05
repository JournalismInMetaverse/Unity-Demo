// 2. VideoPlayerInstance.cs (UPDATED)
// Added a line to clear listeners to prevent double-click bugs.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerInstance : MonoBehaviour
{
    [Header("Core Components")]
    [Tooltip("The Video Player component for this specific instance.")]
    public VideoPlayer videoPlayer;
    [Tooltip("The Render Texture used by this instance's 2D display.")]
    public RenderTexture renderTexture;
    [Tooltip("The Canvas that holds the 2D version of this video player.")]
    public GameObject videoPlayerCanvas;
    [Tooltip("The full-screen button for this specific video player.")]
    public Button fullScreenButton;
    [Tooltip("The parent GameObject of the UI controls (play button, slider, etc.).")]
    public GameObject controlsCanvas;

    // --- Private State ---
    private FullScreenManager manager;
    private Transform originalControlsParent;
    private Vector3 originalControlsPosition;
    private Quaternion originalControlsRotation;
    private Vector3 originalControlsScale;

    void Start()
    {
        manager = FindObjectOfType<FullScreenManager>();
        if (manager == null)
        {
            Debug.LogError("No FullScreenManager found in the scene!");
            return;
        }

        if (fullScreenButton != null)
        {
            // --- THIS IS THE FIX ---
            // First, remove any existing listeners to prevent duplicates.
            fullScreenButton.onClick.RemoveAllListeners();
            // Then, add our listener. This ensures it only ever has one.
            fullScreenButton.onClick.AddListener(OnFullScreenButtonClicked);
        }

        if (controlsCanvas != null)
        {
            originalControlsParent = controlsCanvas.transform.parent;
            originalControlsPosition = controlsCanvas.transform.localPosition;
            originalControlsRotation = controlsCanvas.transform.localRotation;
            originalControlsScale = controlsCanvas.transform.localScale;
        }
    }
    
    public void OnFullScreenButtonClicked()
    {
        if (manager != null)
        {
            manager.RequestFullScreen(this);
        }
    }

    public void OnEnterFullScreen(Renderer fullScreenRenderer, Transform controlsTarget)
    {
        videoPlayerCanvas.SetActive(false);
        videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        videoPlayer.targetMaterialRenderer = fullScreenRenderer;

        if (controlsCanvas != null && controlsTarget != null)
        {
            controlsCanvas.transform.SetParent(controlsTarget, true);
            controlsCanvas.transform.position = controlsTarget.position;
            controlsCanvas.transform.rotation = controlsTarget.rotation;
            controlsCanvas.transform.localScale = controlsTarget.localScale;
        }
    }

    public void OnExitFullScreen()
    {
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoPlayerCanvas.SetActive(true);

        if (controlsCanvas != null && originalControlsParent != null)
        {
            controlsCanvas.transform.SetParent(originalControlsParent, true);
            controlsCanvas.transform.localPosition = originalControlsPosition;
            controlsCanvas.transform.localRotation = originalControlsRotation;
            controlsCanvas.transform.localScale = originalControlsScale;
        }
    }
}