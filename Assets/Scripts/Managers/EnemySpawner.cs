using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    private EnemyRunData runData;
    [SerializeField] private SceneLoader sceneLoader;

    public enum SpawnMode
    {
        FixedPoints,
        RandomInPolygon
    }

    [Header("Spawn Mode")]
    [SerializeField] private SpawnMode spawnMode = SpawnMode.RandomInPolygon;

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Header("Random Spawn Area")]
    [SerializeField] private PolygonCollider2D spawnArea;
    [SerializeField] private LayerMask blockedSpawnLayers;
    [SerializeField] private float spawnCheckRadius = 0.4f;
    [SerializeField] private int maxSpawnAttempts = 20;

    [Header("Spawn Settings")]
    [SerializeField] private GameObject spawnPointPrefab;
    [SerializeField] private int baseEnemiesPerWave = 10;
    [SerializeField] private int enemiesPerWave = 10;
    [SerializeField] private int enemyIncreasePerWave = 1;
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField] private float delayBetweenWaves = 3f;
    //[SerializeField] private int baseMaxWaves;
    [SerializeField] private int maxWaves;
    [SerializeField] private int currentWave;
    //[SerializeField] private int wavesCleared;
    private bool isSpawningWave;

    [Header("Spawn Warning")]
    [SerializeField] private float spawnWarningTime = 1f;

    [Header("Enemy Settings")]
    [SerializeField] private List<GameObject> enemyPrefabs;

    private float spawnTimer;
    private int currentEnemies = 0;
    [SerializeField] private int enemiesKilled = 0;

    private void Awake()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();

        if(spawnPoints.Count == 0)
        {
            foreach(Transform child in transform)
            {
                spawnPoints.Add(child);
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.enabled = false;
            }
        }
    }

    void Start()
    {
        runData = EnemyRunData.Instance;

        currentWave = 0; // reset per scene
        maxWaves = runData.GetWavesForCurrentScene();
        enemiesPerWave = runData.GetEnemiesPerWave(baseEnemiesPerWave); // includes persistent extra enemies

        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        currentWave = 0;
        int maxWaves = runData.GetWavesForCurrentScene();

        while (currentWave < maxWaves)
        {
            currentWave++;

            yield return StartCoroutine(SpawnWave());
            yield return new WaitUntil(() => currentEnemies <= 0);

            // Increase enemies for next wave
            enemiesPerWave += enemyIncreasePerWave;
            runData.OnWaveCleared(enemyIncreasePerWave);  // persist for next scene
        }

        // Scene fully cleared → increment scenesCleared
        runData.OnSceneCleared();

        //Load new scene
        sceneLoader.LoadRandomGameScene();

    }

    private IEnumerator SpawnWave()
    {
        isSpawningWave = true;

        int enemiesToSpawn = enemiesPerWave;

        // Used spawn positions this wave (prevents overlap)
        List<Vector2> usedPositions = new List<Vector2>();

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 spawnPos;

            if (spawnMode == SpawnMode.FixedPoints)
            {
                if (spawnPoints.Count == 0)
                    yield break;

                List<Transform> availablePoints = new List<Transform>(spawnPoints);

                Transform point = availablePoints[Random.Range(0, availablePoints.Count)];
                spawnPos = point.position;

                SpriteRenderer sr = point.GetComponent<SpriteRenderer>();
                if (sr != null) sr.enabled = true;

                yield return new WaitForSeconds(spawnWarningTime);

                if (sr != null) sr.enabled = false;
            }
            else
            {
                spawnPos = GetValidRandomPosition(usedPositions);

                GameObject marker = Instantiate(spawnPointPrefab, spawnPos, Quaternion.identity);
                yield return new WaitForSeconds(spawnWarningTime);
                Destroy(marker);
            }

            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            currentEnemies++;
            usedPositions.Add(spawnPos);

            EnemyDeathNotifier notifier = enemy.AddComponent<EnemyDeathNotifier>();
            notifier.spawner = this;

            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawningWave = false;
    }

    private Vector2 GetValidRandomPosition(List<Vector2> usedPositions)
    {
        Bounds bounds = spawnArea.bounds;

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Vector2 point = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            if (!spawnArea.OverlapPoint(point))
                continue;

            if (Physics2D.OverlapCircle(point, spawnCheckRadius, blockedSpawnLayers))
                continue;

            bool tooClose = false;
            foreach (Vector2 used in usedPositions)
            {
                if (Vector2.Distance(used, point) < spawnCheckRadius * 2f)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
                continue;

            return point;
        }

        // Fallback if all attempts fail
        return spawnArea.bounds.center;
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        currentEnemies++;
        enemiesKilled++;

        EnemyDeathNotifier notifier = enemy.AddComponent<EnemyDeathNotifier>();
        notifier.spawner = this;
    }

    public void EnemyDied()
    {
        currentEnemies = Mathf.Max(0, currentEnemies - 1);

        enemiesKilled++;
        EnemyRunData.Instance.enemiesKilled++;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        foreach (Transform t in transform)
        {
            Gizmos.DrawWireSphere(t.position, 0.3f);
        }
    }
}
