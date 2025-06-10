using UnityEngine;

public class RoomMarkerController : MonoBehaviour
{
    public GameObject markerGroup; // Reference to TopViewRoomMarkers
    public CameraMovement cameraMovement; // From Camera0

    void Update()
    {
        // Show markers only when viewing from top-down (Camera0)
        markerGroup.SetActive(cameraMovement.CurrentCameraIndex == 0);
    }
}

