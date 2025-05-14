using UnityEngine;

public class TestWithCube : MonoBehaviour
{
    [Header("Cube Settings")]
    public GameObject cube;  // Reference to the Cube GameObject
    public Material cubeNewMaterial;  // The new material for the cube
    private Material cubeOriginalMaterial;  // Store the original material of the cube

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
        if (cube != null)
        {
            Renderer cubeRenderer = cube.GetComponent<Renderer>();  // Find the cube's renderer
            if (cubeRenderer != null)
            {
                cubeOriginalMaterial = cubeRenderer.sharedMaterial;  // Save the original cube material
                Debug.Log("Original cube material: " + cubeOriginalMaterial.name);  // Log original cube material name
            }
            else
            {
                Debug.LogWarning("Cube Renderer not found!");
            }
        }
        else
        {
            Debug.LogWarning("Cube not assigned in the inspector!");
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
                if (hit.transform == cube.transform)
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
        if (cubeNewMaterial != null && cubeOriginalMaterial != cubeNewMaterial)
        {
            Debug.Log("Cube material needs to be changed.");
        }
        else
        {
            Debug.Log("Cube material does not need to be changed.");
        }

        // Change the material of the cube
        if (cube != null)
        {
            Renderer cubeRenderer = cube.GetComponent<Renderer>();  // Get the cube's renderer
            if (cubeRenderer != null && cubeNewMaterial != null && cubeOriginalMaterial != cubeNewMaterial)
            {
                cubeRenderer.material = cubeNewMaterial;  // Set the new cube material
            }
            else
            {
                Debug.LogWarning("Cube renderer or new material not found!");
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
