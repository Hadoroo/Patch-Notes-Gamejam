using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    public int scoreValue = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Destroy(gameObject);

            // Add Score each death
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddPoints(scoreValue);
            }

        }
    }
}
