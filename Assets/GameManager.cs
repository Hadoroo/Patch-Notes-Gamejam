using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("UI Game Over")]
    public GameObject gameOverPanel; // Panel UI untuk Game Over
    bool isGameOver = false;
    public TMP_Text scoreText;
    void Start()
    {
        //Play BGM
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBackgroundMusic();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
    }

    void UpdateTime()
    {
        time += Time.deltaTime;
    }

    public float GetTime()
    {
        return time;
    }

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over!");


         if (AudioManager.Instance != null)
        {
            // Stop background music
            AudioManager.Instance.StopBackgroundMusic();
            
            // Play Died sound (5), Hardcode
            AudioManager.Instance.PlaySoundEffect(5); 
        }
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        int finalScore = ScoreManager.Instance.Score;
        if (scoreText != null)
        {
            scoreText.text = finalScore.ToString() + " DREAMS";
        }
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}


