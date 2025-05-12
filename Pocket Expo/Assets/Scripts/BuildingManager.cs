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
    public float gridSize;
    bool gridOn = true;
    [SerializeField] private Toggle gridToggle;

    void Update()
    {
        if (pendingObject != null)
        {
            if (gridOn)
            {
                pendingObject.transform.position = new Vector3(
                    RoundToNearestGrid(pos.x),
                    RoundToNearestGrid(pos.y),
                    RoundToNearestGrid(pos.z)
                );
            }
            else
            {
                pendingObject.transform.position = pos;
            }

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
        pendingObject = null;
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            pos = hit.point;

            if (pendingObject != null)
            {
                PlaceableObject placeable = pendingObject.GetComponent<PlaceableObject>();

                string hitTag = hit.collider.tag;

                switch (placeable.placementType)
                {
                    case PlaceableObject.PlacementType.GroundOnly:
                        if (hitTag == "Ground" || hitTag == "Stackable")
                            pendingObject.SetActive(true);
                        else
                            pendingObject.SetActive(false);
                        break;

                    case PlaceableObject.PlacementType.Stackable:
                        if (hitTag == "Ground" || hitTag == "Stackable")
                            pendingObject.SetActive(true);
                        else
                            pendingObject.SetActive(false);
                        break;

                    case PlaceableObject.PlacementType.WallOnly:
                        if (hitTag == "Wall")
                        {
                            pendingObject.SetActive(true);

                            // Snap to wall surface
                            pendingObject.transform.position = hit.point;

                            // Align rotation with wall surface
                            Quaternion lookRotation = Quaternion.LookRotation(-hit.normal);
                            pendingObject.transform.rotation = lookRotation;

                            // Offset to prevent embedding in wall
                            Vector3 offset = hit.normal * 0.01f;
                            pendingObject.transform.position += offset;
                        }
                        else
                        {
                            pendingObject.SetActive(false);
                        }
                        break;
                }
            }
        }
    }

    public void SelectingObject(int index)
    {
        pendingObject = Instantiate(objects[index], pos, transform.rotation);
    }

    public void RotateObject()
    {
        if (pendingObject != null)
        {
            pendingObject.transform.Rotate(Vector3.up, rotateAmount);
        }
    }

    public void GridToggle()
    {
        gridOn = gridToggle.isOn;
    }

    float RoundToNearestGrid(float pos)
    {
        float xDiff = pos % gridSize;
        pos -= xDiff;

        if (xDiff > (gridSize / 2))
        {
            pos += gridSize;
        }

        return pos;
    }
}
