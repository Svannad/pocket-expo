using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject pendingObject;
    private Vector3 pos;
    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;
    public float rotateAmount;
    public InventoryManager inventoryManager;
    private float currentYRotation = 0f;
    public Camera activeCamera;
    public AudioClip placeSound;
    public AudioClip rotateSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (pendingObject != null)
        {
            pendingObject.transform.position = pos;

            if (Input.GetMouseButtonDown(0))
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
        GameObject placed = pendingObject;
        pendingObject = null;

        if (placed != null)
        {
            Vector3 originalScale = placed.transform.localScale;

            placed.transform.DOScale(originalScale * 0.8f, 0.05f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                placed.transform.DOScale(originalScale, 0.15f).SetEase(Ease.OutBack);
            });
        }

        if (placeSound != null)
        {
            audioSource.PlayOneShot(placeSound);
        }
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
                Collider[] allCols = pendingObject.GetComponentsInChildren<Collider>();
                Bounds combinedBounds = new Bounds(pendingObject.transform.position, Vector3.zero);
                foreach (var c in allCols)
                {
                    combinedBounds.Encapsulate(c.bounds);
                }

                float offsetFromWall = Vector3.Dot(combinedBounds.extents, hitInfo.normal.normalized);
                offsetFromWall = Mathf.Abs(offsetFromWall);
                if (offsetFromWall < 0.01f) offsetFromWall = 0.1f;

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

        pendingObject.transform.position = pos;

        foreach (var col in colliders) col.enabled = true;

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

        pendingObject = Instantiate(selectedPrefab, pos, defaultRotation);
        pendingObject.transform.localScale = selectedPrefab.transform.localScale;

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

            if (rotateSound != null)
            {
                audioSource.PlayOneShot(rotateSound);
            }
        }
    }
}
