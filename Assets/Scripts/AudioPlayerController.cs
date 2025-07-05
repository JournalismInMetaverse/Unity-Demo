// AudioPlayerController.cs

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.EventSystems; // Required for drag/click interfaces

// -------------------  THIS IS THE CRITICAL LINE -------------------
// It must include IPointerDownHandler and IPointerUpHandler to work with Event Triggers.
public class AudioPlayerController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Audio Components")]
    [Tooltip("The Audio Source component that will play the .wav file.")]
    public AudioSource audioSource;

    [Header("UI Controls")]
    [Tooltip("The main slider to show and control audio progress.")]
    public Slider progressBar;
    [Tooltip("The button to play and pause the audio.")]
    public Button playPauseButton;
    [Tooltip("The button to jump backward 10 seconds.")]
    public Button jumpBackButton;
    [Tooltip("The button to jump forward 10 seconds.")]
    public Button jumpForwardButton;

    [Header("UI Text Display")]
    [Tooltip("The TextMeshPro text on the play/pause button.")]
    public TextMeshProUGUI playPauseButtonText;
    [Tooltip("The TextMeshPro text to display the current and total time.")]
    public TextMeshProUGUI timeDisplayText;

    private bool isScrubbing = false; // Flag to check if the user is dragging the slider

    void Start()
    {
        if (audioSource == null || progressBar == null || playPauseButton == null || timeDisplayText == null)
        {
            Debug.LogError("AudioPlayerController is missing required component references!");
            this.enabled = false;
            return;
        }

        // Add listeners to the buttons to call our methods
        playPauseButton.onClick.AddListener(TogglePlayPause);
        jumpBackButton.onClick.AddListener(JumpBackward);
        jumpForwardButton.onClick.AddListener(JumpForward);
        
        // Initialize the UI
        UpdateUI();
    }

    void Update()
    {
        // Only update the slider value if the audio is playing and the user is NOT dragging it
        if (audioSource.isPlaying && !isScrubbing)
        {
            progressBar.value = audioSource.time / audioSource.clip.length;
        }
        
        // Always update the time display text
        UpdateUITime();
    }

    /// <summary>
    /// Toggles the audio between playing and pausing.
    /// </summary>
    public void TogglePlayPause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }
        UpdateUIButtonText();
    }

    /// <summary>
    /// Jumps the audio playback forward by 10 seconds.
    /// </summary>
    public void JumpForward()
    {
        // Use Mathf.Min to ensure we don't jump past the end of the clip
        audioSource.time = Mathf.Min(audioSource.clip.length, audioSource.time + 5f);
        UpdateUI();
    }

    /// <summary>
    /// Jumps the audio playback backward by 10 seconds.
    /// </summary>
    public void JumpBackward()
    {
        // Use Mathf.Max to ensure we don't jump before the start of the clip
        audioSource.time = Mathf.Max(0f, audioSource.time - 5f);
        UpdateUI();
    }

    /// <summary>
    /// Called by an Event Trigger when the user presses down on this GameObject.
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        // Check if the click/press started on the progress bar's rectangle
        if (RectTransformUtility.RectangleContainsScreenPoint(progressBar.GetComponent<RectTransform>(), eventData.position))
        {
            isScrubbing = true;
        }
    }

    /// <summary>
    /// Called by an Event Trigger when the user releases the pointer.
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isScrubbing)
        {
            // Get the value from the slider where the user released the pointer
            float value = progressBar.value;
            audioSource.time = value * audioSource.clip.length;
            isScrubbing = false;
        }
    }

    // --- Helper Methods to Update UI ---

    private void UpdateUI()
    {
        UpdateUITime();
        UpdateUIButtonText();
    }

    private void UpdateUITime()
    {
        string totalTime = FormatTime(audioSource.clip.length);
        string currentTime = FormatTime(audioSource.time);
        timeDisplayText.text = $"{currentTime} / {totalTime}";
    }

    private void UpdateUIButtonText()
    {
        playPauseButtonText.text = audioSource.isPlaying ? "Pause" : "Play";
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
