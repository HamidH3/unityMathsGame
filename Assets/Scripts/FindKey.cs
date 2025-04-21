using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindKey : MonoBehaviour
{


    public QuestionGenerator questionGenerator;
    public Narrator narrator;

    private Renderer keyRenderer;

    private void Start()
    {
        questionGenerator = FindObjectOfType<QuestionGenerator>();
        narrator = FindObjectOfType<Narrator>();
        keyRenderer = GetComponent<Renderer>();

    }
    private void OnMouseDown()
    {
        if (questionGenerator != null && narrator != null)
        {
            questionGenerator.CollectKey(); // set hasKey = true and update UI
            narrator.ShowMessage("Well Done, Now you can access the shop...");
            
            //this is brute forcing the key to hide. i will make it invisible 
            //for 3 seconds then it will be set to false in the IEnumerator.
            //since i couldnt add setFalse here because gameobject needs to be active
            //for Coroutine to happen, I had to add it there, and hence I made it 
            //invisible here.
            if (keyRenderer != null)
            {
                keyRenderer.enabled = false; // Hide the key
            }

            StartCoroutine(DisableMessage());
            
        }
    }
    private IEnumerator DisableMessage()
    {
        
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
        narrator.messageText.text = "";
       


    }
}
