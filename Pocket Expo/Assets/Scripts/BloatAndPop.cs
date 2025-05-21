using UnityEngine;

public class BloatAndPop : MonoBehaviour
{
    [Header("Bloat Settings")]
    public float bloatDuration = 0.2f;
    public float maxScale = 1.3f;
    public AnimationCurve bloatCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Shrink Before Pop")]
    public float shrinkScale = 0.6f;
    public float shrinkDuration = 0.2f;
    public AnimationCurve shrinkCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Pop Settings")]
    public ParticleSystem popEffect; // Will auto-assign if null
    public AudioClip popSound;       // Will auto-assign if null
    public float delayBeforePop = 0.1f;
    public bool destroyAfterPop = true;

    private AudioSource audioSource;
    private Vector3 originalScale;
    private bool isPopping = false;
    private bool isClicked = false;

    void Start()
    {
        originalScale = transform.localScale;

        // Get or create AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign default particle system if not assigned
        if (popEffect == null)
        {
            Transform psTransform = transform.Find("Particle System");
            if (psTransform != null)
            {
                popEffect = psTransform.GetComponent<ParticleSystem>();
            }
        }

        // Load default sound from Resources folder if not assigned
        if (popSound == null)
        {
            popSound = Resources.Load<AudioClip>("BloatAndPop");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isClicked)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
            {
                isClicked = true;
                StartCoroutine(BloatThenShrinkThenPop());
            }
        }
    }

    System.Collections.IEnumerator BloatThenShrinkThenPop()
    {
        float elapsed = 0f;

        // Bloat
        while (elapsed < bloatDuration)
        {
            float t = elapsed / bloatDuration;
            float scaleFactor = Mathf.Lerp(1f, maxScale, bloatCurve.Evaluate(t));
            transform.localScale = originalScale * scaleFactor;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale * maxScale;

        // Shrink
        elapsed = 0f;
        Vector3 bloatScale = originalScale * maxScale;
        Vector3 shrinkTarget = originalScale * shrinkScale;

        while (elapsed < shrinkDuration)
        {
            float t = elapsed / shrinkDuration;
            float curveValue = shrinkCurve.Evaluate(t);
            transform.localScale = Vector3.Lerp(bloatScale, shrinkTarget, curveValue);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = shrinkTarget;

        yield return new WaitForSeconds(delayBeforePop);

        Pop();
    }

    void Pop()
    {
        if (isPopping) return;
        isPopping = true;

        // Play particle effect
        if (popEffect)
        {
            popEffect.transform.SetParent(null);
            popEffect.Play();
            Destroy(popEffect.gameObject, popEffect.main.duration + popEffect.main.startLifetime.constantMax);
        }

        // Play sound
        if (popSound)
        {
            audioSource.PlayOneShot(popSound);
        }

        // Hide visuals
        foreach (var rend in GetComponentsInChildren<Renderer>())
            rend.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;

        if (destroyAfterPop)
        {
            Destroy(gameObject, popSound ? popSound.length : 0f);
        }
    }
}
