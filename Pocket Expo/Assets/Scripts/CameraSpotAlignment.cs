using UnityEngine;

public class CameraSpotAlignment : MonoBehaviour
{
    public float fieldOfView;

    private CameraSpot cameraSpot;

    // 👇 These hold the initial snapshot at Awake time
    [HideInInspector] public Vector3 staticPosition;
    [HideInInspector] public Quaternion staticRotation;

    void Awake()
    {
        // Save transform at startup
        staticPosition = transform.position;
        staticRotation = transform.rotation;

        cameraSpot = new CameraSpot()
        {
            position = staticPosition,
            rotation = staticRotation,
            fieldOfView = fieldOfView
        };
    }

    public CameraSpot GetCameraSpot()
    {
        return cameraSpot;
    }
}

[System.Serializable]
public class CameraSpot
{
    public Vector3 position;
    public Quaternion rotation;
    public float fieldOfView;
}
