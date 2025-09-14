using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Camera cam;

    Collider2D col;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider2D>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewportPos = cam.WorldToViewportPoint(transform.position);

        // cek kalau keluar layar
        if (viewportPos.x < 0f || viewportPos.x > 1f || viewportPos.y < 0f || viewportPos.y > 1f)
        {
            // kasih tahu FixedAspect arah mana keluar
            FixedAspect.Instance.ExpandWindow(viewportPos);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayRandomBulletSound();
            }

            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null) {
                enemyHealth.TakeDamage(25f); // contoh damage
            }
            Destroy(gameObject); // hancurkan peluru setelah mengenai musuh
        }
    }
}
