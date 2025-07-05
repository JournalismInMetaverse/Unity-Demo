// VideoPlayerController.cs

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems; // Required for drag/click interfaces

public class VideoPlayerController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Video Player Components")]
    [Tooltip("The main Video Player component.")]
    public VideoPlayer videoPlayer;
    [Tooltip("The Raw Image UI element where the video will be displayed.")]
    public RawImage videoDisplay;

    [Header("UI Controls")]
    [Tooltip("The button to play/pause the video.")]
    public Button playPauseButton;
    [Tooltip("The TextMeshPro UGUI component on the play/pause button.")]
    public TMPro.TextMeshProUGUI playPauseButtonText;
    [Tooltip("The slider that shows video progress.")]
    public Slider progressBar;
    [Tooltip("The Image component that displays the video thumbnail.")]
    public GameObject thumbnailObject;

    private bool isDraggingProgressBar = false;

    void Start()
    {
        if (videoPlayer == null || videoDisplay == null || playPauseButton == null || progressBar == null)
        {
            Debug.LogError("Video Player Controller is missing one or more required components!");
            return;
        }

        // Prepare the video player
        videoPlayer.prepareCompleted += PrepareCompleted;
        videoPlayer.Prepare();

        // Add a listener to the button to call our PlayPause method
        playPauseButton.onClick.AddListener(PlayPause);

        // Set the text to "Play" initially
        if(playPauseButtonText != null) playPauseButtonText.text = "Play";
    }

    void Update()
    {
        // Update the progress bar only if the video is playing and we are not dragging it
        if (videoPlayer.isPlaying && !isDraggingProgressBar)
        {
            // The videoPlayer.time and .length are in seconds. The slider value is 0-1.
            progressBar.value = (float)(videoPlayer.time / videoPlayer.length);
        }
    }

    /// <summary>
    /// This method is called by the Video Player once it's ready to play.
    /// </summary>
    private void PrepareCompleted(VideoPlayer source)
    {
        // Assign the video texture to the Raw Image display
        videoDisplay.texture = source.texture;
        playPauseButton.interactable = true; // Enable the play button
    }

    /// <summary>
    /// Toggles between playing and pausing the video.
    /// </summary>
    public void PlayPause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            if(playPauseButtonText != null) playPauseButtonText.text = "Play";
        }
        else
        {
            // Hide the thumbnail when play is pressed for the first time
            if(thumbnailObject != null && thumbnailObject.activeSelf)
            {
                thumbnailObject.SetActive(false);
            }
            videoPlayer.Play();
            if(playPauseButtonText != null) playPauseButtonText.text = "Pause";
        }
    }

    /// <summary>
    /// Called when the user presses down on the progress bar.
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(progressBar.GetComponent<RectTransform>(), eventData.position))
        {
            isDraggingProgressBar = true;
        }
    }

    /// <summary>
    /// Called when the user releases the pointer from the progress bar.
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDraggingProgressBar)
        {
            Seek(progressBar.value);
            isDraggingProgressBar = false;
        }
    }
    
    /// <summary>
    /// Seeks the video to a specific point based on the slider's value.
    /// </summary>
    private void Seek(float normalizedTime)
    {
        if(videoPlayer.canSetTime)
        {
            videoPlayer.time = normalizedTime * videoPlayer.length;
        }
    }
}
