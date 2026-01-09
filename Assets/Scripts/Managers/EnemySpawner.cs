using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    private EnemyRunData runData;
    [SerializeField] private SceneLoader sceneLoader;

    private int difficultyLevel;

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
    [SerializeField] private int maxWaves;
    [SerializeField] private int currentWave;

    [Header("Spawn Warning")]
    [SerializeField] private float spawnWarningTime = 1f;

    private int currentEnemies = 0;
    [SerializeField] private int enemiesKilled = 0;

    public int CurrentWave => currentWave;
    public int MaxWaves => maxWaves;
    public int EnemiesPerWave => enemiesPerWave;
    public int CurrentEnemies => currentEnemies;

    private void Awake()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();

        if (spawnPoints.Count == 0)
        {
            foreach (Transform child in transform)
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

        currentWave = 0;
        maxWaves = runData.GetWavesForCurrentScene();
        enemiesPerWave = runData.GetEnemiesForWave(baseEnemiesPerWave, 1);

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
            runData.totalWavesCleared++;
            enemiesPerWave = runData.GetEnemiesForWave(baseEnemiesPerWave, currentWave + 1);

        }

        runData.OnSceneCleared();

        sceneLoader.LoadRandomGameScene();

    }
    
    private IEnumerator SpawnWave()
    {
        currentEnemies = enemiesPerWave;

        int enemiesToSpawn = enemiesPerWave;
        List<Vector2> usedPositions = new List<Vector2>();

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 spawnPos;

            if (spawnMode == SpawnMode.FixedPoints)
            {
                if (spawnPoints.Count == 0)
                    yield break;

                Transform point = spawnPoints[Random.Range(0, spawnPoints.Count)];
                spawnPos = point.position;

                SpriteRenderer sr = point.GetComponent<SpriteRenderer>();
                if (sr != null) sr.enabled = true;

                yield return new WaitForSeconds(spawnWarningTime);

                if (sr != null) sr.enabled = false;
            }
            else
            {
                spawnPos = GetValidRandomPosition(usedPositions);
            }

            GameObject portal = Instantiate(spawnPointPrefab, spawnPos, Quaternion.identity);

            EnemySpawnObject spawnObject = portal.GetComponent<EnemySpawnObject>();
            if (spawnObject != null)
            {
                EnemyDifficulty difficulty = EnemyRunData.Instance.RollEnemyDifficulty();
                spawnObject.Init((int)difficulty, this);
            }

            usedPositions.Add(spawnPos);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void EnemySpawned()
    {
        currentEnemies++;
    }

    public void EnemyDied()
    {
        currentEnemies = Mathf.Max(0, currentEnemies - 1);

        enemiesKilled++;
        EnemyRunData.Instance.enemiesKilled++;
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

            if (!tooClose)
                return point;
        }

        return spawnArea.bounds.center;
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