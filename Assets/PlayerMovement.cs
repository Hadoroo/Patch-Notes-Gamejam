using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f, bulletSpeed = 10f;

    public Transform arrow;

    public GameObject projectilePrefab;

    // FireRate and Cooldown
    public float fireRate = 0.2f; // Time in seconds between shots
    private float nextFireTime = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Move();

        // Input Shoot adn check cooldown
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && Time.time >= nextFireTime)
        {
            Shoot();

            // Set Cooldown
            nextFireTime = Time.time + fireRate;
        }
        
    }

    void Move()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(inputX, inputY).normalized;
        rb.linearVelocity = movement * speed;

        ClampToWindow();
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(projectilePrefab, arrow.position, arrow.rotation);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = arrow.up * bulletSpeed;
        Destroy(bullet, 0.5f);
    }

    void ClampToWindow()
    {
        // Ambil ukuran window dari FixedAspect
        float worldWidth = FixedAspect.Instance.CurrentWidthWorld;
        float worldHeight = FixedAspect.Instance.CurrentHeightWorld;

        // Ambil setengah ukuran player (asumsi BoxCollider2D)
        float halfPlayerW = 0.5f;
        float halfPlayerH = 0.5f;

        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        if (bc != null)
        {
            halfPlayerW = bc.size.x * 0.5f;
            halfPlayerH = bc.size.y * 0.5f;
        }

        // Batas kiri/kanan/atas/bawah dikurangi setengah ukuran player
        float halfW = worldWidth / 2f - halfPlayerW;
        float halfH = worldHeight / 2f - halfPlayerH;

        Vector2 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, -halfW, halfW);
        pos.y = Mathf.Clamp(pos.y, -halfH, halfH);

        rb.position = pos;
    }

}
