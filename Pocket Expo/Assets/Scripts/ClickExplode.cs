using UnityEngine;

public class ClickExplode : MonoBehaviour
{
    [Header("Effect Settings")]
    public ParticleSystem explosionEffect; // Assign a particle system prefab
    public AudioClip explosionSound;       // Assign a sound effect

    private AudioSource audioSource;
    private bool hasExploded = false;

    void Start()
    {
        // Create and configure audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnMouseDown()
    {
        if (hasExploded) return; // Prevent multiple triggers
        hasExploded = true;

        // Play explosion sound
        if (explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // Spawn particle system and assign material from the object
        if (explosionEffect != null)
        {
            ParticleSystem ps = Instantiate(explosionEffect, transform.position, Quaternion.identity);

            // Copy this object's material to the particle system
            Renderer objRenderer = GetComponent<Renderer>();
            if (objRenderer != null)
            {
                Material originalMaterial = objRenderer.material;
                ParticleSystemRenderer psRenderer = ps.GetComponent<ParticleSystemRenderer>();
                if (psRenderer != null)
                {
                    psRenderer.material = originalMaterial;
                }
            }

            ps.Play();
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax); // Cleanup
        }

        // Hide or destroy the object after explosion
        Destroy(gameObject); // Or use gameObject.SetActive(false);
    }
}
