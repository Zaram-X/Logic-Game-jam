using UnityEngine;
using System.Collections.Generic;

public class SpawnTile : MonoBehaviour
{
     [Header("Chunk Settings")]
    public GameObject[] trackChunks; // Assign Base Chunk, Small Pit Chunk, Big Pit Chunk, etc.
    public GameObject referenceObject; // Usually the player or first tile
    public float timeOffset = 0.5f;
    public float distanceBetweenChunks = 3f;

    [Header("Spawn Direction")]
    private Vector3 previousChunkPosition;
    private Vector3 direction = Vector3.forward;

    private float lastSpawnTime;

    void Start()
    {
        previousChunkPosition = referenceObject.transform.position;
        lastSpawnTime = Time.time;

        // Spawn a few chunks to start the loop
        for (int i = 0; i < 5; i++)
        {
            SpawnNextChunk();
        }
    }

    void Update()
    {
        if (Time.time - lastSpawnTime > timeOffset)
        {
            SpawnNextChunk();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnNextChunk()
    {
        // Pick a random chunk from the list
        int index = Random.Range(0, trackChunks.Length);
        GameObject chunkPrefab = trackChunks[index];

        // Determine spawn position
        Vector3 spawnPos = previousChunkPosition + direction * distanceBetweenChunks;

        // Spawn chunk
        GameObject newChunk = Instantiate(chunkPrefab, spawnPos, Quaternion.LookRotation(direction));
        previousChunkPosition = spawnPos;
    }
}

