using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class IntroVideoController : MonoBehaviour
{
    [Header("Video & Scene")]
    public VideoPlayer videoPlayer;
    public string nextSceneName = "Start Scene";

    [Header("Fade Overlay")]
    public Image fadeImage;
    public float fadeDuration = 1.5f;

    [Header("Skip Button")]
    public Button skipButton;
    public float skipButtonDelay = 3f;
    public float skipButtonFadeDuration = 1f;
    public float skipSlideDistance = 100f;

    private CanvasGroup skipCanvasGroup;
    private RectTransform skipRectTransform;
    private Vector2 skipOriginalPosition;

    void Start()
    {
        // Set up skip button references
        if (skipButton != null)
        {
            skipCanvasGroup = skipButton.GetComponent<CanvasGroup>();
            skipRectTransform = skipButton.GetComponent<RectTransform>();
            if (skipCanvasGroup != null && skipRectTransform != null)
            {
                skipOriginalPosition = skipRectTransform.anchoredPosition;
                skipCanvasGroup.alpha = 0f;
                skipCanvasGroup.interactable = false;
                skipCanvasGroup.blocksRaycasts = false;
                skipRectTransform.anchoredPosition += new Vector2(0, skipSlideDistance); // Start off-screen
            }
        }

        videoPlayer.loopPointReached += OnVideoFinished;

        StartCoroutine(FadeInAndPlay());
        StartCoroutine(ShowSkipButtonAfterDelay());
    }

    IEnumerator FadeInAndPlay()
    {
        yield return StartCoroutine(Fade(1f, 0f)); // Fade in
        videoPlayer.Play();
    }

    IEnumerator ShowSkipButtonAfterDelay()
    {
        yield return new WaitForSeconds(skipButtonDelay);

        if (skipCanvasGroup != null && skipRectTransform != null)
        {
            float elapsed = 0f;
            Vector2 startPos = skipRectTransform.anchoredPosition;
            Vector2 endPos = skipOriginalPosition;

            while (elapsed < skipButtonFadeDuration)
            {
                float t = elapsed / skipButtonFadeDuration;
                skipCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
                skipRectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            skipCanvasGroup.alpha = 1f;
            skipRectTransform.anchoredPosition = endPos;
            skipCanvasGroup.interactable = true;
            skipCanvasGroup.blocksRaycasts = true;
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(FadeOutAndLoad());
    }

    public void SkipVideo()
    {
        videoPlayer.Stop();
        StartCoroutine(FadeOutAndLoad());
    }

    IEnumerator FadeOutAndLoad()
    {
        yield return StartCoroutine(Fade(0f, 1f)); // Fade to black
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }
}