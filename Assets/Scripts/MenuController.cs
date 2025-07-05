using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [Header("Menu Objects")]
    [Tooltip("The parent GameObject of your UI menu. This will be enabled/disabled.")]
    public GameObject menuCanvas;
    
    [Header("Positioning")]
    [Tooltip("The main XR Camera. Used to position the menu in front of the user.")]
    public Camera mainCamera;
    [Tooltip("The distance the menu will appear from the player when opened.")]
    public float distanceFromPlayer = 1.5f;

    [Header("Input Action")]
    [Tooltip("The Input Action Reference for the menu button (e.g., XRI LeftHand/Menu).")]
    public InputActionReference menuButtonAction;

    void Start()
    {
        // Ensure the menu is hidden when the scene begins and this script is active.
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false);
        }

        // Auto-find the camera if it hasn't been assigned.
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void OnEnable()
    {
        if (menuButtonAction != null)
        {
            menuButtonAction.action.performed += ToggleMenu;
        }
    }

    private void OnDisable()
    {
        if (menuButtonAction != null)
        {
            menuButtonAction.action.performed -= ToggleMenu;
        }
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        if (menuCanvas == null || mainCamera == null)
        {
            Debug.LogError("Menu Canvas or Main Camera not assigned.");
            return;
        }
        
        bool shouldBeActive = !menuCanvas.activeSelf;
        
        if (shouldBeActive)
        {
            // --- NEW LOGIC: Position and rotate the menu before showing it ---

            // 1. Get the camera's forward direction, but ignore the up/down tilt.
            Vector3 forward = mainCamera.transform.forward;
            forward.y = 0; // Flatten the vector to be parallel to the ground
            
            // 2. Calculate the position in front of the camera
            Vector3 position = mainCamera.transform.position + forward.normalized * distanceFromPlayer;
            
            // 3. Set the menu's position
            menuCanvas.transform.position = position;

            // 4. Rotate the menu to face the user
            menuCanvas.transform.rotation = Quaternion.LookRotation(menuCanvas.transform.position - mainCamera.transform.position);
        }

        // Toggle the menu's visibility
        menuCanvas.SetActive(shouldBeActive);
        Debug.Log("Menu Toggled: " + (shouldBeActive ? "ON" : "OFF"));
    }
}