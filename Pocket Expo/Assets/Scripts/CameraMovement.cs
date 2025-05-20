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
    private StationaryLookAround lookScript;


    void Start()
    {
        mainCamera = GetComponent<Camera>();
        lookScript = GetComponent<StationaryLookAround>();
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
            float smoothPercentage = Mathf.SmoothStep(0f, 1f, percentComplete);

            // Interpolate position and FOV
            transform.position = Vector3.Lerp(startPosition, targetSpot.position, smoothPercentage);
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetSpot.fieldOfView, smoothPercentage);
            transform.rotation = Quaternion.Slerp(startRotation, targetSpot.rotation, smoothPercentage);

            if (percentComplete >= 1f)
            {
    isTransitioning = false;

    if (lookScript != null)
    {
        lookScript.SyncToCurrentTransformRotation();
        lookScript.enabled = true; // 👈 Re-enable after move
    }
        {
        isTransitioning = false;

        // Sync rotation with StationaryLookAround script
        StationaryLookAround lookScript = GetComponent<StationaryLookAround>();
        if (lookScript != null)
        {
            lookScript.SyncToCurrentTransformRotation();
        }
        else
        {
            // Ensure the camera is at the target position and FOV
            transform.position = targetSpot.position;
            mainCamera.fieldOfView = targetSpot.fieldOfView;
            transform.rotation = targetSpot.rotation;

        }
        }
        }
    }
        }
    // Transition to a specific camera spot
       public void TransitionToSpot(CameraSpotAlignment spotAlignment)
    {
        startPosition = transform.position;
        startFOV = mainCamera.fieldOfView;
        startRotation = transform.rotation;
        targetSpot = spotAlignment.GetCameraSpot();
        currentLerpTime = 0f;
        isTransitioning = true;

        // 👇 Temporarily disable the look script
        if (lookScript != null)
            lookScript.enabled = false;
    }

    }

