using UnityEngine;

public class EnemyRunData : MonoBehaviour
{
    public static EnemyRunData Instance { get; private set; }

    [Header("Run Progress")]
    public int enemiesKilled;
    public int totalWavesCleared;
    public int scenesCleared;

    [Header("Difficulty")]
    public int baseWavesPerScene = 3;
    public int extraEnemiesNextScene = 0;

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

    public int GetWavesForCurrentScene()
    {
        // Every 3 cleared waves adds 1 wave to future scenes
        int bonusWaves = scenesCleared / 3;
        return baseWavesPerScene + bonusWaves;
    }

    public int GetEnemiesPerWave(int baseEnemies)
    {
        return baseEnemies + extraEnemiesNextScene;
    }

    // Call this after each wave to increment difficulty for next wave/scene
    public void OnWaveCleared(int enemyIncreasePerWave)
    {
        extraEnemiesNextScene += enemyIncreasePerWave;
    }

    public void ResetRun()
    {
        enemiesKilled = 0;
        scenesCleared = 0;
        totalWavesCleared = 0;
    }

    public void OnSceneCleared()
    {
        scenesCleared++;
    }
}
