using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSelection : MonoBehaviour
{
    public GameObject selectedObject;
    private BuildingManager buildingManager;
    public GameObject objUi;

    public Camera activeCamera;

    void Start()
    {
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
    }

void Update()
{
    if (buildingManager.pendingObject != null) return;

    if (EventSystem.current.IsPointerOverGameObject())
        return;

    if (Input.GetMouseButtonDown(0))
    {
        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.collider.gameObject.CompareTag("GroundOnly") || hit.collider.gameObject.CompareTag("WallOnly"))
            {
                Select(hit.collider.gameObject);
            }
            else
            {
                Deselect();
            }
        }
        else
        {
            Deselect();
        }
    }

    if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
    {
        Deselect();
    }
}




    private void Select(GameObject obj)
    {
        if (obj == selectedObject) return;
        if (selectedObject != null) Deselect();

        Outline outline = obj.GetComponent<Outline>();
        if (outline == null) obj.AddComponent<Outline>();
        else outline.enabled = true;
        objUi.SetActive(true);
        selectedObject = obj;
    }

    private void Deselect()
    {
        objUi.SetActive(false);
        if (selectedObject != null && selectedObject.GetComponent<Outline>() != null)
        {
            selectedObject.GetComponent<Outline>().enabled = false;
        }
        selectedObject = null;
    }

    public void Move()
    {
        buildingManager.pendingObject = selectedObject;
    }

    public void Delete()
    {
        GameObject objToDestroy = selectedObject;
        Deselect();
        Destroy(objToDestroy);
    }
}
