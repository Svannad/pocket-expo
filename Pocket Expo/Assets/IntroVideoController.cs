using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video; // For VideoPlayer

public class IntroVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "Start Scene"; // Set this to your actual scene name

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    // 📺 Call this from the Skip Button's OnClick
    public void SkipVideo()
    {
        videoPlayer.Stop(); // Optional: stop the video if needed
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}