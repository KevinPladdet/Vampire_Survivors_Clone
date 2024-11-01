using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

    [Header("Timer References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI survivedTimeText;

    [Header("Variables")]
    private float gameTime;
    public bool stopTimer = false;
    public float enemyStatsMultiplier;
    public float spawningFrequency;

    private float timeForStatsMultiplier;

    void Update()
    {
        if (!stopTimer)
        {
            gameTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(gameTime / 60);
            int seconds = Mathf.FloorToInt(gameTime % 60);

            string textTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            timerText.text = textTime;

            timeForStatsMultiplier += Time.deltaTime;

            // Every 30 seconds the enemyStatsMultiplier will increase by 10%
            if (timeForStatsMultiplier >= 30f)
            {
                enemyStatsMultiplier *= 1.1f;
                spawningFrequency *= 0.95f;

                timeForStatsMultiplier = 0f; // Reset 30 second timer
            }
        }
    }

    // Is called when the player dies
    public void onPlayerDeath()
    {
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);

        string textTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        survivedTimeText.text = "Survived: " + textTime;
    }
}
