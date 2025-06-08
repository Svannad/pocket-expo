using UnityEngine;

public class TopDownUIController : MonoBehaviour
{
    public GameObject roomButtonsUI;
    public CameraMovement cameraMovement;

    public void RefreshVisibility()
    {
        roomButtonsUI.SetActive(cameraMovement.HasReachedSpot());
    }

    public void ForceShowButtons()
    {
        RefreshVisibility();
    }
}

