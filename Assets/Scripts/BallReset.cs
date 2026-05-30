using UnityEngine;

public class BallReset : MonoBehaviour
{
    Vector3 startPos;
    Quaternion startRot;
    Rigidbody rb;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        
        rb = GetComponent<Rigidbody>();
    }
    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBall();
        }
    }
    */
    void ResetBall()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = startPos;
        transform.rotation = startRot;
    }
}
