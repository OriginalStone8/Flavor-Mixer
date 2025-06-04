using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText, highScoreText;
    [SerializeField] private GameObject gameOverPopup;
    [SerializeField] private float meltAnimTime;
    [SerializeField] private float animTime;

    private void Awake() 
    {
        Instance = this;
        gameOverPopup.SetActive(false);
    }

    private void Start() 
    {
        GameManagerScript.Instance.OnGameOver += HandleGameOver;
    }

    private void HandleGameOver(object sender, EventArgs e)
    {
        UpdateScoreTexts();
        PrizeTracker.Instance.SetRewardTexts();
        ContinueGame.Instance.UpdateContinueButtonInteractability();
        SceneManagement.Instance.MeltTransition(OnPopup);
    }

    private void OnPopup()
    {
        gameOverPopup.transform.localScale = new Vector3(0, 0, 0);
        gameOverPopup.SetActive(true);
        LeanTween.scale(gameOverPopup, new Vector3(1, 1, 1), animTime).setEaseOutBack();
    }

    private void UpdateScoreTexts()
    {
        scoreText.text = "Score: " + ScoreManager.Instance.GetCurrentScore().ToString();
        highScoreText.text = "High Score: " + ScoreManager.Instance.GetHighScore().ToString();
    }

    public void ClosePopup(bool gameOver)
    {
        LeanTween.scale(gameOverPopup, new Vector3(0, 0, 0), 0.2f).setEaseInBack().setOnComplete(() => 
        {
            gameOverPopup.SetActive(false);
            SceneManagement.Instance.RestartMeltTransition(gameOver);
        });
    }
}
