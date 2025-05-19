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
    public bool canPlace;
    public InventoryManager inventoryManager;
    private float currentYRotation = 0f;
    public Camera activeCamera;

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

        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (pendingObject.CompareTag("WallOnly"))
        {
            if (Physics.Raycast(ray, out hitInfo, 1000f, LayerMask.GetMask("Wall")))
            {
                pos = hitInfo.point;

                // Face the wall using surface normal
                Quaternion wallFacing = Quaternion.LookRotation(hitInfo.normal * -1f);

                // Use default rotation from prefab
                Quaternion originalRot = pendingObject.GetComponent<OriginalPrefabInfo>().defaultRotation;
                Vector3 originalEuler = originalRot.eulerAngles;
                Quaternion finalRot = Quaternion.Euler(originalEuler.x, wallFacing.eulerAngles.y, originalEuler.z);

                pendingObject.transform.rotation = finalRot;
            }
        }
        else
        {
            if (Physics.Raycast(ray, out hitInfo, 1000f, LayerMask.GetMask("Ground", "Stackable")))
            {
                pos = hitInfo.point;

                Quaternion baseRot = pendingObject.GetComponent<OriginalPrefabInfo>().defaultRotation;
                Quaternion finalRot = Quaternion.Euler(baseRot.eulerAngles.x, currentYRotation, baseRot.eulerAngles.z);
                pendingObject.transform.rotation = finalRot;
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
        GameObject selectedPrefab = objects[index];

        // Try to get OriginalPrefabInfo
        OriginalPrefabInfo info = selectedPrefab.GetComponent<OriginalPrefabInfo>();
        Quaternion defaultRotation = selectedPrefab.transform.rotation;

        if (info != null)
        {
            defaultRotation = info.defaultRotation;
            Debug.Log("Using defaultRotation from script: " + defaultRotation.eulerAngles);
        }
        else
        {
            Debug.Log("Using transform.rotation: " + defaultRotation.eulerAngles);
        }

        // Instantiate
        pendingObject = Instantiate(selectedPrefab, pos, defaultRotation);
        pendingObject.transform.localScale = selectedPrefab.transform.localScale;

        // Track the base Y rotation
        currentYRotation = defaultRotation.eulerAngles.y;

        if (inventoryManager != null)
        {
            inventoryManager.CloseCurrentPage();
        }
    }

    public void RotateObject()
    {
        currentYRotation += rotateAmount;
        currentYRotation %= 360f; 
    }
}
