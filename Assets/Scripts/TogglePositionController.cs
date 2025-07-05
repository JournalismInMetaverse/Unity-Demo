// TogglePositionController.cs (Self-Contained Version)

using UnityEngine;

public class TogglePositionController : MonoBehaviour
{
    [Header("Target Position Marker")]
    [Tooltip("An empty GameObject marking the target position and rotation.")]
    public Transform endMarker;

    [Header("Movement Settings")]
    [Tooltip("How fast the object moves between positions.")]
    public float moveSpeed = 8f;
    [Tooltip("How fast the object rotates between rotations.")]
    public float rotateSpeed = 8f;

    // Private variables to store the initial state
    private Vector3 startPosition;
    private Quaternion startRotation;

    // State tracking
    private bool isAtEndPosition = false;
    private bool isMoving = false;

    // This script now acts on its own Transform component
    private RectTransform objectToMove;

    void Awake()
    {
        // Get the RectTransform of this GameObject
        objectToMove = GetComponent<RectTransform>();

        // --- NEW: Automatically record the starting position ---
        // This is the key change that removes the need for a StartPositionMarker.
        startPosition = objectToMove.position;
        startRotation = objectToMove.rotation;
    }

    /// <summary>
    /// This is the public function you will call from your UI Button's OnClick() event.
    /// </summary>
    public void TogglePosition()
    {
        if (isMoving) return;
        isAtEndPosition = !isAtEndPosition;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving || objectToMove == null || endMarker == null)
        {
            return;
        }

        // Determine the target position and rotation based on the current state
        Vector3 targetPosition = isAtEndPosition ? endMarker.position : startPosition;
        Quaternion targetRotation = isAtEndPosition ? endMarker.rotation : startRotation;
        
        // --- Smoothly move towards the target position ---
        objectToMove.position = Vector3.Lerp(objectToMove.position, targetPosition, moveSpeed * Time.deltaTime);

        // --- Smoothly rotate towards the target rotation ---
        objectToMove.rotation = Quaternion.Slerp(objectToMove.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        // Check if the object is close enough to the target to stop moving
        float distanceToTarget = Vector3.Distance(objectToMove.position, targetPosition);
        float angleToTarget = Quaternion.Angle(objectToMove.rotation, targetRotation);

        if (distanceToTarget < 0.01f && angleToTarget < 1.0f)
        {
            // Snap to the final position and rotation to ensure precision
            objectToMove.position = targetPosition;
            objectToMove.rotation = targetRotation;
            isMoving = false;
        }
    }
}
