using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private List<GameObject> enemyPrefabsEasy;
    [SerializeField] private List<GameObject> enemyPrefabsMedium;
    [SerializeField] private List<GameObject> enemyPrefabsHard;

    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private float portalGrowthTime = 1f;

    private int difficultyLevel = 0; // 0 = Easy | 1 = Medium | 2 = Hard
    private EnemySpawner spawner;
    private EnemyDeathNotifier notifier;

    private const float MAX_SCALE = 0.3f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.zero;
        StartCoroutine(SpawnEnemy());
    }

    public void Init(int difficulty, EnemySpawner enemySpawner)
    {
        difficultyLevel = Mathf.Clamp(difficulty, 0, 2);
        spawner = enemySpawner;
    }

    private IEnumerator SpawnEnemy()
    {
        float elapsedTime = 0f;

        while(elapsedTime < portalGrowthTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / portalGrowthTime;

            float logT = Mathf.Log10(t * 9f + 1);
            float scale = Mathf.Lerp(0f, MAX_SCALE, logT);

            transform.localScale = Vector3.one * scale;
            yield return null;
        }

        transform.localScale = Vector3.one * MAX_SCALE;

        yield return new WaitForSeconds(spawnDelay);

        GameObject enemyToSpawn = GetRandomEnemy();
        if(enemyToSpawn != null)
        {
            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(0.5f);

        elapsedTime = 0f;

        while(elapsedTime < portalGrowthTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / portalGrowthTime;

            float logT = Mathf.Log10(t * 9f + 1f);
            float scale = Mathf.Lerp(MAX_SCALE, 0f, logT);

            transform.localScale = Vector3.one * scale;
            yield return null;
        }

        transform.localScale = Vector3.zero;

        Destroy(gameObject);
    }

    private GameObject GetRandomEnemy()
    {
        List<GameObject> list = difficultyLevel switch
        {
            0 => enemyPrefabsEasy,
            1 => enemyPrefabsMedium,
            2 => enemyPrefabsHard,
            _ => enemyPrefabsEasy
        };

        if(list == null || list.Count == 0)
            return null;

        return list[Random.Range(0, list.Count)];
    }
}