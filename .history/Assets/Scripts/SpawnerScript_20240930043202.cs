using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner2D : MonoBehaviour
{
    // List of prefabs to spawn
    public List<GameObject> prefabsToSpawn;

    // Reference to the player
    public Transform player;

    // Reference to the BoxCollider2D defining the spawn area
    public BoxCollider2D spawnArea;

    // Minimum distance from player
    public float minDistanceFromPlayer = 5f;

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

        // Calculate a spawn position within the BoxCollider2D
        Vector3 spawnPosition;
        do
        {
            spawnPosition = GetRandomPointInBox2D(spawnArea);

        } while (Vector3.Distance(spawnPosition, player.position) < minDistanceFromPlayer);

        // Instantiate the chosen prefab at the calculated spawn position in 2D
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }

    // Generate a random point within the BoxCollider2D
    Vector3 GetRandomPointInBox2D(BoxCollider2D box)
    {
        // Get the bounds of the BoxCollider2D
        Bounds bounds = box.bounds;

        // Generate a random position within the bounds (X and Y only)
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(randomX, randomY, 0f); // Z is zero for 2D
    }
}
