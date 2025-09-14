using UnityEngine;

public class FixedAspect : SingletonMonoBehaviour<FixedAspect>
{
    private float currentWidth;
    private float currentHeight;

    public float maxWidth = 1280f;
    public float maxHeight = 720f;

    public float CurrentWidthWorld => currentWidth / 100f;   // scale px → world
    public float CurrentHeightWorld => currentHeight / 100f;

    bool isGameOver = false;

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

        currentHeight = maxHeight;
        currentWidth = maxWidth;

        targetWidth = currentWidth;
        targetHeight = currentHeight;

        Screen.SetResolution((int)currentWidth, (int)currentHeight, false);
        UpdateCameraSize();
    }

    void Update()
    {
        isGameOver = GameManager.Instance.IsGameOver();

        if (!isGameOver)
        {
            HandleResize();
        }
        else
        {
            // Kunci ukuran saat game over
            targetWidth = maxWidth;
            targetHeight = maxHeight;
        }

    }

    void HandleResize()
    {
        // Target otomatis mengecil
        targetWidth -= shrinkSpeed * Time.deltaTime;
        targetHeight -= shrinkSpeed * Time.deltaTime;

        targetWidth = Mathf.Max(100f, targetWidth);
        targetHeight = Mathf.Max(100f, targetHeight);

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

        if (viewportPos.x < 0f || viewportPos.x > 1f) // kiri atau kanan
            targetWidth += growAmount;
        if (viewportPos.y < 0f || viewportPos.y > 1f) // bawah atau atas
            targetHeight += growAmount;

        // Clamp supaya tidak melebihi max
        targetWidth = Mathf.Clamp(targetWidth, 100f, maxWidth);
        targetHeight = Mathf.Clamp(targetHeight, 100f, maxHeight);
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
