using UnityEngine;

public class CameraSpotAlignment : MonoBehaviour

{
    public float fieldOfView;

    private CameraSpot cameraSpot;

    void Awake() {
        cameraSpot = new CameraSpot() {
            position = transform.position,
            rotation = transform.rotation,
            fieldOfView = fieldOfView
        };
    }

    public CameraSpot GetCameraSpot() {
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
