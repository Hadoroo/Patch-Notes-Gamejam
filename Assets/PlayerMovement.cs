using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f, bulletSpeed = 10f;

    public Transform arrow;

    public GameObject projectilePrefab;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
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

        // Batas kiri/kanan/atas/bawah (pakai setengah ukuran)
        float halfW = worldWidth / 2f;
        float halfH = worldHeight / 2f;

        Vector2 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, -halfW, halfW);
        pos.y = Mathf.Clamp(pos.y, -halfH, halfH);

        rb.position = pos;
    }
}
