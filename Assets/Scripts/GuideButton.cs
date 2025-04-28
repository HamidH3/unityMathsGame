using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideButton : MonoBehaviour
{
    public GameObject guidePanel;
    public GameObject guideButton;
    public List<GameObject> MainMenuButtons;
    public Narrator narrator;



    private bool guideOpen = false;



    private void Start()
    {

        guidePanel.SetActive(false);

    }

    public void ToggleGuide()
    {
        guideOpen = !guideOpen;

        if (guideOpen)
        {

            guidePanel.SetActive(true);
            foreach (GameObject button in MainMenuButtons)
            {
                button.SetActive(false);
            }
            narrator.ShowMessage("Welcome, Explorer!\r\n\r\nAnswer maths questions to earn points!\r\nRight answers give you points.\r\nWrong answers lose you health!\r\n\r\nReach Level 3 to find a hidden key in a secret cave.\r\nUse the key to unlock the Shop, where you can spend points on Health Bars or Fuel Bars.\r\n\r\nYou need 3 Fuel Bars to escape the planet.\r\nNo fuel means you’re stuck forever! No health means you perish...\r\n\r\nSolve questions, stay healthy, find the key, buy fuel — and escape!\r\n\r\nGood luck! ");
        }
       


        
    }

    public void BackButton()
    {
        guidePanel.SetActive(false);
        foreach (GameObject button in MainMenuButtons)
        {
            button.SetActive(true);
        }
    }


}
