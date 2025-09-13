using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float lifetime = 0.5f;
    private Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        Destroy(gameObject, lifetime); // jaga-jaga kalau tidak keluar layar
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
}
