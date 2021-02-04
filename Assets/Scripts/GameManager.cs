using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public GameObject scoreCounter;
    private TMP_Text scoreCounterTMP;

    void Start()
    {
        // Get TMP component from our scorecounter gameobject
        scoreCounterTMP = scoreCounter.GetComponent<TMP_Text>();
    }

    // Function called from outside this class, when called update the score and score ui
    public void UpdateScore(int amount)
    {
        score += amount;
        scoreCounterTMP.text = score.ToString();

    }
    
    void Update()
    {
    }
}
