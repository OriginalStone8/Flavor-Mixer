using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText, highScoreText;
    private int currentScore;

    private void Awake() 
    {
        Instance = this;
    }

    private void Start() 
    {
        MovementManager.Instance.OnMerge += AddMergeScore;
        currentScore = 0;
        UpdateScoreText();
    }

    private void AddMergeScore(object sender, MovementManager.OnMergeEventArgs e)
    {
        ModifyScore(e.MergedBlock.GetIceCreamType().scorePerMerge);
    }

    public void SaveHighScore(int score)
    {
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }

    public void UpdateScoreText()
    {
        scoreText.text = currentScore.ToString();
        highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    public void ModifyScore(int amount)
    {
        currentScore += amount;
        SaveHighScore(currentScore);
        UpdateScoreText();
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }

    public void ResetScore()
    {
        currentScore = 0;
    }
}
