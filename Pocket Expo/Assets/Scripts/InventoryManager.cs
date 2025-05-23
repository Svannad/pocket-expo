using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] pages;
    private int currentOpenIndex = -1;
    public GameObject inventoryMenu;

    public void TogglePage(int index)
    {
        Debug.Log("Toggling to panel: " + index);

        if (currentOpenIndex == index)
        {
            // Close the currently open page
            pages[index].SetActive(false);
            currentOpenIndex = -1;
        }
        else
        {
            // Close previously open page
            if (currentOpenIndex != -1)
                pages[currentOpenIndex].SetActive(false);

            // Open the selected page
            pages[index].SetActive(true);
            currentOpenIndex = index;
        }
    }

    public void CloseCurrentPage()
    {
        if (currentOpenIndex != -1)
        {
            pages[currentOpenIndex].SetActive(false);
            currentOpenIndex = -1;
        }
    }
  public void SetInventoryVisibility(bool visible)
    {
        if (inventoryMenu != null)
        {
            inventoryMenu.SetActive(visible);

            // Optionally close any open pages when hiding
            if (!visible)
            {
                CloseCurrentPage();
            }
        }
        else
        {
            Debug.LogWarning("Inventory Menu GameObject reference is missing.");
        }
    }

}
