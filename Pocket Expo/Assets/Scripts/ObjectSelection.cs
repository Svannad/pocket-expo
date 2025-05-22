using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ObjectSelection : MonoBehaviour
{
    public GameObject selectedObject;
    private BuildingManager buildingManager;
    public GameObject objUi;
    public Camera activeCamera;
    private Tween floatTween;

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
        if (selectedObject == null) return;

        GameObject objToDestroy = selectedObject;
        Deselect();

        Renderer rend = objToDestroy.GetComponentInChildren<Renderer>();
        if (rend != null && rend.material.HasProperty("_Color"))
        {
            Color originalColor = rend.material.color;

            Sequence seq = DOTween.Sequence();
            seq.Append(objToDestroy.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack));
            seq.Join(rend.material.DOFade(0f, 0.3f));
            seq.OnComplete(() => Destroy(objToDestroy));
        }
        else
        {
            objToDestroy.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack)
                .OnComplete(() => Destroy(objToDestroy));
        }
    }
}
