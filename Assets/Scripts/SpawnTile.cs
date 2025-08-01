using UnityEngine;
using System.Collections.Generic;

public class SpawnTile : MonoBehaviour
{
    [Header("Tile Spawning")]
    public GameObject tileToSpawn;
    public GameObject referenceObject;
    public float timeOffset = 0.4f;
    public float distanceBetweenTiles = 5.0f;
    public float randomValue = 0.8f;
    public int poolSize = 20;

    private Vector3 previousTilePosition;
    private float startTime;

    private Vector3 direction;
    private Vector3 mainDirection = Vector3.forward;
    private Vector3 otherDirection = Vector3.right;

    private Queue<GameObject> tilePool = new Queue<GameObject>();
    private ObstacleManager obstacleManager;

    void Start()
    {
        obstacleManager = GetComponent<ObstacleManager>();
        previousTilePosition = referenceObject.transform.position;
        direction = mainDirection;
        startTime = Time.time;

        // Preload tile pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject tile = Instantiate(tileToSpawn, Vector3.zero, Quaternion.identity);
            tile.SetActive(false);
            tilePool.Enqueue(tile);
        }

        // Spawn the first tile
        SpawnNextTile();
    }

    void Update()
    {
        if (Time.time - startTime > timeOffset)
        {
            startTime = Time.time;
            SpawnNextTile();
        }
    }

    void SpawnNextTile()
    {
        // Choose direction
        if (Random.value < randomValue)
        {
            direction = mainDirection;
        }
        else
        {
            Vector3 temp = direction;
            direction = otherDirection;
            mainDirection = direction;
            otherDirection = temp;
        }

        // Calculate spawn position
        Vector3 spawnPos = previousTilePosition + direction * distanceBetweenTiles;

        // Get tile from pool
        GameObject tile = tilePool.Dequeue();
        tile.transform.position = spawnPos;
        tile.transform.rotation = Quaternion.LookRotation(direction);
        tile.SetActive(true);

        // Destroy old obstacles on reused tile
        foreach (Transform child in tile.transform)
        {
            if (child.CompareTag("SpawnedObstacle"))
                Destroy(child.gameObject);
        }

        // Spawn obstacle
        if (obstacleManager != null)
            obstacleManager.SpawnObstacleOnTile(tile);

        // Enqueue tile back to pool
        tilePool.Enqueue(tile);

        // Update position for next tile
        previousTilePosition = spawnPos;
    }
}

