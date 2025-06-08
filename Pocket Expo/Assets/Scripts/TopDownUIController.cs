using UnityEngine;

public class TopDownUIController : MonoBehaviour
{
    public GameObject roomButtonsUI;
    public CameraMovement cameraMovement;

    void Start()
    {
        RefreshVisibility();
    }

    public void RefreshVisibility()
    {
        if (roomButtonsUI == null || cameraMovement == null)
        {
            Debug.LogWarning("[TopDownUI] Missing references to UI or CameraMovement.");
            return;
        }

        bool isAtTopDown = cameraMovement.HasReachedSpot();
        Debug.Log("[TopDownUI] Camera at top-down position: " + isAtTopDown);

        roomButtonsUI.SetActive(isAtTopDown);
    }

    public void ForceShowButtons()
    {
        RefreshVisibility();
    }
}



