using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] enemyPrefabs; // Array of enemy prefabs
    public int poolSize = 20; // Number of enemies in the pool for each prefab
    public float initialSpawnDelay = 2f;
    public float spawnInterval = 3f;

    [Header("Spawn Area Settings")]
    public Transform towerTransform; // Reference to the tower
    private List<GameObject>[] enemyPools; // Pools for each enemy type
    private float spawnTimer;

    private void Start()
    {
        InitializePools();
        spawnTimer = initialSpawnDelay;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver()) return;

        HandleEnemySpawning();
    }

    /// <summary>
    /// Initializes object pools for enemies.
    /// </summary>
    private void InitializePools()
    {
        enemyPools = new List<GameObject>[enemyPrefabs.Length];
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemyPools[i] = new List<GameObject>();
            for (int j = 0; j < poolSize; j++)
            {
                GameObject enemy = Instantiate(enemyPrefabs[i]);
                enemy.SetActive(false);
                enemyPools[i].Add(enemy);
            }
        }
    }

    /// <summary>
    /// Handles spawning of enemies at intervals.
    /// </summary>
    private void HandleEnemySpawning()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    /// <summary>
    /// Spawns an enemy at a random point outside the camera view.
    /// </summary>
    private void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = GetPooledEnemy(randomIndex);

        if (enemy != null)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            enemy.transform.position = spawnPosition;
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.target = towerTransform; // Assign the Tower's Transform
            }
            enemy.SetActive(true);
        }
    }

    /// <summary>
    /// Retrieves an inactive enemy from the pool or null if none are available.
    /// </summary>
    private GameObject GetPooledEnemy(int poolIndex)
    {
        foreach (GameObject enemy in enemyPools[poolIndex])
        {
            if (!enemy.activeInHierarchy)
                return enemy;
        }
        return null; // Pool exhausted
    }

    /// <summary>
    /// Calculates a random spawn point outside the camera view.
    /// </summary>
    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-0.2f, 0.2f);
        float y = Random.Range(-0.2f, 0.2f);
        if (x >= 0) x += 0.9f;
        if (y >= 0) y += 0.9f;
        Vector3 randomPoint = new(x, y);

        randomPoint.z = 10f;
        Vector3 worldPoint = Camera.main.ViewportToWorldPoint(randomPoint);
        worldPoint.y = 0.5f;
        return worldPoint;
    }
}