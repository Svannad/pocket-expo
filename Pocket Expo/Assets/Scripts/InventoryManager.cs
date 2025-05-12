using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] pages; // Assign your 6 page GameObjects in the Inspector
    private int currentOpenIndex = -1; // -1 means no page is open

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
