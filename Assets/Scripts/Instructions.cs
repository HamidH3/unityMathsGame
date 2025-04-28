using System;
using UnityEngine;
using System.Collections;

public class Instructions : MonoBehaviour
{
    public bool canClick = true;
    public SpaceShip spaceShip;
    [Header("Panel to Show When Clicked")]
    public GameObject OpenPanel;

    [Header("Optional: Player Reference")]
    public Player player;

    private void OnMouseDown()
    {
        if (!canClick) return;

        if (OpenPanel != null)
        {
            OpenPanel.SetActive(true);
            spaceShip.canClick = false;

        }

        if (player != null)
        {
            player.DisableMovement();
        }
    }


 
}
