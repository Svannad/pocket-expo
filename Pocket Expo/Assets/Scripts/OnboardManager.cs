using UnityEngine;
using UnityEngine.UI;
using TMPro; // only if you use TextMeshPro
using System.Collections;

public class OnboardingManager : MonoBehaviour
{
    public OnboardScript onboardScript;
    public Text dialogueText; // Or just use UnityEngine.UI.Text
    private int currentLine = 0;
    private bool isTyping = false;

    void Start()
    {
        StartCoroutine(PlayLine());
    }

    IEnumerator PlayLine()
    {
        while (currentLine < onboardScript.scriptLines.Length)
        {
            isTyping = true;
            dialogueText.text = "";
            string line = onboardScript.scriptLines[currentLine];

            foreach (char letter in line.ToCharArray())
            {
                dialogueText.text += letter;

                if (onboardScript.typingSound)
                    AudioSource.PlayClipAtPoint(onboardScript.typingSound, Camera.main.transform.position);

                yield return new WaitForSeconds(onboardScript.typingSpeed);
            }

            isTyping = false;

            if (onboardScript.autoProgressLines.Length > currentLine && onboardScript.autoProgressLines[currentLine])
            {
                yield return new WaitForSeconds(onboardScript.autoProgressDelay);
                currentLine++;
            }
            else
            {
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                currentLine++;
            }
        }

        EndOnboarding();
    }

    void EndOnboarding()
    {
        // Hide the dialogue UI or trigger the next part of the game
        dialogueText.transform.parent.gameObject.SetActive(false);
    }
}
