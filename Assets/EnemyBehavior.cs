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

    private bool isDashing = false, isCharging = false, hasExploded = false, isWalking = false;
    private Vector2 dashTarget;

    public GameObject projectilePrefab;

    public Animator animator;

    public float orbitBaseSpeed = 100f; // orbit rotation speed in degrees/sec
    public float approachSpeed = 0.5f; // speed to decrease orbit radius
    private float orbitRadius;
    private float orbitAngle; //
     public AudioSource enemyAudioSource;
    public AudioClip explosionSound;
    public AudioClip gunnerShootSound;
    private bool hasPlayedSound = false;

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
                animator.SetBool("isWalking", true);
                break;

            case EnemyType.Orbiter:
                OrbitPlayer();
                if (hasExploded)
                {
                    StartCoroutine(DestroyAfterDelay(2f));
                }
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
        FlipSprite(transform.position.x - player.position.x);
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    void OrbitPlayer()
    {
        if (hasExploded) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > 4f)
        {
            animator.SetBool("isWalking", true);
            // Dekat dulu sampai radius tertentu
            MoveTowardsPlayer(moveSpeed);
            orbitRadius = distanceToPlayer;
        }
        else
        {
            animator.SetBool("isWalking", false); 
            animator.SetBool("isOrbiting", true);
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

            if (distance < 0.01f)
            {
                transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Transform explosionRadius = transform.Find("ExplosionRadius");
                explosionRadius.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                explosionRadius.gameObject.GetComponent<Collider2D>().enabled = true;
                rb.linearVelocity = Vector2.zero;
                hasExploded = true;
                if (enemyAudioSource != null && explosionSound != null && !hasPlayedSound)
                {
                    enemyAudioSource.PlayOneShot(explosionSound);
                    hasPlayedSound = true;
                }
            }
            else
            {
                Vector2 direction = toTarget.normalized;
                rb.linearVelocity = direction * Mathf.Min(moveSpeed * 5f, distance / Time.deltaTime);
            }
        }
    }

    void GunnerBehavior()
    {
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        RotateTowards(direction);
        FlipSprite(transform.position.x - player.position.x);

        animator.SetBool("isWalking", distanceToPlayer >= 3f);

        // Movement
        if (distanceToPlayer < 3f)
        {
            rb.linearVelocity = Vector2.zero;
            isWalking = false;
        }
        else
        {
            rb.linearVelocity = direction * moveSpeed;
            isWalking = true;

        }



        // Shooting
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot(transform.localScale.x);
            shootTimer = shootCooldown;
        }
    }

    void DasherBehavior()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (isCharging)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isCharging", true);
            rb.linearVelocity = Vector2.zero;
            FlipSprite(transform.position.x - player.position.x);
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
                animator.SetBool("isWalking", true);
                FlipSprite(transform.position.x - player.position.x);
                RotateTowards((Vector2)player.position - (Vector2)transform.position);
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

    void Shoot(float scaleX)
    {
        // Shooting Sound
        if (enemyAudioSource != null && gunnerShootSound != null)
        {
            enemyAudioSource.PlayOneShot(gunnerShootSound);
        }

        GameObject bullet = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        if (scaleX < 0)
        {
             rbBullet.linearVelocity = transform.right * bulletSpeed; // pakai velocity
        }
        else
        {
             rbBullet.linearVelocity = -transform.right * bulletSpeed; // pakai velocity
        }
        Destroy(bullet, 2f);
    }

    void RotateTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (transform.localScale.x > 0)
        {
            rb.rotation = angle - 180f;
        }
        else
        {
            rb.rotation = angle;
        }
    }

    void FlipSprite(float distance)
    {
        if (distance < 0)
        {
            transform.localScale = new Vector3(-0.3156904f, 0.3156904f, 0.3156904f);
        }
        else
        {
            transform.localScale = new Vector3(0.3156904f, 0.3156904f, 0.3156904f); ;
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
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
