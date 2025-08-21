using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] Button playAgainButton;
    [SerializeField] GameObject gameOverPanel;
    float currentScore = 0f;

    void Start()
    {
        Time.timeScale = 1f;
        StartGame();
    }

    void StartGame()
    {
        gameOverPanel.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void AddScore(float amount)
    {
        currentScore += amount;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Score: " + scoreText.text;
    }

    void Update()
    {
        scoreText.text = currentScore.ToString();
    }
}
