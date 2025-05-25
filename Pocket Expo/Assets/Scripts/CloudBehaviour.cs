using UnityEngine;

public class CloudBehavior : MonoBehaviour
{
    public Vector2 sizeRange = new Vector2(3f, 6f);
    public Vector2 driftSpeedRange = new Vector2(0.1f, 0.5f);
    public float fadeDuration = 2f;

    private float driftSpeed;
    private float alpha = 0f;
    private float timer = 0f;
    private Material mat;

    void Start()
    {
        // Randomize scale
        float randomSize = Random.Range(sizeRange.x, sizeRange.y);
        transform.localScale = new Vector3(randomSize, randomSize * 0.6f, 1f); // Keep a natural cloud shape

        // Random drift speed (along X axis)
        driftSpeed = Random.Range(driftSpeedRange.x, driftSpeedRange.y);

        // Clone the material (shared material would affect all clouds)
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        mat = renderer.material = new Material(renderer.material);
        SetAlpha(0f);
    }

    void Update()
    {
        // Drift cloud
        transform.position += new Vector3(driftSpeed * Time.deltaTime, 0f, 0f);

        // Fade in
        if (alpha < 1f)
        {
            timer += Time.deltaTime;
            alpha = Mathf.Clamp01(timer / fadeDuration);
            SetAlpha(alpha);
        }
    }

    void SetAlpha(float a)
    {
        if (mat != null && mat.HasProperty("_Color"))
        {
            Color color = mat.color;
            color.a = a;
            mat.color = color;
        }
    }
}
// This script controls the behavior of each cloud, including size, drift speed, and fading in over time.