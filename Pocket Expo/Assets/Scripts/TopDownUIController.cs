using UnityEngine;
using System.Collections;

public class TopDownUIController : MonoBehaviour
{
    public GameObject roomButtonsUI;
    public CanvasGroup canvasGroup;
    public CameraMovement cameraMovement;
    public float fadeDuration = 0.15f;
    public float bounceScale = 1.1f;

    private Coroutine fadeCoroutine;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = roomButtonsUI.GetComponent<RectTransform>();
        RefreshVisibility();
    }

    public void RefreshVisibility()
    {
        if (roomButtonsUI == null || cameraMovement == null || canvasGroup == null)
        {
            Debug.LogWarning("[TopDownUI] Missing references.");
            return;
        }

        bool shouldBeVisible = cameraMovement.HasReachedSpot();
        Debug.Log("[TopDownUI] Camera at top-down position: " + shouldBeVisible);

        // Always keep the object active; only control visibility through alpha and scale
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeWithBounce(shouldBeVisible));
    }

    public void ForceShowButtons()
    {
        RefreshVisibility();
    }

    private IEnumerator FadeWithBounce(bool visible)
    {
        float startAlpha = canvasGroup.alpha;
        float endAlpha = visible ? 1f : 0f;
        float timer = 0f;

        Vector3 startScale = rectTransform.localScale;
        Vector3 targetScale = visible ? Vector3.one * bounceScale : Vector3.zero;

        if (visible)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            float easedT = EaseOutBack(t);

            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, easedT);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        rectTransform.localScale = visible ? Vector3.one : Vector3.zero;

        if (!visible)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }

    // Simple bounce easing
    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;
        return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    }
}




