// 1. MovableImageManager.cs
// This is the central brain. Place it on an empty GameObject.

using UnityEngine;
using System.Collections.Generic;

public class MovableImageManager : MonoBehaviour
{
    [Header("Position Marker")]
    [Tooltip("The single, shared location where images will move to.")]
    public Transform endMarker;

    [Header("Registered Images")]
    [Tooltip("The script will automatically find all movable images at start.")]
    public List<MovableImage> movableImages = new List<MovableImage>();

    // A reference to the image that is currently at the end position
    private MovableImage activeImage = null;

    void Start()
    {
        // Automatically find and register all MovableImage components in the scene
        movableImages.AddRange(FindObjectsOfType<MovableImage>());
        foreach (var image in movableImages)
        {
            image.RegisterManager(this);
        }
    }

    /// <summary>
    /// Called by a MovableImage when it is clicked.
    /// </summary>
    public void OnImageClicked(MovableImage clickedImage)
    {
        // If there's already an active image, tell it to go home.
        if (activeImage != null && activeImage != clickedImage)
        {
            activeImage.MoveToStart();
        }

        // If the clicked image is the currently active one, send it home.
        if (activeImage == clickedImage)
        {
            clickedImage.MoveToStart();
            activeImage = null; // No image is active now
        }
        // Otherwise, make the clicked image the new active one and move it to the end.
        else
        {
            clickedImage.MoveToEnd(endMarker);
            activeImage = clickedImage;
        }
    }
}