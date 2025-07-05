// SceneContentToggler.cs

using UnityEngine;
using System.Collections.Generic;

public class SceneContentToggler : MonoBehaviour
{
    [Header("Content to Toggle")]
    [Tooltip("A list of all the root GameObjects you want to show when the button is clicked.")]
    public List<GameObject> contentObjects;

    [Header("Start Button")]
    [Tooltip("The button that triggers the content to appear. It will be hidden after being clicked.")]
    public GameObject startButton;

    void Start()
    {
        // Ensure all content is hidden at the start of the scene.
        foreach (var contentObject in contentObjects)
        {
            contentObject.SetActive(false);
        }

        // Make sure the start button is visible.
        if (startButton != null)
        {
            startButton.SetActive(true);
        }
    }

    /// <summary>
    /// This public method will be called by the start button's OnClick() event.
    /// </summary>
    public void ShowContent()
    {
        // Show all the content objects.
        foreach (var contentObject in contentObjects)
        {
            contentObject.SetActive(true);
        }

        // Hide the start button after it has been used.
        if (startButton != null)
        {
            startButton.SetActive(false);
        }
    }
}