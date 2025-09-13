using UnityEngine;
using System.Collections;

public enum EnemyType
{
    Basic,
    Dasher,
    Gunner
}

public class EnemyBehavior : MonoBehaviour
{
    public float moveSpeed = 2f, shootCooldown = 2f, bulletSpeed = 5f, shootTimer = 0f;
    Collider2D enemyCollider;

    public Rigidbody2D rb;

    public EnemyType enemyType;

    Transform player;

    private bool isDashing = false, isCharging = false;
    private Vector2 dashTarget;

    public GameObject projectilePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (enemyType == EnemyType.Basic)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
            RotateTowards(direction);
        }
        if (enemyType == EnemyType.Gunner)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Movement
            if (distanceToPlayer < 3f)
                rb.linearVelocity = Vector2.zero;
            else
                rb.linearVelocity = direction * moveSpeed;

            RotateTowards(direction);

            // Shooting timer
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                Shoot();
                shootTimer = shootCooldown; // reset timer
            }
        }
        if (enemyType == EnemyType.Dasher)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (isDashing)
            {
                Vector2 direction = (dashTarget - (Vector2)transform.position).normalized;
            }
            else if (isCharging) // saat delay
            {
                RotateTowards((player.position - transform.position).normalized);
                rb.linearVelocity = Vector2.zero; // berhenti
            }
            else // normal movement
            {
                if (distanceToPlayer < 3f)
                {
                    StartCoroutine(DashAfterDelay(2f));
                }
                else if (!isDashing)
                {
                    Vector2 direction = (player.position - transform.position).normalized;
                    rb.linearVelocity = direction * moveSpeed;
                    RotateTowards(direction);
                }
            }
        }
    }

    IEnumerator DashAfterDelay(float delay)
    {
        isCharging = true;
        // lock posisi terakhir player
        rb.linearVelocity = Vector2.zero;   // berhenti saat charging

        yield return new WaitForSeconds(delay);

        dashTarget = player.position;
        isCharging = false;
        isDashing = true;

        // Hitung arah dash
        Vector2 dashDirection = (dashTarget - (Vector2)transform.position).normalized;
        rb.linearVelocity = dashDirection * (moveSpeed * 3f); // kecepatan dash

        // Tunggu selama dashDuration
        float dashDuration = 2f;
        yield return new WaitForSeconds(dashDuration);

        Destroy(gameObject); // hilang setelah dash selesai
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        rbBullet.linearVelocity = transform.up * bulletSpeed; // pakai velocity
        Destroy(bullet, 1f);
    }

    void RotateTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
