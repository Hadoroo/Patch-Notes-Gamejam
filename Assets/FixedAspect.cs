using UnityEngine;

public class FixedAspect : SingletonMonoBehaviour<FixedAspect>
{
    private float currentWidth = 1280f;
    private float currentHeight = 720f;

    private float targetWidth;
    private float targetHeight;

    [Header("Resize Speeds")]
    public float shrinkSpeed = 0.5f;   // bisa lebih kecil dari 1 px/frame
    public float expandSpeed = 300f;   // px/detik

    private Camera cam;

    void Start()
    {
        Screen.fullScreen = false;
        cam = Camera.main;

        targetWidth = currentWidth;
        targetHeight = currentHeight;

        Screen.SetResolution((int)currentWidth, (int)currentHeight, false);
        UpdateCameraSize();
    }

    void Update()
    {
        // Target otomatis mengecil
        targetWidth -= shrinkSpeed * Time.deltaTime;
        targetHeight -= shrinkSpeed * Time.deltaTime;

        targetWidth = Mathf.Max(200f, targetWidth);
        targetHeight = Mathf.Max(200f, targetHeight);

        // Smooth menuju target
        if ((int)currentWidth != (int)targetWidth || (int)currentHeight != (int)targetHeight)
        {
            SmoothResize();
            Screen.SetResolution((int)currentWidth, (int)currentHeight, false);
            UpdateCameraSize();
        }
    }

    void SmoothResize()
    {
        float speedX = targetWidth < currentWidth ? shrinkSpeed : expandSpeed;
        float speedY = targetHeight < currentHeight ? shrinkSpeed : expandSpeed;

        currentWidth = Mathf.MoveTowards(currentWidth, targetWidth, speedX * Time.deltaTime);
        currentHeight = Mathf.MoveTowards(currentHeight, targetHeight, speedY * Time.deltaTime);
    }

    public void ExpandWindow(Vector3 viewportPos)
    {
        float growAmount = 100f; // px tiap tembakan

        if (viewportPos.x < 0f) // kiri
            targetWidth += growAmount;
        else if (viewportPos.x > 1f) // kanan
            targetWidth += growAmount;
        else if (viewportPos.y < 0f) // bawah
            targetHeight += growAmount;
        else if (viewportPos.y > 1f) // atas
            targetHeight += growAmount;

        targetWidth = Mathf.Max(200f, targetWidth);
        targetHeight = Mathf.Max(200f, targetHeight);
    }

    void UpdateCameraSize()
    {
        if (cam != null && cam.orthographic)
        {
            float aspect = currentWidth / currentHeight;
            cam.orthographicSize = currentHeight / 200f;
        }
    }
}
