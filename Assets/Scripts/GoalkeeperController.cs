using System.Collections;
using UnityEngine;

public class GoalkeeperController : MonoBehaviour
{
    private Animator animator;

    public enum DecisionMode
    {
        Random,
        Predict
    }

    [Header("AI")]
    public DecisionMode decisionMode = DecisionMode.Random;

    [Range(0f, 1f)]
    public float intelligence = 1f;

    [Header("Predict Settings")]

    [Range(0f, 1f)]
    public float leftThreshold = 0.35f;

    [Range(0f, 1f)]
    public float rightThreshold = 0.65f;

    [Header("Random Weights")]
    public bool canStayIdle = true;

    public int stayWeight = 1;
    public int lowLeftWeight = 1;
    public int highLeftWeight = 1;
    public int lowRightWeight = 1;
    public int highRightWeight = 1;

    [Header("Dive Settings")]
    public float lowDiveDistance = 1.5f;
    public float lowDiveHeight = 0f;

    public float highDiveDistance = 1.5f;
    public float highDiveHeight = 0f;

    public float diveDuration = 0.2f;

    [Header("Animation Names")]
    public string idleAnimation = "Idle";

    public string lowLeftAnimation = "LowDiveLeft";
    public string lowRightAnimation = "LowDiveRight";

    public string highLeftAnimation = "HighDiveLeft";
    public string highRightAnimation = "HighDiveRight";

    private Vector3 startPosition;
    private bool hasDived = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
    }

    public bool HasDived()
    {
        return hasDived;
    }

    public void DecideAction(
        float directionValue,
        float heightValue
    )
    {
        if (decisionMode == DecisionMode.Random)
        {
            ExecuteRandomAction();
        }
        else
        {
            PredictShot(
                directionValue,
                heightValue
            );
        }
    }

    private void ExecuteRandomAction()
    {
        int totalWeight = 0;

        if (canStayIdle)
            totalWeight += stayWeight;

        totalWeight += lowLeftWeight;
        totalWeight += highLeftWeight;
        totalWeight += lowRightWeight;
        totalWeight += highRightWeight;

        int roll = Random.Range(0, totalWeight);

        if (canStayIdle)
        {
            if (roll < stayWeight)
                return;

            roll -= stayWeight;
        }

        if (roll < lowLeftWeight)
        {
            DiveLowLeft();
            return;
        }

        roll -= lowLeftWeight;

        if (roll < highLeftWeight)
        {
            DiveHighLeft();
            return;
        }

        roll -= highLeftWeight;

        if (roll < lowRightWeight)
        {
            DiveLowRight();
            return;
        }

        DiveHighRight();
    }

    private void PredictShot(
        float direction,
        float height
    )
    {
        float roll = Random.value;

        if (roll > intelligence)
        {
            ExecuteRandomAction();
            return;
        }

        if (
            direction >= leftThreshold &&
            direction <= rightThreshold
        )
        {
            return;
        }

        bool left = direction < leftThreshold;
        bool low = height < 0.5f;

        if (left && low)
        {
            DiveLowRight();
        }
        else if (left)
        {
            DiveHighRight();
        }
        else if (low)
        {
            DiveLowLeft();
        }
        else
        {
            DiveHighLeft();
        }
    }

    public void DiveLowLeft()
    {
        Dive(
            lowLeftAnimation,
            -transform.right,
            lowDiveDistance,
            lowDiveHeight
        );
    }

    public void DiveLowRight()
    {
        Dive(
            lowRightAnimation,
            transform.right,
            lowDiveDistance,
            lowDiveHeight
        );
    }

    public void DiveHighLeft()
    {
        Dive(
            highLeftAnimation,
            -transform.right,
            highDiveDistance,
            highDiveHeight
        );
    }

    public void DiveHighRight()
    {
        Dive(
            highRightAnimation,
            transform.right,
            highDiveDistance,
            highDiveHeight
        );
    }

    private void Dive(
        string animationName,
        Vector3 direction,
        float distance,
        float height
    )
    {
        if (hasDived)
            return;

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

    private IEnumerator DiveMovement(
        Vector3 direction,
        float distance,
        float height
    )
    {
        Vector3 initialPos = transform.position;

        Vector3 targetPos =
            initialPos +
            (direction * distance);

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

    public void ResetGoalkeeper()
    {
        StopAllCoroutines();

        hasDived = false;

        transform.position = startPosition;

        animator.Play(idleAnimation);
    }
}
