using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenLoader : MonoBehaviour
{
    // This function can be called from the Button's OnClick event
    public void LoadStartScene()
    {
        SceneManager.LoadScene("Start Scene");
    }
}
