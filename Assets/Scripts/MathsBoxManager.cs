using System.Collections;
using UnityEngine;

public class MathsBoxManager : MonoBehaviour
{
    public GameObject[] mathBoxes; // Assign in Inspector
    public int currentActiveIndex = -1;

    // Method to activate a random box, deactivating any previously active box
    public void ActivateRandomBox()
    {
        // Deactivate the previously active box, if any
        if (currentActiveIndex != -1 && currentActiveIndex < mathBoxes.Length)
        {
            mathBoxes[currentActiveIndex].SetActive(false);
        }

        // Find a random index for the new box
        int nextIndex;
        do
        {
            nextIndex = Random.Range(0, mathBoxes.Length);  // Get a random index for a box
        } while (nextIndex == currentActiveIndex);  // Ensure it's not the same box

        // Set the new box as active
        currentActiveIndex = nextIndex;
        mathBoxes[currentActiveIndex].SetActive(true);
    }
}
