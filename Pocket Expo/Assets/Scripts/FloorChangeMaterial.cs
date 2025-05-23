using UnityEngine;
using UnityEngine.EventSystems;

public class FloorChangeMaterial : MonoBehaviour
{
    [Header("Floor Settings")]
    public GameObject floor;  // Reference to the Floor GameObject
    public Material floorNewMaterial;  // The new material for the floor
    private Material floorOriginalMaterial;  // Store the original material of the floor

    [Header("Audio Settings")]
    public AudioClip soundEffect;  // The sound to play when the material changes
    private AudioSource audioSource;  // AudioSource to play the sound

    private bool materialsChanged = false;  // Flag to ensure materials are only changed once

    void Start()
    {
        // Set up the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Store the original material of the floor
        if (floor != null)
        {
            Renderer floorRenderer = floor.GetComponent<Renderer>();
            if (floorRenderer != null)
            {
                floorOriginalMaterial = floorRenderer.material;
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
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == floor.transform)
                {
                    ChangeMaterials(); // Now handles the sound as well
                }
            }
            else
            {
                Debug.Log("Raycast did NOT hit anything.");
            }
        }
    }

    void ChangeMaterials()
    {
        if (materialsChanged) return;

        if (floorNewMaterial != null && floorOriginalMaterial != floorNewMaterial)
        {
            Debug.Log("Floor material needs to be changed.");
        }
        else
        {
            Debug.Log("Floor material does not need to be changed.");
        }

        if (floor != null)
        {
            Renderer floorRenderer = floor.GetComponent<Renderer>();
            if (floorRenderer != null && floorNewMaterial != null && floorOriginalMaterial != floorNewMaterial)
            {
                floorRenderer.material = floorNewMaterial;
                Debug.Log("Floor material changed successfully.");

                // Only play sound once, at the same time as changing material
                PlaySound();

                materialsChanged = true;
            }
            else
            {
                Debug.LogWarning("Floor renderer or new material not found!");
            }
        }
    }


    void PlaySound()
    {
        if (soundEffect != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundEffect);
        }
    }
}
