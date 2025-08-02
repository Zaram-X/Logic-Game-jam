using UnityEngine;

public class GoalGate : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance.CanPassGoal())
        {
            GameManager.Instance.PassGoalGate(); // ✅ Successful pass
            Destroy(gameObject);
        }
        else
        {
            GameManager.Instance.GameOver("Didn't collect enough segments!");
        }
    }
    
}
