using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] pages;
    private int currentOpenIndex = -1;

    public void TogglePage(int index)
    {
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
}
