using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Area")]
    public Vector2 areaSize = new(20f, 20f);

    [Header("Enemy Prefabs")]
    public List<GameObject> enemyPrefabs = new();

    [Header("Spawn Settings")]
    public int enemyCount = 10;

    public LayerMask groundLayer;

    [Tooltip("How many times should I try to find the right spot?")]
    public int maxSpawnAttempts = 20;

    void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        if (enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("EnemySpawner: No enemy prefabs assigned.");
            return;
        }

        int spawned = 0;

        while (spawned < enemyCount)
        {
            bool foundPosition = false;
            Vector3 spawnPosition = Vector3.zero;

            for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
            {
                Vector3 candidate = GetRandomPosition();

                if (IsValidSpawnPosition(candidate))
                {
                    spawnPosition = candidate;
                    foundPosition = true;
                    break;
                }
            }

            if (!foundPosition)
            {
                Debug.LogWarning("EnemySpawner: Could not find valid spawn position.");

                break;
            }

            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            spawned++;
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f);

        float y = Random.Range(-areaSize.y * 0.5f, areaSize.y * 0.5f);

        return transform.position + new Vector3(x, y, 0f);
    }

    bool IsValidSpawnPosition(Vector3 position)
    {
        Collider2D ground = Physics2D.OverlapPoint(position, groundLayer);

        return ground != null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, new(areaSize.x, areaSize.y, 0f)
        );
    }
}