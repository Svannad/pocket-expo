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
    public InventoryManager inventoryManager;

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

    private void UpdatePlacementPosition()
    {
        if (pendingObject == null) return;

        Collider[] colliders = pendingObject.GetComponentsInChildren<Collider>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (pendingObject.CompareTag("WallOnly"))
        {
            if (Physics.Raycast(ray, out hitInfo, 1000f, LayerMask.GetMask("Wall")))
            {
                pos = hitInfo.point;
                pendingObject.transform.rotation = Quaternion.LookRotation(hitInfo.normal * -1f);
            }
        }
        else
        {
            if (Physics.Raycast(ray, out hitInfo, 1000f, LayerMask.GetMask("Ground", "Stackable")))
            {
                pos = hitInfo.point;
                pendingObject.transform.rotation = Quaternion.Euler(0, pendingObject.transform.rotation.eulerAngles.y, 0);
            }
        }

        pendingObject.transform.position = pos;

        foreach (var col in colliders)
        {
            col.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        UpdatePlacementPosition();
    }


    public void SelectingObject(int index)
    {
        pendingObject = Instantiate(objects[index], pos, transform.rotation);
        pendingObject.transform.localScale = Vector3.one;

        if (inventoryManager != null)
        {
            inventoryManager.CloseCurrentPage();
        }
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
