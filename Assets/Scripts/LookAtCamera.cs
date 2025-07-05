// LookAtCamera.cs

using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("If true, the panel will always face the camera.")]
    public bool lookAtCamera = true;
    
    [Tooltip("If true, the panel will only rotate horizontally (on the Y-axis). This is usually best for UI.")]
    public bool onlyRotateOnYAxis = true;
    
    [Tooltip("How quickly the panel turns to face the camera. A lower value is slower and smoother. Set to a high value like 100 for an instant snap.")]
    [Range(0.1f, 100f)]
    public float smoothingSpeed = 5.0f;

    private Transform cameraTransform;

    void Start()
    {
        // Find the main camera in the scene automatically
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("LookAtCamera Script: Could not find a camera tagged 'MainCamera' in the scene.");
            this.enabled = false; // Disable the script if no camera is found
        }
    }

    // LateUpdate runs after all other Update calls, which is the best time to adjust the camera
    void LateUpdate()
    {
        if (!lookAtCamera || cameraTransform == null)
        {
            return; // Do nothing if the feature is turned off or there's no camera
        }

        // Determine the target direction
        Vector3 targetDirection = cameraTransform.position - transform.position;

        // Optionally ignore the vertical difference
        if (onlyRotateOnYAxis)
        {
            targetDirection.y = 0;
        }

        // Calculate the target rotation
        // We use Quaternion.LookRotation to create a rotation that "looks" in the target direction
        Quaternion targetRotation = Quaternion.LookRotation(-targetDirection);

        // Smoothly rotate towards the target rotation over time
        // Time.deltaTime makes the rotation speed independent of the frame rate
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothingSpeed * Time.deltaTime);
    }
}
