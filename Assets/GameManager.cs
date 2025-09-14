using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("UI Game Over")]
    public GameObject gameOverPanel; // Panel UI untuk Game Over
    public Text scoreText;
    void Start()
    {

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

        Debug.Log("Game Over!");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        int finalScore = (int)time;
        if (scoreText != null)
        {
            scoreText.text = finalScore.ToString() + " POINTS";
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
}
    

