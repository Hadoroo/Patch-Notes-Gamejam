using UnityEngine;

public class ArrowLookAtMouse : MonoBehaviour
{
    public Transform player; // lingkaran (pusatnya)
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Ambil posisi mouse di world
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // Hitung arah dari player ke mouse
        Vector3 direction = mousePos - player.position;

        // Hitung sudut rotasi
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set rotasi arrow (di Unity default "kanan" = 0 derajat, kalau segitiga menghadap atas perlu offset -90)
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // Posisikan arrow tetap di keliling lingkaran
        float radius = 0.8f; // jarak dari player ke arrow (atur sesuai ukuran lingkaran)
        transform.position = player.position + direction.normalized * radius;
    }
}
