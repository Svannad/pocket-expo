using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraMovement : MonoBehaviour
{
    public CameraSpotAlignment[] cameraSpotAlignments;
    public Volume GlobalVolume;
    public AudioSource transitionAudioSource;
    public InventoryManager inventoryManager;
    public TopDownUIController topDownUIController;

    private DepthOfField blurEffect;
    private Camera mainCamera;
    private float currentLerpTime = 0f;
    private float lerpDuration = 1f;
    private Vector3 startPosition;
    private float startFOV;
    private CameraSpot targetSpot;
    private bool isTransitioning = false;
    private Quaternion startRotation;
    private StationaryLookAround lookScript;
    private int currentCameraIndex = 0;

    public int CurrentCameraIndex => currentCameraIndex;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        lookScript = GetComponent<StationaryLookAround>();

        if (GlobalVolume != null && GlobalVolume.profile != null)
        {
            if (GlobalVolume.profile.TryGet(out blurEffect))
            {
                blurEffect.active = false;
                blurEffect.gaussianMaxRadius.value = 0f;
            }
            else
            {
                Debug.LogWarning("DepthOfField override not found in the PostProcess Volume profile!");
            }
        }
        else
        {
            Debug.LogWarning("PostProcess Volume or Profile not assigned!");
        }

        if (transitionAudioSource == null)
        {
            Debug.LogWarning("AudioSource for transitions not assigned!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            TransitionToSpot(cameraSpotAlignments[0]);
            UpdateInventoryVisibility(0);

            if (topDownUIController != null)
            {
                topDownUIController.ForceShowButtons();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) { TransitionToSpot(cameraSpotAlignments[1]); UpdateInventoryVisibility(1); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { TransitionToSpot(cameraSpotAlignments[2]); UpdateInventoryVisibility(2); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { TransitionToSpot(cameraSpotAlignments[3]); UpdateInventoryVisibility(3); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { TransitionToSpot(cameraSpotAlignments[4]); UpdateInventoryVisibility(4); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { TransitionToSpot(cameraSpotAlignments[5]); UpdateInventoryVisibility(5); }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (blurEffect != null)
            {
                blurEffect.active = true;
                blurEffect.gaussianMaxRadius.value = Mathf.Lerp(0f, 10f, Mathf.Sin(currentLerpTime / lerpDuration * Mathf.PI));
            }
        }

        if (isTransitioning)
        {
            currentLerpTime += Time.deltaTime;
            float percentComplete = currentLerpTime / lerpDuration;
            float smoothPercentage = Mathf.SmoothStep(0f, 1f, percentComplete);

            transform.position = Vector3.Lerp(startPosition, targetSpot.position, smoothPercentage);
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetSpot.fieldOfView, smoothPercentage);
            transform.rotation = Quaternion.Slerp(startRotation, targetSpot.rotation, smoothPercentage);

            if (blurEffect != null)
            {
                blurEffect.mode.value = DepthOfFieldMode.Gaussian;
                blurEffect.gaussianStart.value = 0.5f;
                blurEffect.gaussianEnd.value = 5f;
                blurEffect.gaussianMaxRadius.value = Mathf.Lerp(0f, 60f, Mathf.Sin(smoothPercentage * Mathf.PI));
                blurEffect.active = true;
            }

            if (percentComplete >= 1f)
            {
                isTransitioning = false;

                if (lookScript != null)
                {
                    lookScript.SyncToCurrentTransformRotation();
                    lookScript.enabled = true;
                }

                if (blurEffect != null)
                {
                    blurEffect.active = false;
                    blurEffect.gaussianMaxRadius.value = 0f;
                }

                if (topDownUIController != null)
                {
                    topDownUIController.RefreshVisibility();
                }
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

        if (lookScript != null)
            lookScript.enabled = false;

        if (transitionAudioSource != null && transitionAudioSource.clip != null)
        {
            transitionAudioSource.Play();
        }
    }

    void UpdateInventoryVisibility(int cameraIndex)
    {
        currentCameraIndex = cameraIndex;
        if (inventoryManager != null)
        {
            bool shouldShow = cameraIndex >= 1 && cameraIndex <= 5;
            inventoryManager.SetInventoryVisibility(shouldShow);
        }
    }

    // ✅ Uses static position snapshot from CameraSpotAlignment[0]
    public bool HasReachedSpot()
    {
        if (isTransitioning || currentCameraIndex != 0)
            return false;

        var refSpot = cameraSpotAlignments[0];
        float dist = Vector3.Distance(transform.position, refSpot.staticPosition);
        float angle = Quaternion.Angle(transform.rotation, refSpot.staticRotation);

        return dist < 0.1f && angle < 1f;
    }
}

