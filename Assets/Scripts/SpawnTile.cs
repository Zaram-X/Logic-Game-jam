using UnityEngine;
using System.Collections.Generic;

public class SpawnTile : MonoBehaviour
{
    [Header("Chunk Settings")]
    public GameObject[] trackChunks;
    public GameObject referenceObject;
    public float timeOffset = 0.5f;
    public float distanceBetweenChunks = 3f;

    [Header("Segment & Goal Settings")]
    public GameObject cageSegmentPrefab;
    public GameObject goalGatePrefab;
    public float segmentHeight = 3.5f;
    public float gateHeight = 5f;

    [Header("Cleanup Settings")]
    public Transform player;
    public float cleanupDistance = 25f;

    private Vector3 previousChunkPosition;
    private Vector3 direction = Vector3.forward;
    private float lastSpawnTime;

    private List<GameObject> activeChunks = new List<GameObject>();

    // ⏱️ Timed Cycle Controls
    [Header("Cycle Settings")]
    public float spawnCycleDuration = 60f;
    private float cycleTimer = 0f;

    public int segmentsPerCycle = 15;
    private int segmentsSpawnedThisCycle = 0;
    private bool goalGateSpawned = false;

    void Start()
    {
        previousChunkPosition = referenceObject.transform.position;
        lastSpawnTime = Time.time;

        for (int i = 0; i < 5; i++)
        {
            SpawnNextChunk();
        }
    }

    void Update()
    {
        cycleTimer += Time.deltaTime;

        float adjustedOffset = timeOffset / GameManager.Instance.speedMultiplier;

        if (Time.time - lastSpawnTime > adjustedOffset)
        {
            SpawnNextChunk();
            lastSpawnTime = Time.time;
        }

        // Reset cycle after finishing segment + gate
        if (segmentsSpawnedThisCycle >= segmentsPerCycle && goalGateSpawned)
        {
            segmentsSpawnedThisCycle = 0;
            goalGateSpawned = false;
            cycleTimer = 0f;
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

        // Timed controlled spawning
        if (segmentsSpawnedThisCycle < segmentsPerCycle)
        {
            SpawnSegment(spawnPos);
            segmentsSpawnedThisCycle++;
        }
        else if (!goalGateSpawned)
        {
            SpawnGoalGate(spawnPos);
            goalGateSpawned = true;
        }
    }

    void SpawnSegment(Vector3 spawnPos)
    {
        Vector3 offset = new Vector3(0, segmentHeight, 0);
        Quaternion rotation = Quaternion.Euler(0, 90f, 0);
        Instantiate(cageSegmentPrefab, spawnPos + offset, rotation);
    }

    void SpawnGoalGate(Vector3 spawnPos)
    {
        Vector3 offset = new Vector3(0, gateHeight, 0);
        Instantiate(goalGatePrefab, spawnPos + offset, Quaternion.identity);
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
