// 2. MovableImage.cs
// Place this script on every clickable image GameObject.

using UnityEngine;

public class MovableImage : MonoBehaviour
{
    // Private variables to store state and references
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Transform endMarker;
    private MovableImageManager manager;

    private bool isMoving = false;
    private bool isAtEndPosition = false;

    // Settings for movement feel
    private float moveSpeed = 10f;
    private float rotateSpeed = 10f;
    
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.position;
        startRotation = rectTransform.rotation;
    }
    
    public void RegisterManager(MovableImageManager mgr)
    {
        manager = mgr;
    }

    /// <summary>
    /// This is called by the Button's OnClick() event.
    /// </summary>
    public void WasClicked()
    {
        if (manager != null)
        {
            manager.OnImageClicked(this);
        }
    }

    public void MoveToEnd(Transform target)
    {
        endMarker = target;
        isAtEndPosition = true;
        isMoving = true;
    }

    public void MoveToStart()
    {
        isAtEndPosition = false;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        Vector3 targetPosition = isAtEndPosition ? endMarker.position : startPosition;
        Quaternion targetRotation = isAtEndPosition ? endMarker.rotation : startRotation;
        
        rectTransform.position = Vector3.Lerp(rectTransform.position, targetPosition, moveSpeed * Time.deltaTime);
        rectTransform.rotation = Quaternion.Slerp(rectTransform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        if (Vector3.Distance(rectTransform.position, targetPosition) < 0.01f)
        {
            // Snap to the final position to fix the "not returning perfectly" bug
            rectTransform.position = targetPosition;
            rectTransform.rotation = targetRotation;
            isMoving = false;
        }
    }
}