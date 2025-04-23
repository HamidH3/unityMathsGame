
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

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("FloatingPlatform"))
    //    {
    //        transform.SetParent(null);
    //    }
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("FloatingPlatform"))
    //    {
    //        MovingFloatingPlatform platform = collision.gameObject.GetComponent<MovingFloatingPlatform>();
    //        if (platform != null)
    //        {
    //            currPlatform = platform;




    //        }

    //    }
    //}
        //private void OnCollisionExit2D(Collision2D collision)
        //{
          


        //    if (collision.gameObject.CompareTag("FloatingPlatform"))
        //    {
        //        currPlatform = null;

        //    }
     

        //}
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


