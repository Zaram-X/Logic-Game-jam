using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;

    [Header("Loop Progress")]
    public int segmentsCollected = 0;
    public int requiredSegments = 5;
    public float speedMultiplier = 1f;

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
        segmentsCollected = 0;
        Debug.Log("✅ Goal Passed! Speed increased.");
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
