using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WalkingCameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent camera from tipping over on collision
    }

    void Update()
    {
        // Get input
        float moveInput = Input.GetAxis("Vertical");
        float rotateInput = Input.GetAxis("Horizontal");

        // Rotate camera left/right
        transform.Rotate(Vector3.up * rotateInput * rotationSpeed * Time.deltaTime);

        // Move camera forward/backward using physics
        Vector3 moveDirection = transform.forward * moveInput * moveSpeed;
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
    }
}
