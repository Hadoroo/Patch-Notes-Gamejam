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

        // ON HIT SOUND
        if (AudioManager.Instance != null)
        {
            // Play clip 0 [HIT]
            AudioManager.Instance.PlaySoundEffect(0);
        }
       

        health -= damage;
       
        if (health <= 0f)
        {


            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}
