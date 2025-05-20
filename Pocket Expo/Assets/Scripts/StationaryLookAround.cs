using UnityEngine;

public class StationaryLookAround : MonoBehaviour
{
    // This script allows the camera to look around in a stationary position
    public float rotationSpeed = 30f;
    public float pitchSpeed = 30f;
    public float maxPitch = 90f;
    public float minPitch = -90f;
    public float smoothTime = 0.2f;

    private float targetYaw;
    private float targetPitch;
    private float currentYaw;
    private float currentPitch;
    private float yawVelocity;
    private float pitchVelocity;

    private float startYaw;
    private float startPitch;
    private float startRoll;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;

        // Normalize angles
        startYaw = currentYaw = targetYaw = angles.y;
        startPitch = currentPitch = targetPitch = NormalizePitch(angles.x);
        startRoll = angles.z;
    }
    public void SyncToCurrentTransformRotation()
    {
        Vector3 angles = transform.eulerAngles;

        targetYaw = currentYaw = angles.y;
        targetPitch = currentPitch = NormalizePitch(angles.x);
        startRoll = angles.z; 
    }

    void Update()
    {
        // Input for yaw (left/right)
        if (Input.GetKey(KeyCode.LeftArrow))
            targetYaw -= rotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow))
            targetYaw += rotationSpeed * Time.deltaTime;

        // Input for pitch (up/down)
        if (Input.GetKey(KeyCode.UpArrow))
            targetPitch -= pitchSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow))
            targetPitch += pitchSpeed * Time.deltaTime;

        // Clamp pitch
        targetPitch = Mathf.Clamp(targetPitch, minPitch, maxPitch);

        // Smooth transition
        currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawVelocity, smoothTime);
        currentPitch = Mathf.SmoothDampAngle(currentPitch, targetPitch, ref pitchVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(currentPitch, currentYaw, startRoll);

        // Reset to start rotation
        if (Input.GetKeyDown(KeyCode.R))
        {
            targetYaw = startYaw;
            targetPitch = startPitch;
        }
    }
    float NormalizePitch(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return Mathf.Clamp(angle, minPitch, maxPitch);
    }
}


