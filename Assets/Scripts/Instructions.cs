
//using UnityEngine;

//public class Instructions : MonoBehaviour
//{
//    public CanvasScript canvasScript; // Reference to the UI manager

//    private void OnMouseDown()
//    {
//        if (canvasScript != null)
//        {
//            canvasScript.BillBoardCloseup.SetActive(true);
//            canvasScript.player.DisableMovement();
//        }

//    }
//}


using UnityEngine;

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
