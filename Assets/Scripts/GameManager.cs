using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Loop Progress")]
    public int segmentsCollected = 0;
    public int requiredSegments = 12;
    public float speedMultiplier = 1f;
    public float maxSpeedMultiplier = 3f;


    [Header("Game State")]
    public bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CollectSegment()
    {
        segmentsCollected++;
        Debug.Log("Collected Segment: " + segmentsCollected);
    }

    public bool CanPassGoal()
    {
        return segmentsCollected >= requiredSegments;
    }

    public void PassGoalGate()
    {
        speedMultiplier *= 1.5f;
        speedMultiplier = Mathf.Min(speedMultiplier, maxSpeedMultiplier); // ✅ Apply cap
        segmentsCollected = 0;

        Debug.Log("✅ Goal Passed! Speed increased to " + speedMultiplier);
    }

    public void GameOver(string reason)
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("💥 Game Over: " + reason);
        Time.timeScale = 0f;

        // Optional: Trigger UI, restart scene, etc.
    }
}
