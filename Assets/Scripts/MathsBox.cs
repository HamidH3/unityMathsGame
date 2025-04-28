
using UnityEngine;

public class MathChest : MonoBehaviour
{
    public GameObject QuestionGeneratorObj;  // Reference to the GameObject containing QuestionGenerator
    public CanvasScript canvasScript;
    //public MathsBoxManager mathsBoxManager;
    //public RandomSpawns spawns;
    //private MovingFloatingPlatform currPlatform;

    //private bool QStarted = false;
    //public Player player;
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            //QStarted = true;
            //canvasScript.QPanelOpen();
            canvasScript.QPanelOpen();
            FindObjectOfType<QuestionGenerator>().GenerateQuestion();

            //spawns.StartQuestion();
            //FindObjectOfType<RandomSpawns>().DespawnChest();

        }
        else if (other.CompareTag("FloatingPlatform"))
        {
            transform.SetParent(other.transform);
        }

    }

    
   }


