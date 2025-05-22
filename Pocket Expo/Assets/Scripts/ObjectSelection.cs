using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ObjectSelection : MonoBehaviour
{
    public GameObject selectedObject;
    private BuildingManager buildingManager;
    public GameObject objUi;
    public Camera activeCamera;

    private void Start()
    {
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
    }

    private void Update()
    {
        if (buildingManager.pendingObject != null) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000))
            {
                if (hit.collider.CompareTag("GroundOnly") || hit.collider.CompareTag("WallOnly"))
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

        // Play selection sound
        ObjectSoundFeedback sound = obj.GetComponent<ObjectSoundFeedback>();
        if (sound != null)
        {
            sound.PlaySelect();
        }
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
        if (selectedObject == null) return;

        buildingManager.pendingObject = selectedObject;

        // Play move sound
        ObjectSoundFeedback sound = selectedObject.GetComponent<ObjectSoundFeedback>();
        if (sound != null)
        {
            sound.PlayMove();
        }
    }

    public void Delete()
    {
        if (selectedObject == null) return;

        GameObject objToDestroy = selectedObject;
        Deselect();

        // Play delete sound
        ObjectSoundFeedback sound = objToDestroy.GetComponent<ObjectSoundFeedback>();
        if (sound != null)
        {
            sound.PlayDelete();
        }

        Renderer rend = objToDestroy.GetComponentInChildren<Renderer>();
        if (rend != null && rend.material.HasProperty("_Color"))
        {
            DOTween.Sequence()
                .Append(objToDestroy.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack))
                .Join(rend.material.DOFade(0f, 0.3f))
                .OnComplete(() => Destroy(objToDestroy));
        }
        else
        {
            objToDestroy.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack)
                .OnComplete(() => Destroy(objToDestroy));
        }
    }
}
