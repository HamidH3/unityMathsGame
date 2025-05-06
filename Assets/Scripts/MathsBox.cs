
using UnityEngine;

public class MathChest : MonoBehaviour
{
    public GameObject QuestionGeneratorObj;  // Reference to the GameObject containing QuestionGenerator
    public CanvasScript canvasScript;
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {

            canvasScript.QPanelOpen();
            FindObjectOfType<QuestionGenerator>().GenerateQuestion();


        }
        else if (other.CompareTag("FloatingPlatform"))
        {
            transform.SetParent(other.transform);
        }

    }

    
   }


