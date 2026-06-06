using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public GoalkeeperController goalkeeper;

    [Header("UI")]
    public Slider powerSlider;
    public Slider directionSlider;
    public Slider heightSlider;

    public Image powerFill;
    public Image directionFill;
    public Image heightFill;

    [Header("Slider Speeds")]
    public float powerSliderSpeed = 1.5f;
    public float directionSliderSpeed = 1.5f;
    public float heightSliderSpeed = 1.5f;

    [Header("Power Settings")]
    public float minPower = 5f;
    public float maxPower = 30f;

    [Header("Direction Settings")]
    public float minHorizontal = -1f;
    public float maxHorizontal = 1f;

    [Header("Height Settings")]
    public float minVertical = 0.2f;
    public float maxVertical = 1.2f;

    [Header("Rules")]
    public float shotTimeoutSeconds = 5f;

    enum ShootState
    {
        Power,
        Direction,
        Height,
        Done
    }

    private ShootState currentState;

    private float powerValue;
    private float directionValue;
    private float heightValue;

    private bool increasing = true;
    private bool kicked = false;

    private Vector3 startPos;
    private Quaternion startRot;

    private Coroutine shotCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        startRot = transform.rotation;

        ResetShotSystem();
    }

    private void Update()
    {
        if (PenaltySessionController.Instance != null)
        {
            if (PenaltySessionController.Instance.IsGameFinished())
                return;

            if (PenaltySessionController.Instance.IsPaused())
                return;
        }

        UpdateBarColors();

        if (!kicked)
        {
            HandleCurrentSlider();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ConfirmCurrentValue();
            }
        }
    }

    private void HandleCurrentSlider()
    {
        Slider activeSlider = null;
        float currentSpeed = 1f;

        switch (currentState)
        {
            case ShootState.Power:
                activeSlider = powerSlider;
                currentSpeed = powerSliderSpeed;
                break;

            case ShootState.Direction:
                activeSlider = directionSlider;
                currentSpeed = directionSliderSpeed;
                break;

            case ShootState.Height:
                activeSlider = heightSlider;
                currentSpeed = heightSliderSpeed;
                break;
        }

        if (activeSlider == null)
            return;

        float value = activeSlider.value;

        if (increasing)
            value += currentSpeed * Time.deltaTime;
        else
            value -= currentSpeed * Time.deltaTime;

        if (value >= 1f)
        {
            value = 1f;
            increasing = false;
        }

        if (value <= 0f)
        {
            value = 0f;
            increasing = true;
        }

        activeSlider.value = value;
    }

    private void ConfirmCurrentValue()
    {
        switch (currentState)
        {
            case ShootState.Power:
                powerValue = powerSlider.value;
                currentState = ShootState.Direction;
                break;

            case ShootState.Direction:
                directionValue = directionSlider.value;
                currentState = ShootState.Height;
                break;

            case ShootState.Height:
                heightValue = heightSlider.value;
                ShootBall();
                currentState = ShootState.Done;
                break;
        }
    }

    public void ShootBall()
    {
        kicked = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayKick();
        }

        if (PenaltySessionController.Instance != null)
        {
            PenaltySessionController.Instance.BeginShot();
        }

        if (shotCoroutine != null)
        {
            StopCoroutine(shotCoroutine);
        }

        shotCoroutine = StartCoroutine(ShotTimeout());

        if (goalkeeper != null)
        {
            goalkeeper.DecideAction(
                directionValue,
                heightValue
            );
        }

        float power =
            Mathf.Lerp(
                minPower,
                maxPower,
                powerValue
            );

        float horizontal =
            Mathf.Lerp(
                minHorizontal,
                maxHorizontal,
                directionValue
            );

        float vertical =
            Mathf.Lerp(
                minVertical,
                maxVertical,
                heightValue
            );

        Vector3 shootDirection =
            transform.forward +
            (transform.right * horizontal) +
            (transform.up * vertical);

        shootDirection.Normalize();

        rb.AddForce(
            shootDirection * power,
            ForceMode.Impulse
        );
    }

    public void ResetBall()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = startPos;
        transform.rotation = startRot;

        rb.Sleep();

        ResetShotSystem();
    }

    private void ResetShotSystem()
    {
        kicked = false;

        increasing = true;

        currentState = ShootState.Power;

        powerValue = 0f;
        directionValue = 0.5f;
        heightValue = 0f;

        powerSlider.value = 0f;
        directionSlider.value = 0.5f;
        heightSlider.value = 0f;
    }

    private void UpdateBarColors()
    {
        Color activeColor = Color.yellow;
        Color inactiveColor = Color.white;

        powerFill.color =
            currentState == ShootState.Power
            ? activeColor
            : inactiveColor;

        directionFill.color =
            currentState == ShootState.Direction
            ? activeColor
            : inactiveColor;

        heightFill.color =
            currentState == ShootState.Height
            ? activeColor
            : inactiveColor;
    }

    private System.Collections.IEnumerator ShotTimeout()
    {
        yield return new WaitForSeconds(
            shotTimeoutSeconds
        );

        if (
            PenaltySessionController.Instance != null &&
            !PenaltySessionController.Instance.ShotResolved()
        )
        {
            PenaltySessionController.Instance.RegisterMiss();
        }
    }
}
