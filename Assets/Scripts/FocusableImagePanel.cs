// 2. FocusableImagePanel.cs (CORRECTED)
// Place this script on each of your root 3D Cube panel GameObjects.

using UnityEngine;
using UnityEngine.UI;

public class FocusableImagePanel : MonoBehaviour
{
    // Private state variables
    private FocusableImageManager manager;
    private Vector3 startPosition, startScale;
    private Quaternion startRotation;
    
    private Transform focusMarker;
    private bool isMoving = false;
    private bool isFocused = false;
    
    // --- THIS IS THE KEY CHANGE ---
    // Changed from RectTransform to the universal Transform.
    private Transform objectTransform;

    void Awake()
    {
        // --- THIS IS THE KEY CHANGE ---
        // GetComponent<Transform>() will get the transform of our 3D Cube.
        objectTransform = GetComponent<Transform>();
        
        // Automatically record its unique starting state
        startPosition = objectTransform.position;
        startRotation = objectTransform.rotation;
        startScale = objectTransform.localScale;
    }

    public void RegisterManager(FocusableImageManager mgr)
    {
        manager = mgr;
    }

    /// <summary>
    /// This is called by the panel's Button component OnClick() event.
    /// </summary>
    public void WasClicked()
    {
        if (manager != null)
        {
            manager.OnPanelClicked(this);
        }
    }

    public void MoveToFocus(Transform target)
    {
        focusMarker = target;
        isFocused = true;
        isMoving = true;
    }

    public void MoveToStart()
    {
        isFocused = false;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        // Determine the target transform based on the current state
        Vector3 targetPosition = isFocused ? focusMarker.position : startPosition;
        Quaternion targetRotation = isFocused ? focusMarker.rotation : startRotation;
        Vector3 targetScale = isFocused ? focusMarker.localScale : startScale;
        
        // Smoothly move, rotate, and scale towards the target
        // We now use objectTransform instead of rectTransform
        objectTransform.position = Vector3.Lerp(objectTransform.position, targetPosition, Time.deltaTime * 10f);
        objectTransform.rotation = Quaternion.Slerp(objectTransform.rotation, targetRotation, Time.deltaTime * 10f);
        objectTransform.localScale = Vector3.Lerp(objectTransform.localScale, targetScale, Time.deltaTime * 10f);

        // Check if we are close enough to stop
        if (Vector3.Distance(objectTransform.position, targetPosition) < 0.01f)
        {
            // Snap to the final state to ensure precision
            objectTransform.position = targetPosition;
            objectTransform.rotation = targetRotation;
            objectTransform.localScale = targetScale;
            isMoving = false;
        }
    }
}