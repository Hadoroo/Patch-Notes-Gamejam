using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveDirection;
    void Start()
    {    
        Camera mainCamera = Camera.main;
        float randomX = Random.Range(0.1f, 0.9f);
        float randomY = Random.Range(0.1f, 0.9f);
        Vector3 targetPointInScreen = new Vector3(Screen.width * randomX, Screen.height * randomY, mainCamera.nearClipPlane);

        Vector2 targetWorldPosition = mainCamera.ScreenToWorldPoint(targetPointInScreen);
        moveDirection = (targetWorldPosition - (Vector2)transform.position).normalized;
    }
    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }


    private void OnCollisionEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit an obstacle");
            PlayerHealth.Instance.TakeDamage();
            // Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}