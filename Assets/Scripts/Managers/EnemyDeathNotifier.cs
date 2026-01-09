using UnityEngine;

public class EnemyDeathNotifier : MonoBehaviour
{
    public EnemySpawner spawner;

    private void Start()
    {
        spawner = GameObject.Find("GameManager").GetComponent<EnemySpawner>();
    }

    private void OnDestroy()
    {
        if (spawner != null)
            spawner.EnemyDied();
    }
}