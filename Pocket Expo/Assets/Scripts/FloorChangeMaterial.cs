using UnityEngine;
using UnityEngine.EventSystems;

public class FloorChangeMaterial : MonoBehaviour
{
    [Header("Floor Settings")]
    public GameObject floor;
    public Material floorNewMaterial;
    private Material floorOriginalMaterial;

    [Header("Audio Settings")]
    public AudioClip soundEffect;
    private AudioSource audioSource;

    private bool materialsChanged = false;
    public OnboardingManager onboardingManager;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

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
                    ChangeMaterials();

                    // ✅ Safe to call here
                    onboardingManager?.NotifyFloorClicked();
                    onboardingManager?.NotifyFloorChanged();
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

        if (floor != null && floorNewMaterial != null)
        {
            Renderer floorRenderer = floor.GetComponent<Renderer>();
            if (floorRenderer != null && floorOriginalMaterial != floorNewMaterial)
            {
                floorRenderer.material = floorNewMaterial;
                PlaySound();
                materialsChanged = true;

                Debug.Log("Floor material changed successfully.");
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
