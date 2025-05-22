using UnityEngine;
using UnityEngine.EventSystems;

public class baseboards_change_material : MonoBehaviour
{
    [Header("Baseboards Settings")]
    public GameObject baseboards;  // Reference to the baseboard GameObject
    public Material baseboardsNewMaterial;  // The new material
    private Material baseboardsOriginalMaterial;  // Store the original material

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
        if (baseboards != null)
        {
            Renderer baseboardsRenderer = baseboards.GetComponent<Renderer>();
            if (baseboardsRenderer != null)
            {
                baseboardsOriginalMaterial = baseboardsRenderer.sharedMaterial;
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
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == baseboards.transform)
                {
                    ChangeMaterials(); // Now includes sound logic
                }
            }
        }
    }

    void ChangeMaterials()
    {
        if (materialsChanged) return;

        if (baseboardsNewMaterial != null && baseboardsOriginalMaterial != baseboardsNewMaterial)
        {
            Debug.Log("Baseboards material needs to be changed.");
        }
        else
        {
            Debug.Log("Baseboards material does not need to be changed.");
        }

        if (baseboards != null)
        {
            Renderer baseboardsRenderer = baseboards.GetComponent<Renderer>();
            if (baseboardsRenderer != null && baseboardsNewMaterial != null && baseboardsOriginalMaterial != baseboardsNewMaterial)
            {
                baseboardsRenderer.material = baseboardsNewMaterial;
                Debug.Log("Baseboards material changed successfully.");

                PlaySound(); // ✅ Only play when the material is actually changed

                materialsChanged = true;
            }
            else
            {
                Debug.LogWarning("Baseboards renderer or new material not found!");
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
