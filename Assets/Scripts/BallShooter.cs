using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BallShooter : MonoBehaviour
{
    public Rigidbody rb;

    private float stoppedTime = 0f;
    private float shotTimer = 0f;
    private float missCheckDelay = 0.5f;

    public Image powerFill;
    public Image directionFill;
    public Image heightFill;

    private Coroutine shotCoroutine;

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

    private bool missChecked = false;
    private bool wasShot = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        startRot = transform.rotation;

        ResetShotSystem();
    }

    void Update()
    {
        // Impede interação após vitória ou derrota
        if (PenaltySessionController.Instance != null &&
            PenaltySessionController.Instance.IsGameFinished())
        {
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

        // Reset manual APENAS após a cobrança ter sido resolvida
        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (PenaltySessionController.Instance != null &&
                PenaltySessionController.Instance.ShotResolved())
            {
                ResetBall();
            }
        }
        */

        // Detecta erro quando a bola para
        if (wasShot && !missChecked)
        {
            if (rb.linearVelocity.magnitude < 0.1f)
            {
                stoppedTime += Time.deltaTime;

                if (stoppedTime >= 1f)
                {
                    if (!PenaltySessionController.Instance.ShotResolved())
                    {
                        missChecked = true;
                        PenaltySessionController.Instance.RegisterMiss();
                    }
                }
            }
            else
            {
                stoppedTime = 0f;
            }
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

    public void ShootBall()
    {
        kicked = true;

        wasShot = true;
        missChecked = false;
        shotTimer = 0f;

        PenaltySessionController.Instance.BeginShot();

        if (shotCoroutine != null)
        {
            StopCoroutine(shotCoroutine);
        }

        shotCoroutine = StartCoroutine(ShotTimeout());

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

    public void ResetBall()
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

        wasShot = false;
        missChecked = false;

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

    private System.Collections.IEnumerator ShotTimeout()
    {
        yield return new WaitForSeconds(5f);

        if (!PenaltySessionController.Instance.ShotResolved())
        {
            PenaltySessionController.Instance.RegisterMiss();
        }
    }
}
