using UnityEngine;

public class FloorChangeMaterial : MonoBehaviour
{
    [Header("Floor Settings")]
    public GameObject floor;  // Reference to the Cube GameObject

    public Material floorNewMaterial;  // The new material for the cube
    private Material floorOriginalMaterial;  // Store the original material of the cube

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
        if (floor != null)
        {
            Renderer floorRenderer = floor.GetComponent<Renderer>();  // Get the floor's renderer

            if (floorRenderer != null)
            {
                floorOriginalMaterial = floorRenderer.material;  // Save the original floor material
                Debug.Log("Original floor material: " + floorOriginalMaterial.name);
            }
            else
            {
                Debug.LogWarning("Floor Renderer not found!");
            }
        }
        else
        {
            Debug.LogWarning("Floor not assigned in the inspector!");
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
                if (hit.transform == floor.transform)
                {
                    ChangeMaterials();  // Change the materials of the cube
                    PlaySound();  // Play the sound effect
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
    {
        Debug.Log("Mouse click detected.");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);

            if (hit.transform.gameObject == floor)
            {
                Debug.Log("Raycast hit the correct floor object!");
                ChangeMaterials();
                PlaySound();
            }
            else
            {
                Debug.Log("Raycast hit something else: " + hit.transform.name);
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }
    }
    }

   void ChangeMaterials()
{
    // Prevent repeated material changes
    if (materialsChanged) return;

    Debug.Log("floorOriginalMaterial: " + (floorOriginalMaterial != null ? floorOriginalMaterial.name : "null"));
    Debug.Log("floorNewMaterial: " + (floorNewMaterial != null ? floorNewMaterial.name : "null"));

    // Debugging: Check if the material needs to be changed
    if (floorNewMaterial != null && floorOriginalMaterial != floorNewMaterial)
    {
        Debug.Log("Floor material needs to be changed.");
    }
    else
    {
        Debug.Log("Floor material does not need to be changed.");
    }

    // Change the material of the cube
    if (floor != null)
    {
        Renderer floorRenderer = floor.GetComponent<Renderer>();  // Get the cube's renderer

        if (floorRenderer != null && floorNewMaterial != null && floorOriginalMaterial != floorNewMaterial)
        {
            floorRenderer.material = floorNewMaterial;  // Set the new cube material
            Debug.Log("Material successfully changed.");
        }
        else
        {
            Debug.LogWarning("Floor renderer or new material not found!");
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
