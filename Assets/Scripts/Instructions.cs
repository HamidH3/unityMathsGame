using System;
using UnityEngine;
using System.Collections;

public class Instructions : MonoBehaviour
{
    public bool canClick = true;
    public SpaceShip spaceShip;
    //this can be called for multiple GameObjects, but in this case its only called for 'billboard' panel
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
