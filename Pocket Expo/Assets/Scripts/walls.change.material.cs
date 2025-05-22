using UnityEngine;
using UnityEngine.EventSystems;

public class wallschange : MonoBehaviour
{
    [Header("Walls Settings")]
    public GameObject walls;  // Reference to the wall GameObject
    public Material wallsNewMaterial;  // The new material for the wall
    private Material wallsOriginalMaterial;  // Store the original material

    [Header("Audio Settings")]
    public AudioClip soundEffect;  // Sound to play when the material changes
    private AudioSource audioSource;

    private bool materialsChanged = false;

    void Start()
    {
        // Set up the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Store the original material
        if (walls != null)
        {
            Renderer wallsRenderer = walls.GetComponent<Renderer>();
            if (wallsRenderer != null)
            {
                wallsOriginalMaterial = wallsRenderer.sharedMaterial;
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
                if (hit.transform == walls.transform)
                {
                    ChangeMaterials(); // This now handles the sound too
                }
            }
        }
    }

    void ChangeMaterials()
    {
        if (materialsChanged) return;

        if (wallsNewMaterial != null && wallsOriginalMaterial != wallsNewMaterial)
        {
            Debug.Log("Wall material needs to be changed.");
        }
        else
        {
            Debug.Log("Wall material does not need to be changed.");
        }

        if (walls != null)
        {
            Renderer wallsRenderer = walls.GetComponent<Renderer>();
            if (wallsRenderer != null && wallsNewMaterial != null && wallsOriginalMaterial != wallsNewMaterial)
            {
                wallsRenderer.material = wallsNewMaterial;
                Debug.Log("Wall material changed successfully.");

                PlaySound(); // Only play once when material is changed

                materialsChanged = true;
            }
            else
            {
                Debug.LogWarning("Walls renderer or new material not found!");
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
