using UnityEngine;
using System.Collections;

public enum EnemyType
{
    Basic,
    Dasher,
    Gunner,
    Orbiter
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

    public float orbitBaseSpeed = 100f; // orbit rotation speed in degrees/sec
    public float approachSpeed = 0.5f; // speed to decrease orbit radius
    private float orbitRadius;
    private float orbitAngle; //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyCollider = GetComponent<Collider2D>();

        if (enemyType == EnemyType.Orbiter)
        {
            orbitRadius = Vector2.Distance(transform.position, player.position);
            Vector2 dir = (Vector2)transform.position - (Vector2)player.position;
            orbitAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        switch (enemyType)
        {
            case EnemyType.Basic:
                MoveTowardsPlayer(moveSpeed);
                break;

            case EnemyType.Orbiter:
                OrbitPlayer();
                break;

            case EnemyType.Gunner:
                GunnerBehavior();
                break;

            case EnemyType.Dasher:
                DasherBehavior();
                break;
        }
    }

    void MoveTowardsPlayer(float speed)
    {
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * speed;
        RotateTowards(direction);
    }

    void OrbitPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > 4f)
        {
            // Dekat dulu sampai radius tertentu
            MoveTowardsPlayer(moveSpeed);
            orbitRadius = distanceToPlayer;
        }
        else
        {
            // Radius mengecil perlahan
            orbitRadius = Mathf.Max(0.5f, orbitRadius - approachSpeed * Time.deltaTime);

            // Update orbit angle (kalikan moveSpeed untuk menambah orbit speed)
            orbitAngle += orbitBaseSpeed * moveSpeed * Time.deltaTime;
            if (orbitAngle > 360f) orbitAngle -= 360f;

            float rad = orbitAngle * Mathf.Deg2Rad;
            Vector2 targetPos = (Vector2)player.position + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * orbitRadius;

            // Hitung velocity proporsional ke targetPos
            Vector2 toTarget = targetPos - (Vector2)transform.position;
            float distance = toTarget.magnitude;

            if (distance > 0.01f)
            {
                Vector2 direction = toTarget.normalized;
                rb.linearVelocity = direction * Mathf.Min(moveSpeed * 5f, distance / Time.deltaTime);
                RotateTowards(direction);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    void GunnerBehavior()
    {
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Movement
        if (distanceToPlayer < 3f)
            rb.linearVelocity = Vector2.zero;
        else
            rb.linearVelocity = direction * moveSpeed;

        RotateTowards(direction);

        // Shooting
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootCooldown;
        }
    }

    void DasherBehavior()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (isCharging)
        {
            rb.linearVelocity = Vector2.zero;
            RotateTowards((Vector2)player.position - (Vector2)transform.position);
        }
        else if (!isDashing && !isCharging)
        {
            if (distanceToPlayer < 3f)
            {
                StartCoroutine(DashAfterDelay(2f));
            }
            else if (!isDashing)
            {
                MoveTowardsPlayer(moveSpeed);
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
        rb.linearVelocity = dashDirection * moveSpeed * 3f;
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
        Destroy(bullet, 2f);
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
            PlayerHealth.Instance.TakeDamage();
        }
    }

}
