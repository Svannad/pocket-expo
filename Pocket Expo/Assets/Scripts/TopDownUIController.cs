using UnityEngine;
using System.Collections;

public class TopDownUIController : MonoBehaviour
{
    public GameObject roomButtonsUI;
    public CanvasGroup canvasGroup;
    public CameraMovement cameraMovement;
    public float fadeInDuration = 0.12f;

    private Coroutine transitionCoroutine;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = roomButtonsUI.GetComponent<RectTransform>();
        canvasGroup.alpha = 0;
        rectTransform.localScale = Vector3.zero;
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

        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        if (shouldBeVisible)
        {
            transitionCoroutine = StartCoroutine(FadeIn());
        }
        else
        {
            FadeOutInstant();
        }
    }

    public void ForceShowButtons()
    {
        RefreshVisibility();
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        float startAlpha = canvasGroup.alpha;
        Vector3 startScale = rectTransform.localScale;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeInDuration;
            float eased = EaseInOut(t);

            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, t);
            rectTransform.localScale = Vector3.Lerp(startScale, Vector3.one, eased);

            yield return null;
        }

        canvasGroup.alpha = 1f;
        rectTransform.localScale = Vector3.one;
    }

    private void FadeOutInstant()
    {
        canvasGroup.alpha = 0f;
        rectTransform.localScale = Vector3.zero;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    private float EaseInOut(float t)
    {
        return t < 0.5f
            ? 2f * t * t
            : -1f + (4f - 2f * t) * t;
    }
}




