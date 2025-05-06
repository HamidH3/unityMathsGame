using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    public List<GameObject> buttons;
    public Narrator narrator;
    public Animator panelBackground;



    public void InitializeEndMenu()
    {
        if (panelBackground != null)
        {
            panelBackground.speed = 1f;
            panelBackground.Play(0); 
        }

        if (narrator != null)
        {
            narrator.ShowMessage("Well Done! You have completed the challenge using your superb maths knowledge!");
        }

        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }

        //start timer here
        StartCoroutine(ShowButtonsAfterAnim());
        StartCoroutine(StopBckgroundAnim());
    }

    IEnumerator ShowButtonsAfterAnim()
    {
        yield return new WaitForSeconds(10f); // wait for 10 secs
        narrator.HideMessage();

        //show buttons after 10 secs
        foreach (GameObject button in buttons)
        {
            button.SetActive(true);
        }
    }
    IEnumerator StopBckgroundAnim()
    {
        yield return new WaitForSeconds(10f);
        panelBackground.speed = 0f;
    }
}
