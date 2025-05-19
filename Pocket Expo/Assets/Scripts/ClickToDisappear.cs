using UnityEngine;
using System.Collections;

public class ClickToDisappear : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private AudioClip disappearSound;
    [Range(0, 1)] [SerializeField] private float volume = 1f;

    [Header("Disappear Settings")]
    [SerializeField] private bool destroyAfterDisappearing = true;
    [SerializeField] private float destroyDelay = 1.5f;

    [Header("Animation Settings")]
    [SerializeField] private float bloatAmount = 1.2f;
    [SerializeField] private float bloatDuration = 0.2f;
    [SerializeField] private float shrinkDuration = 0.3f;
    [SerializeField] private AnimationCurve bloatCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve shrinkCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    [Header("Particle Settings")]
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private bool matchParticleColor = true;

    private AudioSource audioSource;
    private Renderer objectRenderer;
    private Collider objectCollider;
    private bool isVisible = true;
    private Vector3 originalScale;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider>();
        originalScale = transform.localScale;
    }

    private void OnMouseDown()
    {
        if (!isVisible) return;
        StartCoroutine(DisappearSequence());
    }

    private IEnumerator DisappearSequence()
    {
        isVisible = false;

        // Bloat phase
        yield return StartCoroutine(ScaleObject(originalScale, originalScale * bloatAmount, bloatDuration, bloatCurve));

        // Play effects at peak size
        PlayDisappearEffects();

        // Shrink phase
        yield return StartCoroutine(ScaleObject(transform.localScale, Vector3.zero, shrinkDuration, shrinkCurve));

        // Final cleanup
        if (destroyAfterDisappearing)
        {
            Destroy(gameObject, destroyDelay);
        }
        else
        {
            if (objectRenderer != null) objectRenderer.enabled = false;
            if (objectCollider != null) objectCollider.enabled = false;
        }
    }

    private IEnumerator ScaleObject(Vector3 startScale, Vector3 endScale, float duration, AnimationCurve curve)
    {
        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, curve.Evaluate(t));
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = endScale;
    }

    private void PlayDisappearEffects()
    {
        // Play sound
        if (disappearSound != null)
        {
            audioSource.PlayOneShot(disappearSound, volume);
        }

        // Spawn particles
        if (particlePrefab != null)
        {
            GameObject particles = Instantiate(particlePrefab, transform.position, Quaternion.identity);

            var ps = particles.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var main = ps.main;

                if (matchParticleColor && objectRenderer != null)
                {
                    main.startColor = objectRenderer.material.color;
                }

                Destroy(particles, main.duration + main.startLifetime.constant);
            }
        }

        Debug.Log("Spawning particles at: " + transform.position);
    }

    public void MakeDisappear()
    {
        if (isVisible)
        {
            StartCoroutine(DisappearSequence());
        }
    }

   private void OnDisable()
{
    transform.localScale = originalScale;
    Debug.Log(name + " DISABLED at " + Time.time);
}

private void OnEnable()
{
    Debug.Log(name + " ENABLED at " + Time.time);
}


}
