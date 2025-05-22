using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{

    public VideoPlayer videoPlayer;

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }
}
