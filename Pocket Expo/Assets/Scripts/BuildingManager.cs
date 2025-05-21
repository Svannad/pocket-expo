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
        foreach (var col in colliders) col.enabled = false;

        Rigidbody rb = pendingObject.GetComponent<Rigidbody>();
        bool hadRb = rb != null;
        bool originalKinematic = false;
        if (hadRb)
        {
            originalKinematic = rb.isKinematic;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (pendingObject.CompareTag("WallOnly"))
        {
            if (Physics.Raycast(ray, out hitInfo, 1000f, LayerMask.GetMask("Wall")))
            {
                // Combine all child colliders to get more accurate total size
                Collider[] allCols = pendingObject.GetComponentsInChildren<Collider>();
                Bounds combinedBounds = new Bounds(pendingObject.transform.position, Vector3.zero);
                foreach (var c in allCols)
                {
                    combinedBounds.Encapsulate(c.bounds);
                }

                // Calculate offset based on size in the normal direction (Z-axis)
                float offsetFromWall = Vector3.Dot(combinedBounds.extents, hitInfo.normal.normalized);
                offsetFromWall = Mathf.Abs(offsetFromWall); // Make sure it's positive
                if (offsetFromWall < 0.01f) offsetFromWall = 0.1f; // Safety fallback


                pos = hitInfo.point + hitInfo.normal * offsetFromWall;

                Quaternion wallFacing = Quaternion.LookRotation(-hitInfo.normal);
                Quaternion originalRot = pendingObject.GetComponent<OriginalPrefabInfo>().defaultRotation;
                Vector3 originalEuler = originalRot.eulerAngles;
                float combinedY = wallFacing.eulerAngles.y + originalEuler.y;

                Quaternion finalRot = Quaternion.Euler(originalEuler.x, combinedY, originalEuler.z);
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

        // Set position
        pendingObject.transform.position = pos;

        // Re-enable colliders
        foreach (var col in colliders) col.enabled = true;

        // Re-enable rigidbody physics if it had one
        if (hadRb)
        {
            rb.isKinematic = originalKinematic;
            rb.useGravity = !originalKinematic;
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
        if (pendingObject != null && pendingObject.CompareTag("GroundOnly"))
        {
            currentYRotation += rotateAmount;
            currentYRotation %= 360f;
        }
    }

}
