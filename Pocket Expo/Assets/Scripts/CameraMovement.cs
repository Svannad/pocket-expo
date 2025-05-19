using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public CameraSpotAlignment[] cameraSpotAlignments;

    private Camera mainCamera;
    private float currentLerpTime = 0f;
    private float lerpDuration = 1f;
    private Vector3 startPosition;
    private float startFOV;
    private CameraSpot targetSpot;
    private bool isTransitioning = false;
    private Quaternion startRotation;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            TransitionToSpot(cameraSpotAlignments[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TransitionToSpot(cameraSpotAlignments[1]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TransitionToSpot(cameraSpotAlignments[2]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TransitionToSpot(cameraSpotAlignments[3]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TransitionToSpot(cameraSpotAlignments[4]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TransitionToSpot(cameraSpotAlignments[5]);
        }
            
        if (isTransitioning)
        {
            currentLerpTime += Time.deltaTime;
            float percentComplete = currentLerpTime / lerpDuration;

            // Use smoothstep for smoother animation
            float smoothPercentage = percentComplete * percentComplete * (3f - 2f * percentComplete);

            // Interpolate position and FOV
            transform.position = Vector3.Lerp(startPosition, targetSpot.position, smoothPercentage);
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetSpot.fieldOfView, smoothPercentage);
            transform.rotation = Quaternion.Lerp(startRotation, targetSpot.rotation, smoothPercentage);

            if (percentComplete >= 1f)
            {
                isTransitioning = false;
            }
        }
    }

    public void TransitionToSpot(CameraSpotAlignment spotAlignment)
    {
        startPosition = transform.position;
        startFOV = mainCamera.fieldOfView;
        startRotation = transform.rotation;
        targetSpot = spotAlignment.GetCameraSpot();
        currentLerpTime = 0f;
        isTransitioning = true;
    }
}
