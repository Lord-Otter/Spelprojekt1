using System.Collections.Generic;
using UnityEngine;

namespace Spelprojekt1
{
    public class UIHealthBar : MonoBehaviour
    {
        private PlayerHealth playerHealth;
        private List<GameObject> hearts = new List<GameObject>();
        [SerializeField] private GameObject heart;
        private Transform healthBarTransform;

        void Awake()
        {
            playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
            healthBarTransform = GetComponent<Transform>();
        }

        void Start()
        {
            CreateHearts();
            playerHealth.OnHealthChanged += UpdateHearts;
        }

        private void CreateHearts()
        {
            for (int i = 0; i < playerHealth.maxHealth; i++)
            {
                GameObject h = Instantiate(heart, healthBarTransform);
                hearts.Add(h);
            }

            UpdateHearts(playerHealth.currentHealth);
        }

        private void UpdateHearts(int currentHealth)
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                hearts[i].SetActive(i < currentHealth);
            }
        }
    }
}
