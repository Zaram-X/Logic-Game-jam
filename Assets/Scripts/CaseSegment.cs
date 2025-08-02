using UnityEngine;

public class CaseSegment : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectSegment(); // 🟢 Increases count
            Destroy(gameObject);                   // 🔴 Remove segment
        }
    }

}
