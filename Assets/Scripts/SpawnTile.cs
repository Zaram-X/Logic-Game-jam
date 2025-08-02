using UnityEngine;
using System.Collections.Generic;

public class SpawnTile : MonoBehaviour
{
    [Header("Chunk Settings")]
    public GameObject[] trackChunks; // Base Chunk, Pit Chunk, etc.
    public GameObject referenceObject; // Usually the player or first tile
    public float timeOffset = 0.5f;
    public float distanceBetweenChunks = 3f;

    [Header("Segment Settings")]
    public GameObject cageSegmentPrefab;
    public float segmentSpawnChance = 0.4f;

    [Header("Cleanup Settings")]
    public Transform player;
    public float cleanupDistance = 25f; // How far behind before a chunk is destroyed

    private Vector3 previousChunkPosition;
    private Vector3 direction = Vector3.forward;
    private float lastSpawnTime;

    private List<GameObject> activeChunks = new List<GameObject>();

    void Start()
    {
        previousChunkPosition = referenceObject.transform.position;
        lastSpawnTime = Time.time;

        // Spawn initial chunks
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

        CleanupOldChunks();
    }

    void SpawnNextChunk()
    {
        int index = Random.Range(0, trackChunks.Length);
        GameObject chunkPrefab = trackChunks[index];

        Vector3 spawnPos = previousChunkPosition + direction * distanceBetweenChunks;
        GameObject newChunk = Instantiate(chunkPrefab, spawnPos, Quaternion.LookRotation(direction));

        previousChunkPosition = spawnPos;
        activeChunks.Add(newChunk);

        MaybeSpawnSegment(spawnPos);
    }

    void MaybeSpawnSegment(Vector3 spawnPos)
    {
        if (Random.value < segmentSpawnChance)
    {
        Vector3 offset = new Vector3(0, 3.5f, 0); // above tile
        Quaternion rotation = Quaternion.Euler(0, 90f, 0); // Rotate 90° on Y axis
        Instantiate(cageSegmentPrefab, spawnPos + offset, rotation);
    }
    }

    void CleanupOldChunks()
    {
        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = activeChunks[i];
            float distanceBehind = player.position.z - chunk.transform.position.z;

            if (distanceBehind > cleanupDistance)
            {
                Destroy(chunk);
                activeChunks.RemoveAt(i);
            }
        }
    }
}
