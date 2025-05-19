using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    [SerializeField]
    private float zoomFOV;
    [SerializeField]
    private float normalFOV;
    [SerializeField]
    private float zoomSpeed;

    private Camera camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float targetFOV;

        if(Input.GetKey(KeyCode.C))
        {
            targetFOV = zoomFOV;
        }
        else
        {
            targetFOV = normalFOV;
        }

        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
}
