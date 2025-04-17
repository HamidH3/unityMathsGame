using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    public GameObject MainScreenOverlay;
    public GameObject QPanel;
    public GameObject MainMenu;


    public Player player;
    // Start is called before the first frame update
   
    private void Start()
    {
        MainMenu.SetActive(true);
        QPanel.SetActive(false);
        player.DisableMovement();
        Time.timeScale = 0f;


    }
    
    public void QPanelClose()
    {
        QPanel.SetActive(false);
        Time.timeScale = 1f;
        player.EnableMovement();
    }

    public void StartGame()
    {
        MainMenu.SetActive(false);
        Time.timeScale = 1f;
        if (player != null) {
            player.EnableMovement();
        }
        MainScreenOverlay.SetActive(true);
    }
}
