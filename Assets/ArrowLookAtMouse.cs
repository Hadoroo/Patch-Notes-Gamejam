using UnityEngine;

public class ArrowLookAtMouse : MonoBehaviour
{
    public Transform player; // lingkaran (pusatnya)
    Camera cam;
    // di ArrowController (assign bodySprite lewat Inspector)
    public SpriteRenderer bodySprite; // sprite player/body, bukan arrow

    [HideInInspector] public Vector3 shootDirection;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // flip body player (bukan arrow)
        bool faceRight = mousePos.x > player.position.x;
        if (bodySprite != null)
            bodySprite.flipX = faceRight;

        // arah tembak
        shootDirection = (mousePos - player.position).normalized;
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

        // rotasi arrow (offset disesuaikan dengan sprite arrow)
        transform.rotation = Quaternion.Euler(0, 0, angle - 210f);

        // scale arrow fix (tidak usah dibolak-balik)
        transform.localScale = new Vector3(1.38f, 1.38f, 1.38f);

        // posisi arrow di radius kecil dari player
        float radius = 0.3f;
        transform.position = player.position + shootDirection * radius;
    }


}
