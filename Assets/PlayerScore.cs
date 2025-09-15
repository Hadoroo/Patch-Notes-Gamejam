using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Make this class global
    public static ScoreManager Instance { get; private set; }
    private float score = 0f;
    public int Score => Mathf.FloorToInt(score);
    public float scoreRate = 1f;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        // Set up the singleton instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        score += scoreRate * Time.deltaTime;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Dream: " + Score.ToString();
        }
    }

    // Adding score method
    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreText();
    }

    // Minus score method if needed
    public void SubtractPoints(int points)
    {
        score -= points;
        UpdateScoreText();
    }
}