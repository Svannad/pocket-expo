using UnityEngine;

[CreateAssetMenu(fileName ="NewOnboardScript", menuName = "Onboard Script")]
public class OnboardScript : ScriptableObject
{
    public string[] scriptLines;
    public float typingSpeed = 0.05f;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;
    public AudioClip typingSound;
}
