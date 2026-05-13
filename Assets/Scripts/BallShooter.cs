using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    public Rigidbody rb;

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

    ShootState currentState = ShootState.Power;

    float powerValue;
    float directionValue;
    float heightValue;

    bool increasing = true;

    float sliderSpeed = 1.5f;

    bool kicked = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        ResetSliders();
    }

    void Update()
    {
        if (kicked)
            return;

        HandleCurrentSlider();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ConfirmCurrentValue();
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

    void ResetSliders()
    {
        powerSlider.value = 0;
        directionSlider.value = 0.5f;
        heightSlider.value = 0;
    }
}
