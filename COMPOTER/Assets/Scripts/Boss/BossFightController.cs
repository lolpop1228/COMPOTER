using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Required for TextMeshPro

public class BossFightController : MonoBehaviour
{
    [Header("Boss Setup")]
    public GameObject boss;
    public GameObject bossHealthBar;
    public GameObject bossResult;

    [Header("Timer UI")]
    public TextMeshProUGUI timerText;        // Live timer display (during fight)
    public TextMeshProUGUI resultTimerText;  // Final result display (after fight)
    public Color timerColor = Color.white;

    private float fightTimer = 0f;
    private bool timerRunning = false;
    private bool fightEnded = false;

    void Start()
    {
        // Initialize texts
        if (timerText != null)
        {
            timerText.color = timerColor;
            timerText.text = " ";
        }

        if (resultTimerText != null)
            resultTimerText.text = " ";

        // Timer doesn't start automatically — wait for StartTimer() call
        timerRunning = false;
    }

    void Update()
    {
        // --- Update Timer ---
        if (timerRunning && !fightEnded)
        {
            fightTimer += Time.deltaTime;

            int minutes = Mathf.FloorToInt(fightTimer / 60);
            int seconds = Mathf.FloorToInt(fightTimer % 60);
            int milliseconds = Mathf.FloorToInt((fightTimer * 100) % 100); // 2-digit ms

            if (timerText != null)
                timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
        }

        // --- Detect Boss Death ---
        if (boss == null && !fightEnded)
        {
            EndFight();
        }
    }

    // 🟢 Call this from another script to start the timer
    public void StartTimer()
    {
        fightTimer = 0f;
        timerRunning = true;
        fightEnded = false;

        if (timerText != null)
            timerText.text = "00:00:00";

        if (resultTimerText != null)
            resultTimerText.text = " ";

        Debug.Log("Boss fight timer started!");
    }

    // 🔴 Optionally stop the timer manually
    public void StopTimer()
    {
        timerRunning = false;
        Debug.Log("Boss fight timer stopped!");
    }

    // ⚔️ Called automatically when boss is dead
    private void EndFight()
    {
        fightEnded = true;
        timerRunning = false;

        if (bossHealthBar != null)
            bossHealthBar.SetActive(false);

        if (bossResult != null)
            bossResult.SetActive(true);

        int finalMinutes = Mathf.FloorToInt(fightTimer / 60);
        int finalSeconds = Mathf.FloorToInt(fightTimer % 60);
        int finalMilliseconds = Mathf.FloorToInt((fightTimer * 100) % 100);

        string finalTime = $"{finalMinutes:00}:{finalSeconds:00}:{finalMilliseconds:00}";

        Debug.Log($"Boss defeated! Fight duration: {finalTime}");

        // Clear live timer and show final time
        if (timerText != null)
            timerText.text = "";

        if (resultTimerText != null)
            resultTimerText.text = $"Final Time: {finalTime}";
    }
}
