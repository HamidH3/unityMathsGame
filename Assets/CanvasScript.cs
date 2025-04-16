using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    public GameObject QPanel;
    public Player player;
    // Start is called before the first frame update
    private void Start()
    {
        player = FindObjectOfType<Player>(); 
    }

    public void QPanelClose()
    {
        QPanel.SetActive(false);
        Time.timeScale = 1f;

        if (player != null)
        {
            player.EnableMovement(); 
        }
    }
}
