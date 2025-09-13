using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        // Implement game over logic here
        Debug.Log("Game Over!");
    }
}
