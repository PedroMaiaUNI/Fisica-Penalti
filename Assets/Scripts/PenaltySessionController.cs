using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PenaltySessionController : MonoBehaviour
{
    public static PenaltySessionController Instance;

    [Header("Penalty Indicators")]
    public Image[] attempts;

    [Header("References")]
    public BallShooter ballShooter;

    [Header("Menus")]
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    private int currentAttempt = 0;

    private bool gameFinished = false;
    private bool shotResolved = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (Image attempt in attempts)
        {
            attempt.color = Color.white;
        }

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (defeatPanel != null)
            defeatPanel.SetActive(false);
    }

    public bool IsGameFinished()
    {
        return gameFinished;
    }

    public bool ShotResolved()
    {
        return shotResolved;
    }

    public void BeginShot()
    {
        shotResolved = false;
    }

    public void RegisterGoal()
    {
        if (gameFinished)
            return;

        if (shotResolved)
            return;

        shotResolved = true;

        Debug.Log("Gol registrado");

        if (currentAttempt < attempts.Length)
        {
            attempts[currentAttempt].color = Color.green;
        }

        currentAttempt++;

        if (currentAttempt >= 5)
        {
            Victory();
            return;
        }

        StartCoroutine(PrepareNextShot());
    }

    public void RegisterMiss()
    {
        if (gameFinished)
            return;

        if (shotResolved)
            return;

        shotResolved = true;

        Debug.Log("Cobrança perdida");

        if (currentAttempt < attempts.Length)
        {
            attempts[currentAttempt].color = Color.red;
        }

        Defeat();
    }

    private IEnumerator PrepareNextShot()
    {
        yield return new WaitForSeconds(1.5f);

        if (ballShooter != null)
        {
            ballShooter.ResetBall();
        }
    }

    private void Defeat()
    {
        gameFinished = true;

        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    private void Victory()
    {
        gameFinished = true;

        StartCoroutine(ShowVictory());
    }

    private IEnumerator ShowVictory()
    {
        yield return new WaitForSeconds(5f);

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}
