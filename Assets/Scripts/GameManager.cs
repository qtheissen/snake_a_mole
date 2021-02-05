using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public GameObject scoreCounter;
    public GameObject timer;
    private TMP_Text scoreCounterTMP;
    private TMP_Text timerTMP;

    [HideInInspector] // Bool to keep track of game over (not to be edited in inspector)
    public bool gameOver = false;
    
    // For end screen animation
    public EndScreen endScreenUI;

    // This is the total time left in seconds for the game
    public float totalTimeSeconds = 100f;

    // How much time you have left at the start of the game
    public float gameLengthSeconds = 100f;

    void Start()
    {
        // Get TMP component from our scorecounter gameobject
        scoreCounterTMP = scoreCounter.GetComponent<TMP_Text>();

        timerTMP = timer.GetComponent<TMP_Text>();
    }

    // Function called from outside this class, when called update the score and score ui
    public void AddScore(int amount)
    {
        score += amount;
        scoreCounterTMP.text = score.ToString();

    }
    
    void Update()
    {
        Timer();
        if (!gameOver) // Only update timer when game is not over
        {
            UpdateTimer();
        }

        if (totalTimeSeconds < 0 && !gameOver) // Dont game over if already game over
        {
            gameOver = true;
            endScreenUI.ShowEndScreen(score); // Game over ui

            totalTimeSeconds = 0; // Set timer to 0 so it doesn't go negative
        }
    }

    void Timer()
    {
        // Divide the total time in seconds into minutes and seconds
        int timeMinutes = Mathf.FloorToInt (totalTimeSeconds / 60);
        int timeSeconds = Mathf.FloorToInt (totalTimeSeconds % 60);

        // Add a zero before the minute if the timer has less than 10 minutes left
        string timeMinutesString;
        if (timeMinutes < 10) timeMinutesString = "0" + timeMinutes;
        else timeMinutesString = timeMinutes.ToString();
        
        // Add a zero before the second if the timer has less than 10 seconds left
        string timeSecondsString;
        if (timeSeconds < 10) timeSecondsString = "0" + timeSeconds;
        else timeSecondsString = timeSeconds.ToString();

        // Make a complete string with a separator for the ui to use
        string timerString = timeMinutesString + ":" + timeSecondsString;

        timerTMP.text = timerString;
    }

    void UpdateTimer()
    {
        totalTimeSeconds -= Time.deltaTime;
    }
}
