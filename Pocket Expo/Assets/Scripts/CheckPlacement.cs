using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlacement : MonoBehaviour
{
    BuildingManager buildingManager;

    private bool touchingWall = false;
    private bool touchingGround = false;

    void Start()
    {
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            touchingWall = true;
        }
        else if (other.CompareTag("Ground"))
        {
            touchingGround = true;
        }

        EvaluatePlacement();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            touchingWall = false;
        }
        else if (other.CompareTag("Ground"))
        {
            touchingGround = false;
        }

        EvaluatePlacement();
    }

    void EvaluatePlacement()
    {
        if (gameObject.CompareTag("GroundOnly"))
        {
            buildingManager.canPlace = touchingGround && !touchingWall;
        }
        else if (gameObject.CompareTag("WallOnly"))
        {
            buildingManager.canPlace = touchingWall && !touchingGround;
        }
        else
        {
            // Default: allow placing if not overlapping with both or none (can customize)
            buildingManager.canPlace = !touchingWall && !touchingGround ? false : true;
        }
    }
}
