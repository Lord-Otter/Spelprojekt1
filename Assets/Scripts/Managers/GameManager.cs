using UnityEngine;

namespace Spelprojekt1
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerDataPrefab;

        private void Awake()
        {
            GameObject playerData = GameObject.Find("PlayerData");

            if (playerData == null)
            {
                playerData = Instantiate(playerDataPrefab);
                playerData.name = "PlayerData";
            }
        }
    }
}