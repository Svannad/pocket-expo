using UnityEngine;

public class TopDownUIController : MonoBehaviour
{
    public GameObject roomButtonsUI; // Assign RoomButtonsUI in Inspector
    public CameraMovement cameraMovement; // Assign the Camera0 script here

    void Update()
    {
        // Show only when in top-down camera (index 0)
        roomButtonsUI.SetActive(cameraMovement.CurrentCameraIndex == 0);
    }
}

