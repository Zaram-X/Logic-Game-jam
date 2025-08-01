using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
   [Header("Obstacle Settings")]
    public GameObject[] obstaclePrefabs;
    [Range(0f, 1f)] public float spawnChance = 0.4f;

    public void SpawnObstacleOnTile(GameObject tile)
    {
        if (obstaclePrefabs.Length == 0 || Random.value > spawnChance)
            return;

        // Find all child transforms
        Transform[] allChildren = tile.GetComponentsInChildren<Transform>();
        List<Transform> spawnPoints = new List<Transform>();

        foreach (Transform t in allChildren)
        {
            if (t.CompareTag("ObstacleSpawnPoint"))
                spawnPoints.Add(t);
        }

        if (spawnPoints.Count == 0)
        {
            Debug.Log("⚠️ No spawn points with tag 'ObstacleSpawnPoint' on tile: " + tile.name);
            return;
        }

        // Pick random spawn point and prefab
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        GameObject obstacle = Instantiate(prefab, point.position, point.rotation, tile.transform);
        obstacle.tag = "SpawnedObstacle";
    }
}
