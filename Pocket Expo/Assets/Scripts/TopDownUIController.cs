using UnityEngine;

public class TopDownUIController : MonoBehaviour
{
    public GameObject roomButtonsUI;
    public CameraMovement cameraMovement;
    public Transform cameraTransform; // Assign Camera0’s Transform
    public float angleThreshold = 30f; // How "top-down" the view must be

    void Update()
    {
        bool isTopCamera = cameraMovement.CurrentCameraIndex == 0;
        bool isLookingDown = Vector3.Angle(cameraTransform.forward, Vector3.down) <= angleThreshold;

        roomButtonsUI.SetActive(isTopCamera && isLookingDown);
    }
}

