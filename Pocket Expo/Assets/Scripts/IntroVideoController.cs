using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class IntroVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "Start Scene";
    public Image fadeImage; // assign your FadeImage UI element
    public float fadeDuration = 1.5f;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        StartCoroutine(FadeInAndPlay());
    }

    IEnumerator FadeInAndPlay()
    {
        yield return StartCoroutine(Fade(1f, 0f)); // Fade from black to transparent
        videoPlayer.Play();
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
        yield return StartCoroutine(Fade(0f, 1f)); // Fade from transparent to black
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
// This script controls the intro video playback and fading effects.
// It uses Unity's VideoPlayer to play the video and an Image component for the fade effect.
// The video will fade in at the start, and when it finishes, it will fade out and load the next scene.
// The SkipVideo method allows the player to skip the video at any time, triggering the same fade-out effect.