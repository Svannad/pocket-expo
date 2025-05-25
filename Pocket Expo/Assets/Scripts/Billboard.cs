using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (cam != null)
            transform.LookAt(transform.position + cam.transform.forward);
    }
}
// This script makes the GameObject always face the camera, creating a billboard effect.