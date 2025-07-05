// 2. InteractiveImage.cs (UPDATED)
// This script now has a reference to its button to enable/disable it.

using UnityEngine;
using UnityEngine.UI; // NEW: Required for the Button component

public class InteractiveImage : MonoBehaviour
{
    [Header("Position Markers")]
    [Tooltip("An empty GameObject marking the large, starting position and scale.")]
    public Transform primaryMarker;
    [Tooltip("An empty GameObject marking the smaller, secondary position and scale.")]
    public Transform secondaryMarker;

    [Header("Component Reference")]
    [Tooltip("The Button component that makes this image clickable.")]
    public Button imageButton; // NEW: Reference to the button

    // --- Private State & References ---
    private SceneStateManager manager;
    private Transform targetMarker;
    private bool isMoving = false;

    void Awake()
    {
        // NEW: Disable the button by default when the scene starts.
        SetInteractable(false);
    }

    public void RegisterManager(SceneStateManager mgr)
    {
        manager = mgr;
    }

    /// <summary>
    /// Called by this object's Button component OnClick() event.
    /// </summary>
    public void WasClicked()
    {
        if (manager != null)
        {
            manager.OnImageClicked(this);
        }
    }

    // --- Public methods called by the manager ---
    public void MoveToPrimary()
    {
        targetMarker = primaryMarker;
        isMoving = true;
    }

    public void MoveToSecondary()
    {
        targetMarker = secondaryMarker;
        isMoving = true;
    }

    // NEW: Public method to control if the button can be clicked.
    public void SetInteractable(bool isInteractable)
    {
        if (imageButton != null)
        {
            imageButton.interactable = isInteractable;
        }
    }

    void Update()
    {
        if (!isMoving || targetMarker == null) return;

        // Smoothly move, rotate, and scale towards the target marker
        transform.position = Vector3.Lerp(transform.position, targetMarker.position, Time.deltaTime * 8f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetMarker.rotation, Time.deltaTime * 8f);
        transform.localScale = Vector3.Lerp(transform.localScale, targetMarker.localScale, Time.deltaTime * 8f);

        // Check if we are close enough to stop
        if (Vector3.Distance(transform.position, targetMarker.position) < 0.01f)
        {
            // Snap to the final state to ensure precision
            transform.position = targetMarker.position;
            transform.rotation = targetMarker.rotation;
            transform.localScale = targetMarker.localScale;
            isMoving = false;
        }
    }
}