using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("GOOOL!");
        }
    }
}
