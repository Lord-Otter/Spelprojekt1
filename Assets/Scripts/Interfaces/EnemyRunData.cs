using Spelprojekt1;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum EnemyDifficulty
{
    Easy = 0,
    Medium = 1,
    Hard = 2,
}

public class EnemyRunData : MonoBehaviour
{
    public static EnemyRunData Instance { get; private set; }

    [Header("Run Progress")]
    public int enemiesKilled;
    public int totalWavesCleared;
    public int scenesCleared;

    [Header("Difficulty")]
    public int baseWavesPerScene = 3;
    [SerializeField] private int baseEnemiesFirstWave = 3;
    [SerializeField][Tooltip("Lower value means faster ramp up")] private float difficultyRampSpeed = 40f;

    [Header("Score")]
    public int score;
    [SerializeField] private int scorePerEnemyKill = 10;
    [SerializeField] private int scorePerWaveCleared = 50;
    [SerializeField] private int scorePerSceneCleared = 100;
    public float timePlayed;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        timePlayed += Time.deltaTime;
    }

    public EnemyDifficulty RollEnemyDifficulty()
    {
        int wave = totalWavesCleared;

        float progress = wave / difficultyRampSpeed;
        progress = Mathf.Clamp01(progress);

        float easyWeight   = Mathf.Lerp(1f, 0f, progress);
        float mediumWeight = Mathf.Clamp01(Mathf.Sin(progress * Mathf.PI));
        float hardWeight   = Mathf.Lerp(0f, 1f, progress);

        if (wave < 6)
            mediumWeight = 0f;

        if (wave < 13)
            hardWeight = 0f;

        if (progress >= 0.6f)
            easyWeight = 0f;

        float total = easyWeight + mediumWeight + hardWeight;
        if (total <= 0f)
            return EnemyDifficulty.Easy;

        float roll = Random.value * total;

        if (roll < easyWeight)
            return EnemyDifficulty.Easy;

        roll -= easyWeight;
        if (roll < mediumWeight)
            return EnemyDifficulty.Medium;

        return EnemyDifficulty.Hard;
    }

    public int GetWavesForCurrentScene()
    {
        int bonusWaves = scenesCleared / 3;

        return baseWavesPerScene + bonusWaves;
    }

    public void ResetRun()
    {
        enemiesKilled = 0;
        scenesCleared = 0;
        totalWavesCleared = 0;
        score = 0;
        timePlayed = 0;
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;
        score += scorePerEnemyKill;
    }

    public void OnWaveCleared()
    {
        totalWavesCleared++;
        score += scorePerWaveCleared;
    }

    public void OnSceneCleared()
    {
        scenesCleared++;
        score += scorePerSceneCleared;
    }

    public void DebugDifficultyRoll()
    {
        int easy = 0, medium = 0, hard = 0;

        for (int i = 0; i < 1000; i++)
        {
            switch (RollEnemyDifficulty())
            {
                case EnemyDifficulty.Easy: easy++; break;
                case EnemyDifficulty.Medium: medium++; break;
                case EnemyDifficulty.Hard: hard++; break;
            }
        }

        Debug.Log($"Easy: {easy} Medium: {medium} Hard: {hard}");
    }

    public int GetStartingEnemiesForScene()
    {
        int tier = scenesCleared / 3;
        return baseEnemiesFirstWave + tier;
    }

    public int GetEnemiesForWave(int baseEnemies, int currentWave)
    {
        int sceneOffset = scenesCleared % 3;
        return baseEnemies + sceneOffset + (currentWave - 1);
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(timePlayed / 60f);
        int seconds = Mathf.FloorToInt(timePlayed % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}
