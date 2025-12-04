using UnityEngine;

public class EnemyDeathNotifier : MonoBehaviour
{
    public EnemySpawner spawner;

    private void OnDestroy()
    {
        if (spawner != null)
            spawner.EnemyDied();
    }
}