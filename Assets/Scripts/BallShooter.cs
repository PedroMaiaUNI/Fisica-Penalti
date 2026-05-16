using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BallShooter : MonoBehaviour
{
    public Rigidbody rb;

    public Image powerFill;
    public Image directionFill;
    public Image heightFill;

    [Header("UI")]
    public Slider powerSlider;
    public Slider directionSlider;
    public Slider heightSlider;

    enum ShootState
    {
        Power,
        Direction,
        Height,
        Done
    }

    ShootState currentState;

    float powerValue;
    float directionValue;
    float heightValue;

    bool increasing = true;

    float sliderSpeed = 1.5f;

    bool kicked = false;

    Vector3 startPos;
    Quaternion startRot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        startRot = transform.rotation;

        ResetShotSystem();
    }

    void Update()
    {
        UpdateBarColors();
        
        if (!kicked)
        {
            HandleCurrentSlider();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ConfirmCurrentValue();
            }
        }

        // Reset manual
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBall();
        }
    }

    void HandleCurrentSlider()
    {
        Slider activeSlider = null;

        switch (currentState)
        {
            case ShootState.Power:
                activeSlider = powerSlider;
                break;

            case ShootState.Direction:
                activeSlider = directionSlider;
                break;

            case ShootState.Height:
                activeSlider = heightSlider;
                break;
        }

        if (activeSlider == null)
            return;

        float value = activeSlider.value;

        if (increasing)
            value += sliderSpeed * Time.deltaTime;
        else
            value -= sliderSpeed * Time.deltaTime;

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

    void ConfirmCurrentValue()
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

    void ShootBall()
    {
        kicked = true;

        float power = Mathf.Lerp(5f, 30f, powerValue);

        float horizontal = Mathf.Lerp(-1f, 1f, directionValue);

        float vertical = Mathf.Lerp(0.2f, 1.2f, heightValue);

        Vector3 shootDirection =
            transform.forward +
            (transform.right * horizontal) +
            (transform.up * vertical);

        shootDirection.Normalize();

        rb.AddForce(shootDirection * power, ForceMode.Impulse);
    }

    void ResetBall()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = startPos;
        transform.rotation = startRot;

        rb.Sleep();

        ResetShotSystem();
    }

    void ResetShotSystem()
    {
        kicked = false;

        increasing = true;

        currentState = ShootState.Power;

        powerValue = 0;
        directionValue = 0.5f;
        heightValue = 0;

        powerSlider.value = 0;
        directionSlider.value = 0.5f;
        heightSlider.value = 0;
    }

    void UpdateBarColors()
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
}
