using UnityEngine;

namespace Spelprojekt1
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerDataPrefab;
        [SerializeField] private GameObject enemyRunDataPrefab;

        private void Awake()
        {
            GameObject playerData = GameObject.Find("PlayerData");
            GameObject enemyRunData = GameObject.Find("EnemyRunData");

            if (playerData == null)
            {
                playerData = Instantiate(playerDataPrefab);
                playerData.name = "PlayerData";
            }

            if (enemyRunData == null)
            {
                enemyRunData = Instantiate(enemyRunDataPrefab);
                enemyRunData.name = "EnemyRunData";
            }
        }
    }
}