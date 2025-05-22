using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject settingsPanel;
    public GameObject gameManualPanel;
    public Button settingsToggleButton;
    public Button showManualButton;
    public Button closeManualButton;

    private bool hasShownManual = false;

    void Start()
    {
        // Hide settings panel initially
        settingsPanel.transform.localScale = Vector3.zero;
        settingsPanel.SetActive(false);

        // Show manual on first start with pop animation
        if (!hasShownManual)
        {
            ShowManualWithPop();
            hasShownManual = true;
        }
        else
        {
            gameManualPanel.SetActive(false);
        }

        // Hook up button events
        settingsToggleButton.onClick.AddListener(ToggleSettingsPanel);
        showManualButton.onClick.AddListener(ShowManualWithPop);
        closeManualButton.onClick.AddListener(CloseManual);
    }

    void ToggleSettingsPanel()
    {
        if (settingsPanel.activeSelf)
        {
            // Animate close
            settingsPanel.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() => {
                settingsPanel.SetActive(false);
            });
        }
        else
        {
            // Open with scale-in animation
            settingsPanel.SetActive(true);
            settingsPanel.transform.localScale = Vector3.zero;
            settingsPanel.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
        }
    }

    void ShowManualWithPop()
    {
        gameManualPanel.SetActive(true);
        gameManualPanel.transform.localScale = Vector3.zero;
        gameManualPanel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
    }

    void CloseManual()
    {
        gameManualPanel.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() => {
            gameManualPanel.SetActive(false);
        });
    }
}
