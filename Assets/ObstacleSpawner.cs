using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnInterval = 40f;
    public float spawnMargin = 1.0f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            SpawnObstacle();
        }
    }

    private void SpawnObstacle()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogError("Obstacle Prefab belum diatur di Spawner!");
            return;
        }

        Vector2 screenBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector2 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

        Vector2 spawnPosition = Vector2.zero;

        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: // Atas
                spawnPosition = new Vector2(Random.Range(screenBottomLeft.x, screenTopRight.x), screenTopRight.y + spawnMargin);
                break;
            case 1: // Bawah
                spawnPosition = new Vector2(Random.Range(screenBottomLeft.x, screenTopRight.x), screenBottomLeft.y - spawnMargin);
                break;
            case 2: // Kiri
                spawnPosition = new Vector2(screenBottomLeft.x - spawnMargin, Random.Range(screenBottomLeft.y, screenTopRight.y));
                break;
            case 3: // Kanan
                spawnPosition = new Vector2(screenTopRight.x + spawnMargin, Random.Range(screenBottomLeft.y, screenTopRight.y));
                break;
        }

        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }
}