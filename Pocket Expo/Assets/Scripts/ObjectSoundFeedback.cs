using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObjectSoundFeedback : MonoBehaviour
{
    [Header("Sound Effects")]
    public AudioClip selectSound;
    public AudioClip moveSound;
    public AudioClip deleteSound;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlaySelect() => PlayClip(selectSound);
    public void PlayMove() => PlayClip(moveSound);
    public void PlayDelete() => PlayClip(deleteSound);

    private void PlayClip(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
