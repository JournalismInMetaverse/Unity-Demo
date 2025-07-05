// SkyboxSwitcher.cs

using UnityEngine;
using System.Collections.Generic;

public class SkyboxSwitcher : MonoBehaviour
{
    [Header("Background Materials")]
    [Tooltip("A list of all the Skybox materials you want to cycle through.")]
    public List<Material> skyboxMaterials;

    // A counter to keep track of the current material index
    private int currentMaterialIndex = 0;

    void Start()
    {
        // Ensure we have materials to work with
        if (skyboxMaterials == null || skyboxMaterials.Count == 0)
        {
            Debug.LogError("Skybox Materials list is not set up!");
            this.enabled = false; // Disable the script to prevent errors
            return;
        }

        // Set the initial background from the list or use the scene's default
        if (RenderSettings.skybox != skyboxMaterials[currentMaterialIndex])
        {
             RenderSettings.skybox = skyboxMaterials[currentMaterialIndex];
        }
    }

    /// <summary>
    /// This public method cycles to the next material in the list.
    /// Hook this up to your UI button's OnClick() event.
    /// </summary>
    public void CycleSkybox()
    {
        if (skyboxMaterials.Count == 0) return;

        // Move to the next index, wrapping around to the beginning if we reach the end
        currentMaterialIndex = (currentMaterialIndex + 1) % skyboxMaterials.Count;

        // Apply the new material to the scene's skybox
        RenderSettings.skybox = skyboxMaterials[currentMaterialIndex];
    }
}
