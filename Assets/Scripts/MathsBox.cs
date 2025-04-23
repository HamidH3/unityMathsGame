
using UnityEngine;

public class MathChest : MonoBehaviour
{
    public GameObject QuestionGeneratorObj;  // Reference to the GameObject containing QuestionGenerator
    public CanvasScript canvasScript;
    public RandomSpawns spawns;
    //private bool QStarted = false;
    //public Player player;
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            //QStarted = true;
            canvasScript.QPanelOpen();
            FindObjectOfType<QuestionGenerator>().GenerateQuestion();
            //spawns.StartQuestion();
            //FindObjectOfType<RandomSpawns>().DespawnChest();

        }
    }
    //private void OnTriggerExit2D()
    //{
    //    QStarted = false;
    //    spawns.EndQAndRespawn();

    //}
    //private void Update()
    //{
    //    FindObjectOfType<RandomSpawns>().SpawnAtRandomLocation();

    //}
}


