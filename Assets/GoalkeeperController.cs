using System.Collections;
using UnityEngine;

public class GoalkeeperController : MonoBehaviour
{
    private Animator animator;

    [Header("Dive Settings")]
    public float lowDiveDistance = 1.5f;
    public float lowDiveHeight = 0.0f;

    public float highDiveDistance = 1.5f;
    public float highDiveHeight = 0.0f;

    public float diveDuration = 0.2f;

    private Vector3 startPosition;

    private bool hasDived = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        startPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetGoalkeeper();
            return;
        }

        if (hasDived)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Dive(
                "LowDiveRight",
                transform.right,
                lowDiveDistance,
                lowDiveHeight
            );
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Dive(
                "LowDiveLeft",
                -transform.right,
                lowDiveDistance,
                lowDiveHeight
            );
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Dive(
                "HighDiveRight",
                transform.right,
                highDiveDistance,
                highDiveHeight
            );
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Dive(
                "HighDiveLeft",
                -transform.right,
                highDiveDistance,
                highDiveHeight
            );
        }
    }

    void Dive(
        string animationName,
        Vector3 direction,
        float distance,
        float height
    )
    {
        hasDived = true;

        animator.Play(animationName);

        StartCoroutine(
            DiveMovement(
                direction,
                distance,
                height
            )
        );
    }

    IEnumerator DiveMovement(
        Vector3 direction,
        float distance,
        float height
    )
    {
        Vector3 initialPos = transform.position;

        Vector3 targetPos =
            initialPos + (direction * distance);

        float elapsed = 0f;

        while (elapsed < diveDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / diveDuration;

            Vector3 pos =
                Vector3.Lerp(
                    initialPos,
                    targetPos,
                    t
                );

            pos.y +=
                Mathf.Sin(t * Mathf.PI) * height;

            transform.position = pos;

            yield return null;
        }

        transform.position = targetPos;
    }

    void ResetGoalkeeper()
    {
        StopAllCoroutines();

        hasDived = false;

        transform.position = startPosition;

        animator.Play("Idle");
    }
}
