using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject basicEnemyPrefab, dasherEnemyPrefab, gunnerEnemyPrefab, orbiterEnemyPrefab;
    public float spawnInterval = 5f;
    private float timer;
    public float minSpawnInterval = 1f; // batas minimal interval
    public float decreaseStep = 0.5f; // berapa detik dikurangi setiap 30 detik

    float worldWidth, worldHeight, time;

    void Start()
    {
        worldWidth = FixedAspect.Instance.CurrentWidthWorld;
        worldHeight = FixedAspect.Instance.CurrentHeightWorld;
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
        time = GameManager.Instance.GetTime();
        UpdateSpawnInterval();
    }

    void SpawnEnemy()
    {
        // pilih prefab musuh random
        GameObject[] enemies = { basicEnemyPrefab, dasherEnemyPrefab, gunnerEnemyPrefab, orbiterEnemyPrefab };
        GameObject enemyPrefab = enemies[Random.Range(0, enemies.Length)];

        // tentukan posisi spawn random
        float x = Random.Range(-worldWidth / 2f, worldWidth / 2f);
        float y = Random.Range(-worldHeight / 2f, worldHeight / 2f);
        Vector2 spawnPos = new Vector2(x, y);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    void UpdateSpawnInterval()
    {
        int curTime = (int)(time / 30f);

        spawnInterval = Mathf.Max(minSpawnInterval, 4f - curTime * decreaseStep);

    }
}
