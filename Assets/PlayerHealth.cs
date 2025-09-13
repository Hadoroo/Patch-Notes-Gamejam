using UnityEngine;

public class PlayerHealth : SingletonMonoBehaviour<PlayerHealth>
{

    public float health = 100f;

    float damage = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void TakeDamage()
    {
        health -= damage;
        if (health <= 0f)
        {
            GameManager.Instance.GameOver();
        }
    }
}
