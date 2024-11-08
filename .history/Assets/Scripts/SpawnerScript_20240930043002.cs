using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // List of prefabs to spawn
    public List<GameObject> prefabsToSpawn;

    // Reference to the player
    public Transform player;

    // Reference to the BoxCollider defining the spawn area
    public BoxCollider spawnArea;

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

        // Calculate a spawn position within the BoxCollider
        Vector3 spawnPosition;
        do
        {
            spawnPosition = GetRandomPointInBox(spawnArea);

        } while (Vector3.Distance(spawnPosition, player.position) < minDistanceFromPlayer);

        // Instantiate the chosen prefab at the calculated spawn position
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }

    // Generate a random point within the BoxCollider
    Vector3 GetRandomPointInBox(BoxCollider box)
    {
        // Get the bounds of the BoxCollider
        Bounds bounds = box.bounds;

        // Generate a random position within the bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, randomY, randomZ);
    }
}
