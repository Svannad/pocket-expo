using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverClickEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Scale Settings")]
    public float hoverScale = 1.1f;
    public float pressedScale = 0.95f;
    public float scaleSpeed = 8f;

    [Header("Sound Effects")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private AudioSource audioSource;
    private bool isPointerOver = false;
    private bool isPressed = false;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        // Smoothly scale towards target
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        if (!isPressed)
        {
            targetScale = originalScale * hoverScale;
        }

        if (hoverSound)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        if (!isPressed)
        {
            targetScale = originalScale;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        targetScale = originalScale * pressedScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;

        if (clickSound)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // Return to hover state if still hovered
        targetScale = isPointerOver ? originalScale * hoverScale : originalScale;
    }
}
