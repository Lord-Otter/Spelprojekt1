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

    private void Update()
    {
        if (spawner == null)
            return;

        // Current wave
        waveText.text = $"Wave: {spawner.CurrentWave}/{spawner.MaxWaves}";

        // Total enemies in the current wave
        //totalEnemiesText.text = $" / {spawner.EnemiesPerWave}";

        // Enemies remaining
        enemiesLeftText.text = $"Enemies: {spawner.CurrentEnemies} / {spawner.EnemiesPerWave}";
    }
}
