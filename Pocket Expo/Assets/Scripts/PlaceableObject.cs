using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public enum PlacementType
    {
        GroundOnly,
        Stackable,
        WallOnly
    }

    public PlacementType placementType;
    public bool allowStackingOnTop = true;
}
