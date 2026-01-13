using UnityEngine;
using TMPro; // Make sure you have TextMeshPro imported

public class WaveUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemySpawner spawner;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text totalEnemiesText;
    [SerializeField] private TMP_Text enemiesLeftText;
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        waveText = GameObject.Find("WaveText(TMP)").GetComponent<TMP_Text>();
        enemiesLeftText = GameObject.Find("EnemiesCounterText(TMP)").GetComponent<TMP_Text>();
        scoreText = GameObject.Find("ScoreText(TMP)").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (spawner == null)
            return;

        // Current wave
        waveText.text = $"Wave: {spawner.CurrentWave} / {spawner.MaxWaves}";

        // Total enemies in the current wave
        //totalEnemiesText.text = $" / {spawner.EnemiesPerWave}";

        // Enemies remaining
        enemiesLeftText.text = $"Enemies: {spawner.CurrentEnemies} / {spawner.EnemiesPerWave}";

        // Current Score
        scoreText.text = $"Score: {EnemyRunData.Instance.score}";
    }
}
