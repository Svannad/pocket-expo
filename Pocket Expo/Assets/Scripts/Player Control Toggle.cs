using UnityEngine;

public class PlayerControlToggle : MonoBehaviour
{
    public WalkingCameraMovement movementScript;

    private bool isControlEnabled = false;

    public void TogglePlayerControl()
    {
        isControlEnabled = !isControlEnabled;
        movementScript.enabled = isControlEnabled;

        if (isControlEnabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
// This script toggles player control on and off, enabling or disabling the WalkingCameraMovement script.
