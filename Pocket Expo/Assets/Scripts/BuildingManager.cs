using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject pendingObject;
    private Vector3 pos;
    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;
    public float rotateAmount;
    public float scaleStep = 0.1f;
    public float minScale = 0.3f;
    public float maxScale = 3f;
    public bool canPlace;

    void Update()
    {
        if (pendingObject != null)
        {
            pendingObject.transform.position = pos;

            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                PlaceObject();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateObject();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ScaleObject(scaleStep);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ScaleObject(-scaleStep);
            }
        }
    }

    public void PlaceObject()
    {
        pendingObject = null;
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            pos = hit.point;
        }
    }

    public void SelectingObject(int index)
    {
        pendingObject = Instantiate(objects[index], pos, transform.rotation);
        pendingObject.transform.localScale = Vector3.one;
    }

    public void RotateObject()
    {
        pendingObject.transform.Rotate(Vector3.up, rotateAmount);
    }
    private void ScaleObject(float amount)
    {
        Vector3 currentScale = pendingObject.transform.localScale;
        float newScale = Mathf.Clamp(currentScale.x + amount, minScale, maxScale);
        pendingObject.transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
