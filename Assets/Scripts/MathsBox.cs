
using UnityEngine;

public class MathChest : MonoBehaviour
{
    public GameObject QuestionGeneratorObj;  // Reference to the GameObject containing QuestionGenerator
    public CanvasScript canvasScript;
    //public Player player;
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            canvasScript.QPanelOpen();
            FindObjectOfType<QuestionGenerator>().GenerateQuestion();

        }
    }
    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        canvasScript.QPanelClose();
    //    }
    //}
}


