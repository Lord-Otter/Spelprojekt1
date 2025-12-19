using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private float spawnDelay = 2f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(SpawnEnemyAfterDelay());
    }

    private IEnumerator SpawnEnemyAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);

        if (enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned!");
            yield break;
        }

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        // Instantiate at this object's position
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}