using System;
using UnityEngine;
using System.Collections;

public class Instructions : MonoBehaviour
{
    [Header("Panel to Show When Clicked")]
    public GameObject OpenPanel;

    [Header("Optional: Player Reference")]
    public Player player;

    private void OnMouseDown()
    {
        if (OpenPanel != null)
        {
            OpenPanel.SetActive(true);
        }

        if (player != null)
        {
            player.DisableMovement();
        }
    }


 
}
