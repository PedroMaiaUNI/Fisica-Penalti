using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PenaltySessionController : MonoBehaviour
{
    public static PenaltySessionController Instance;

    public GoalkeeperController goalkeeper;

    [Header("Penalty Indicators")]
    public Image[] attempts;

    [Header("References")]
    public BallShooter ballShooter;

    [Header("Menus")]
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    [Header("Tela de Pausa")]
    public GameObject pausa;

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

        if (pausa != null)
            pausa.SetActive(false);
    }

    private void Update()
    {
        if (gameFinished)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            AlternarPausa();
        }
    }

    public void AlternarPausa()
    {
        if (pausa == null)
            return;

        bool pausado = !pausa.activeSelf;

        pausa.SetActive(pausado);
        pausa.transform.SetAsLastSibling();

        Time.timeScale = pausado ? 0f : 1f;
    }

    public bool IsGameFinished()
    {
        return gameFinished;
    }

    public bool IsPaused(){
        return pausa != null && pausa.activeSelf;
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
        if (gameFinished || shotResolved)
            return;

        shotResolved = true;

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
        if (gameFinished || shotResolved)
            return;

        shotResolved = true;

        if (currentAttempt < attempts.Length)
        {
            attempts[currentAttempt].color = Color.red;
        }

        Defeat();
    }

    private IEnumerator PrepareNextShot()
    {
        yield return new WaitForSeconds(1.5f);

        if(goalkeeper != null)
        {
            goalkeeper.ResetGoalkeeper();
        }

        if (ballShooter != null)
        {
            ballShooter.ResetBall();
        }
    }

    private void Defeat()
    {
        gameFinished = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySave();
        }

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
