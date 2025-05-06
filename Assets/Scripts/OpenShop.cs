using System;
using UnityEngine;
using System.Collections;


public class OpenShop : MonoBehaviour
{
    [Header("Panel to Show When Clicked")]
    public GameObject OpenPanel;

    [Header("Optional: Player Reference")]
    public Player player;

    public QuestionGenerator questionGenerator;
    public Narrator narrator;
    public SpaceShip spaceship;

    private void Start()
    {
        if (OpenPanel != null)
        {
            OpenPanel.SetActive(false);
        }
    }
    //content is called when game object is clicked
    private void OnMouseDown()
    {
        if (questionGenerator != null && questionGenerator.HasKey)
        {
            if (OpenPanel != null)
            {
                OpenPanel.SetActive(true);
                spaceship.canClick = false;

            }

            if (player != null)
            {
                player.DisableMovement();
            }
        }
        else
        {
            if (narrator != null)
            {
                narrator.ShowMessage("You Must Find The Key To Open The Shop...");
                StartCoroutine(DisableMessage());
            }
           

        }
        

    }
    private IEnumerator DisableMessage()
    {
        yield return new WaitForSeconds(5f);
        narrator.messageText.text = "";

    }
}
