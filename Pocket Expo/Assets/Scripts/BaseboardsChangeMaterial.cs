using UnityEngine;

public class BaseboardsChangeMaterial : MonoBehaviour
{
    [Header("Baseboards Settings")]
    public GameObject baseboards;  // Reference to the Cube GameObject

    public Material baseboardsNewMaterial;  // The new material for the cube
    private Material baseboardsOriginalMaterial;  // Store the original material of the cube

    [Header("Audio Settings")]
    public AudioClip soundEffect;  // The sound to play when the material changes
    private AudioSource audioSource;  // AudioSource to play the sound

    private bool materialsChanged = false;  // Flag to ensure materials are only changed once

    void Start()
    {
        // Set up the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;  // Prevent the audio from playing automatically

        // Store the original material of the cube
        if (baseboards != null)
        {
            Renderer baseboardsRenderer = baseboards.GetComponent<Renderer>();  // Get the baseboards' renderer

            if (baseboardsRenderer != null)
            {
                baseboardsOriginalMaterial = baseboardsRenderer.sharedMaterial;  // Save the original baseboards material
                Debug.Log("Original baseboards material: " + baseboardsOriginalMaterial.name);
            }
            else
            {
                Debug.LogWarning("Baseboards Renderer not found!");
            }
        }
        else
        {
            Debug.LogWarning("Baseboards not assigned in the inspector!");
        }
    }

    void Update()
    {
        // Check for a left-click (mouse button down)
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // Create a ray from the mouse position
            RaycastHit hit;

            // Cast the ray and check if it hits any colliders in the scene
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycast hit: " + hit.transform.name);  // Log the name of the object hit by the ray

                // Check if the ray hit the object this script is attached to
                if (hit.transform == baseboards.transform)
                {
                    ChangeMaterials();  // Change the materials of the cube
                    PlaySound();  // Play the sound effect
                }
            }
        }
    }

    void ChangeMaterials()
    {
        // Prevent repeated material changes
        if (materialsChanged) return;

        // Debugging: Check if the material needs to be changed
        if (baseboardsNewMaterial != null && baseboardsOriginalMaterial != baseboardsNewMaterial)
        {
            Debug.Log("Baseboards material needs to be changed.");
        }
        else
        {
            Debug.Log("Baseboards material does not need to be changed.");
        }

        // Change the material of the cube
        if (baseboards != null)
        {
            Renderer baseboardsRenderer = baseboards.GetComponent<Renderer>();  // Get the cube's renderer

            if (baseboardsRenderer != null && baseboardsNewMaterial != null && baseboardsOriginalMaterial != baseboardsNewMaterial)
            {
                baseboardsRenderer.material = baseboardsNewMaterial;  // Set the new cube material
            }
            else
            {
                Debug.LogWarning("Baseboards renderer or new material not found!");
            }
        }

        materialsChanged = true;  // Set the flag to prevent repeated changes
    }

    void PlaySound()
    {
        // Play the magic wave sound if it's set up
        if (soundEffect != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundEffect);  // Play the sound once
        }
    }
}

