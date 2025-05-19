using UnityEngine;

public class StationaryLookAround : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public float pitchSpeed = 60f;
    public float maxPitch = 90f;
    public float minPitch = -90f;
    public float smoothTime = 0.1f;

    private float targetYaw;
    private float targetPitch;
    private float currentYaw;
    private float currentPitch;
    private float yawVelocity;
    private float pitchVelocity;

    private float startYaw;
    private float startPitch;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;

        // Normalize angles
        startYaw = currentYaw = targetYaw = angles.y;
        startPitch = currentPitch = targetPitch = NormalizePitch(angles.x);
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

        // Apply rotation
        transform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

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

