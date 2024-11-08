using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // List of prefabs to spawn
    public List<GameObject> prefabsToSpawn;

    // Reference to the player
    public Transform player;

    // Spawn area dimensions
    public float spawnAreaRadius = 50f;

    // Minimum distance from player
    public float minDistanceFromPlayer = 10f;

    // Spawn delay
    public float spawnDelay = 2f;

    void Start()
    {
        // Start the spawning coroutine
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnObject()
    {
        if (prefabsToSpawn.Count == 0)
        {
            Debug.LogWarning("No prefabs to spawn!");
            return;
        }

        // Randomly choose a prefab from the list
        GameObject prefabToSpawn = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Count)];

        // Calculate a spawn position
        Vector3 spawnPosition;
        do
        {
            Vector3 randomDirection = Random.insideUnitSphere * spawnAreaRadius;
            randomDirection.y = 0; // Flatten the spawn area on the Y-axis
            spawnPosition = player.position + randomDirection;

        } while (Vector3.Distance(spawnPosition, player.position) < minDistanceFromPlayer);

        // Instantiate the chosen prefab at the calculated spawn position
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }
}
