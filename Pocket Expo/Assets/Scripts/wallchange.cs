using UnityEngine;

public class wallschange : MonoBehaviour
{
     [Header("Walls Settings")]
    public GameObject walls;  // Reference to the Cube GameObject
    public Material wallsNewMaterial;  // The new material for the cube
    private Material wallsOriginalMaterial;  // Store the original material of the cube

    [Header("Audio Settings")]
    public AudioClip soundEffect;  // The sound to play when the material changes
    private AudioSource audioSource;  // AudioSource to play the sound

    private bool materialsChanged = false;  // Flag to ensure materials are only changed once
// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
  {
        // Set up the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;  // Prevent the audio from playing automatically

        // Store the original material of the cube
        if (walls != null)
       if (walls != null) 
{
    Renderer wallsRenderer = walls.GetComponent<Renderer>();  // Get the walls' renderer

    if (wallsRenderer != null)
    {
        wallsOriginalMaterial = wallsRenderer.sharedMaterial;  // Save the original walls material
        Debug.Log("Original walls material: " + wallsOriginalMaterial.name);
    }
    else
    {
        Debug.LogWarning("Walls Renderer not found!");
    }
}
else
{
    Debug.LogWarning("Walls not assigned in the inspector!");
}
    }

    // Update is called once per frame
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
                if (hit.transform == walls.transform)
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
        if (wallsNewMaterial != null && wallsOriginalMaterial != wallsNewMaterial)
        {
            Debug.Log("Wall material needs to be changed.");
        }
        else
        {
            Debug.Log("Wall material does not need to be changed.");
        }

        // Change the material of the cube
        if (walls != null)
        {
            Renderer wallsRenderer = walls.GetComponent<Renderer>();  // Get the cube's renderer
            if (wallsRenderer != null && wallsNewMaterial != null && wallsOriginalMaterial != wallsNewMaterial)
            {
                wallsRenderer.material = wallsNewMaterial;  // Set the new cube material
            }
            else
            {
                Debug.LogWarning("Walls renderer or new material not found!");
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
