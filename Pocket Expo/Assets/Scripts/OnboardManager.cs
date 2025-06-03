using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class OnboardingManager : MonoBehaviour
{
    public OnboardScript onboardScript;
    public Text dialogueText;
    private int currentLine = 0;
    private bool isTyping = false;
    private bool onboardingEnded = false;
    private bool wallChanged = false;
    private bool floorChanged = false;
    private bool wallClicked = false;
    private bool floorClicked = false;
    private bool feedbackGiven = false;
    private Coroutine currentTypingCoroutine;



    public void NotifyWallChanged()
    {
        wallChanged = true;
        CheckRoomInteractionDone();
    }

    public void NotifyFloorChanged()
    {
        floorChanged = true;
        CheckRoomInteractionDone();
    }

    void CheckRoomInteractionDone()
    {
        if (wallChanged && floorChanged && !feedbackGiven)
        {
            dialogueText.transform.parent.gameObject.SetActive(true);
            StartCoroutine(PlayRoomIntro());
        }
    }

    public void NotifyItemSelected()
    {
        Debug.Log("First item selected!");
        StartCoroutine(PlayPostItemDialogue());
    }

    IEnumerator PlayPostItemDialogue()
    {
        dialogueText.transform.parent.gameObject.SetActive(true);

        string[] lines = new string[]
        {
        "Good choice! You’ve got the eye. You can rotate any item by pressing R. And don’t worry! If something doesn’t feel quite right, you can always move it again… or remove it altogether.",
        "Ah, and if you ever want a closer look at your creation, just hold down the Z key to zoom in. Let go to return to your usual view — just like taking a step back to admire the bigger picture.",
        "And don’t forget, you can use the arrow keys to look around. It helps to take it all in",
        "This museum is your canvas now. Fill it with benches, paintings, history, life—whatever speaks to you. And if you ever forget how something works, just check the player manual under the settings button.",
        "Most of all… have fun. Your imagination is the only limit."
        };

        foreach (string line in lines)
        {
            yield return RunSingleTypingCoroutine(TypeLine(line));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        dialogueText.transform.parent.gameObject.SetActive(false);
    }



    void Start()
    {
        StartCoroutine(PlayLine());
    }

    void Update()
    {
        if (onboardingEnded)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) ||
                Input.GetKeyDown(KeyCode.Alpha2) ||
                Input.GetKeyDown(KeyCode.Alpha3) ||
                Input.GetKeyDown(KeyCode.Alpha4) ||
                Input.GetKeyDown(KeyCode.Alpha5))
            {
                dialogueText.transform.parent.gameObject.SetActive(true);
                StartCoroutine(PlayRoomIntro());
                onboardingEnded = false;
            }
        }
    }

    IEnumerator PlayLine()
    {
        while (currentLine < onboardScript.scriptLines.Length)
        {
            isTyping = true;
            dialogueText.text = "";
            string line = onboardScript.scriptLines[currentLine];

            AudioSource audioSource = null;
            if (onboardScript.typingSound)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = onboardScript.typingSound;
                audioSource.loop = false;
                audioSource.Play();
            }

            foreach (char letter in line.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(onboardScript.typingSpeed);
            }

            if (audioSource != null)
            {
                audioSource.Stop();
                Destroy(audioSource);
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

    Coroutine RunSingleTypingCoroutine(IEnumerator routine)
    {
        if (currentTypingCoroutine != null)
        {
            StopCoroutine(currentTypingCoroutine);
        }
        currentTypingCoroutine = StartCoroutine(routine);
        return currentTypingCoroutine;
    }


    IEnumerator PlayRoomIntro()
    {
        string roomIntro = "Ah, this room… a little rough around the edges, huh? Try clicking on the floors and walls. You’ll see that every room deserves a bit of love.";
        yield return RunSingleTypingCoroutine(TypeLine(roomIntro));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        dialogueText.transform.parent.gameObject.SetActive(false);
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        AudioSource audioSource = null;

        if (onboardScript.typingSound)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = onboardScript.typingSound;
            audioSource.loop = false;
            audioSource.Play();
        }

        for (int i = 0; i < line.Length; i++)
        {
            dialogueText.text += line[i];

            float elapsed = 0;
            while (elapsed < onboardScript.typingSpeed)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    dialogueText.text = line;
                    i = line.Length;
                    break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        if (audioSource != null)
        {
            audioSource.Stop();
            Destroy(audioSource);
        }
    }

    IEnumerator ShowPostClickSpeech()
    {
        dialogueText.transform.parent.gameObject.SetActive(true);
        string line = "Much better. Now it's ready for your vision. Start by picking your first item";
        yield return RunSingleTypingCoroutine(TypeLine(line));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        dialogueText.transform.parent.gameObject.SetActive(false);
    }

    void EndOnboarding()
    {
        dialogueText.transform.parent.gameObject.SetActive(false);
        onboardingEnded = true;
    }

    // 👇 These are called by the wall/floor scripts
    public void NotifyWallClicked()
    {
        wallClicked = true;
        CheckRoomInteractionComplete();
    }

    public void NotifyFloorClicked()
    {
        floorClicked = true;
        CheckRoomInteractionComplete();
    }

    void CheckRoomInteractionComplete()
    {
        if (!feedbackGiven && wallClicked && floorClicked)
        {
            feedbackGiven = true;
            StartCoroutine(ShowPostClickSpeech());
        }
    }
}
