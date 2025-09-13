using UnityEngine;

public class EnemyBulletBehavior : MonoBehaviour
{
    Collider2D col;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth.Instance.TakeDamage();
            Destroy(gameObject); 
        }
    }
}
