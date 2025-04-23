using UnityEngine;
using System.Collections;

public class RandomSpawns : MonoBehaviour
{
    public GameObject mathChest; // Drag the MathChest prefab or instance here
    public Transform[] spawnPoints; // Assign your empty GameObjects here in the Inspector
    private int lastIndex = -1;

    private MovingFloatingPlatform currentPlatform;
    public QuestionGenerator questionGenerator;

    private float timer = 0f;
    private float respawnTime = 20f;
    private bool isQActive = false;
    private bool isPaused = false;



    private void Start()
    {
        SpawnAtRandomLocation();
    }
    private void Update()
    {
        if (isPaused) return;
        if (mathChest.activeSelf && currentPlatform != null)
        {
            mathChest.transform.position += currentPlatform.deltaMovement;
        }
        if (!isQActive)
        {
            timer += Time.deltaTime;
            if (timer >= respawnTime)
            {
                RespawnChest();
            }
        }
    }
    public void PauseSpawning()
    {
        isPaused = true;
    }
    public void ResumeSpawning()
    {
        isPaused = false;
    }

    public void SpawnAtRandomLocation()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        int index;
        do
        {
            index = Random.Range(0, spawnPoints.Length);
        } while (index == lastIndex && spawnPoints.Length > 1); // prevent same spot twice

        lastIndex = index;
        Transform chosenSpawn = spawnPoints[index];

        mathChest.transform.SetParent(chosenSpawn.parent);
        mathChest.transform.position = chosenSpawn.position;
        mathChest.SetActive(true); // In case it was deactivated

        currentPlatform = chosenSpawn.GetComponentInParent<MovingFloatingPlatform>();
        timer = 0f;
    }
    public void StartQuestion()
    {
        isQActive = true;
    }
    public void EndQAndRespawn()
    {
        mathChest.SetActive(false);
        isQActive = false;
        timer = 0f;

        //StartCoroutine(WaitForPanelToCloseAndRespawn());
    }
    //private IEnumerator WaitForPanelToCloseAndRespawn()
    //{
    //    // Wait until QPanel is inactive
    //    yield return new WaitUntil(() => questionGenerator != null && !questionGenerator.QPanel.activeSelf);

    //    yield return new WaitForSeconds(1f); // Optional delay
    //    SpawnAtRandomLocation();
    //}

    public void RespawnChest()
    {
        mathChest.SetActive(false);
        SpawnAtRandomLocation();
        timer = 0f;
    }



    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("FloatingPlatform"))
    //    {
    //        MovingFloatingPlatform platform = collision.gameObject.GetComponent<MovingFloatingPlatform>();
    //        if (platform != null)
    //        {
    //            currentPlatform = platform;


    //        }

    //    }
    //}
    //private void OnCollisionExit2D()
    //{
    //    DespawnChest();
    //}
}
