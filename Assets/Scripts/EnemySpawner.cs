using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float minSpawnTime = 3f;
    [SerializeField] private float maxSpawnTime = 8f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int maxEnemies = 10;
    
    private int currentEnemyCount = 0;
    private bool isSpawning = true;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (isSpawning)
        {
            if (currentEnemyCount < maxEnemies)
            {
                float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
                yield return new WaitForSeconds(waitTime);
                
                if (spawnPoints.Length > 0)
                {
                    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                    
                    // Register enemy with spawner
                    Enemy enemyComponent = enemy.GetComponent<Enemy>();
                    if (enemyComponent != null)
                    {
                        enemyComponent.SetSpawner(this);
                        currentEnemyCount++;
                    }
                }
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void EnemyDestroyed()
    {
        currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1);
    }

    public void SetSpawningActive(bool active)
    {
        isSpawning = active;
    }

    public void AdjustSpawnRate(float difficultyMultiplier)
    {
        // Adjust spawn rates based on game difficulty
        minSpawnTime = Mathf.Max(1f, 3f / difficultyMultiplier);
        maxSpawnTime = Mathf.Max(3f, 8f / difficultyMultiplier);
    }
}

