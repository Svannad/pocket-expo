using UnityEngine;

public class WallsMaterial : MonoBehaviour
{
    [Header("Walls Settings")]
    public GameObject wall;  // Reference to the Cube GameObject
    public Material wallNewMaterial;  // The new material for the cube
    private Material wallOriginalMaterial;  // Store the original material of the cube

    [Header("Audio Settings")]
    public AudioClip magicWaveSound;  // The sound to play when the material changes
    private AudioSource audioSource;  // AudioSource to play the sound

    private bool materialsChanged = false;  // Flag to ensure materials are only changed once

    void Start()
    {
        // Set up the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;  // Prevent the audio from playing automatically

        // Store the original material of the cube
        if (wall != null)
        {
            Renderer wallRenderer = wall.GetComponent<Renderer>();  // Find the cube's renderer
            if (wallRenderer != null)
            {
                wallOriginalMaterial = wallRenderer.sharedMaterial;  // Save the original cube material
                Debug.Log("Original wall material: " + wallOriginalMaterial.name);  // Log original cube material name
            }
            else
            {
                Debug.LogWarning("wall Renderer not found!");
            }
        }
        else
        {
            Debug.LogWarning("wall not assigned in the inspector!");
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
                if (hit.transform == wall.transform)
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
        if (wallNewMaterial != null && wallOriginalMaterial != wallNewMaterial)
        {
            Debug.Log("wall material needs to be changed.");
        }
        else
        {
            Debug.Log("wall material does not need to be changed.");
        }

        // Change the material of the cube
        if (wall != null)
        {
            Renderer wallRenderer = wall.GetComponent<Renderer>();  // Get the cube's renderer
            if (wallRenderer != null && wallNewMaterial != null && wallOriginalMaterial != wallNewMaterial)
            {
                wallRenderer.material = wallNewMaterial;  // Set the new cube material
            }
            else
            {
                Debug.LogWarning("wall renderer or new material not found!");
            }
        }

        materialsChanged = true;  // Set the flag to prevent repeated changes
    }

    void PlaySound()
    {
        // Play the magic wave sound if it's set up
        if (magicWaveSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(magicWaveSound);  // Play the sound once
        }
    }
}
