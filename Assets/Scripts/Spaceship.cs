using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{


  
    public ShopController shopController;
    public Narrator narrator;
    public CanvasScript canvasScript;

    private Renderer keyRenderer;

    private void Start()
    {
        //narrator = FindObjectOfType<Narrator>();
         

    }
    private void OnMouseDown()
    {
        if (narrator != null && !narrator.gameObject.activeSelf)
        {
            narrator.gameObject.SetActive(true); 
        }
        Debug.Log((shopController.HasEnoughFuel));
        if (shopController.HasEnoughFuel == true)
        {
            canvasScript.EndMenuOpen();
            narrator.ShowMessage("Nice, You have completed the game... Turns out you are a mathmatician after all!");

        }
        else
        {
            narrator.ShowMessage("You need more fuel...");
            
        }
        StartCoroutine(DisableMessage());


    }
    private IEnumerator DisableMessage()
    {

        yield return new WaitForSeconds(5f);
        narrator.messageText.text = "";



    }
}
