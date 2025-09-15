using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    public GameObject basicEnemyPrefab, dasherEnemyPrefab, gunnerEnemyPrefab, orbiterEnemyPrefab;
    public float spawnInterval = 5f;
    private float timer;
    public float minSpawnInterval = 1f; // batas minimal interval
    public float decreaseStep = 0.5f; // berapa detik dikurangi setiap 30 detik

    Transform player;

    float worldWidth, worldHeight, time;

    void Start()
    {
        worldWidth = FixedAspect.Instance.CurrentWidthWorld;
        worldHeight = FixedAspect.Instance.CurrentHeightWorld;
        timer = spawnInterval;
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        if (GameManager.Instance.IsGameOver())
        {
            return; // Hentikan eksekusi fungsi
        }
        // pilih prefab musuh random
        GameObject[] enemies = { basicEnemyPrefab, dasherEnemyPrefab, gunnerEnemyPrefab, orbiterEnemyPrefab };
        GameObject enemyPrefab = enemies[Random.Range(0, enemies.Length)];

        // posisi player
        Vector2 playerPos = player.transform.position;

        // arah random (sudut 0–360 derajat)
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        // jarak random 8–12
        float distance = Random.Range(5f, 8f);

        // posisi spawn
        Vector2 spawnPos = playerPos + direction * distance;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }


    void UpdateSpawnInterval()
    {
        int curTime = (int)(time / 30f);

        spawnInterval = Mathf.Max(minSpawnInterval, 4f - curTime * decreaseStep);

    }
}
