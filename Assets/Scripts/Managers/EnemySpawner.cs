using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Area Corners (World Coordinates)")]
    public Vector2 cornerA = new Vector2(-5f, -5f);
    public Vector2 cornerB = new Vector2(5f, 5f);

    [Header("Spawn Timing")]
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 3f;

    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public int maxEnemies = 10;

    private float spawnTimer;
    private int currentEnemies = 0;

    void Start()
    {
        ResetSpawnTimer();
    }

    void Update()
    {
        if (currentEnemies >= maxEnemies)
            return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            ResetSpawnTimer();
        }
    }

    void ResetSpawnTimer()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void SpawnEnemy()
    {
        Vector2 randomPos = GetRandomPosition();

        GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);

        currentEnemies++;

        EnemyDeathNotifier notifier = enemy.AddComponent<EnemyDeathNotifier>();
        notifier.spawner = this;
    }

    Vector2 GetRandomPosition()
    {
        float minX = Mathf.Min(cornerA.x, cornerB.x);
        float maxX = Mathf.Max(cornerA.x, cornerB.x);

        float minY = Mathf.Min(cornerA.y, cornerB.y);
        float maxY = Mathf.Max(cornerA.y, cornerB.y);

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        return new Vector2(x, y);
    }

    public void EnemyDied()
    {
        currentEnemies = Mathf.Max(0, currentEnemies - 1);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 topLeft     = new Vector3(Mathf.Min(cornerA.x, cornerB.x), Mathf.Max(cornerA.y, cornerB.y));
        Vector3 topRight    = new Vector3(Mathf.Max(cornerA.x, cornerB.x), Mathf.Max(cornerA.y, cornerB.y));
        Vector3 bottomLeft  = new Vector3(Mathf.Min(cornerA.x, cornerB.x), Mathf.Min(cornerA.y, cornerB.y));
        Vector3 bottomRight = new Vector3(Mathf.Max(cornerA.x, cornerB.x), Mathf.Min(cornerA.y, cornerB.y));

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
