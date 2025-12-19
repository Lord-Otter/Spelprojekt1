using UnityEngine;

namespace Spelprojekt1
{
    public class PlayerData : MonoBehaviour
    {
        public static PlayerData Instance { get; private set; }

        public PlayerStats stats = new PlayerStats();

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void ResetRun()
        {
            stats.Reset();
        }
    }
}

