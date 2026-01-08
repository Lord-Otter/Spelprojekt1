using System;
using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Spelprojekt1
{
    public class HealthHandler : MonoBehaviour
    {
        [SerializeField] public int maxHealth, currentHealth;
        [SerializeField] protected bool canTakeDamage;

        [Header("Damage Flash Settings")]
        protected SpriteRenderer[] spriteRenderers;
        protected Material[] materials;
        [SerializeField] protected Color flashColor = Color.white;
        [SerializeField] private float flashTime = 0.25f;

        private Coroutine damageFlashCoroutine;

        protected virtual void Awake()
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            Initialize();
        }

        protected virtual void Start()
        {
            currentHealth = maxHealth;

        }

        protected virtual void Initialize()
        {
            materials = new Material[spriteRenderers.Length];

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                materials[i] = spriteRenderers[i].material;
            }
        }

        protected virtual void Update()
        {
            
        }

        public virtual void TakeDamage(int damage)
        {
            if(!canTakeDamage)
                return;
            
            Debug.Log("HIT!");

            currentHealth -= damage;
            if(currentHealth > 0)
            {
                HandleDamage(); // Maybe make this a coroutine for a sequence of events.
            }
            else
            {
                HandleDeath(); // Maybe make this a coroutine for a sequence of events.
            }
        }

        public void CallDamageFlash()
        {
            damageFlashCoroutine = StartCoroutine(DamageFlasher());
        }

        private IEnumerator DamageFlasher()
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetColor("_FlashColor", flashColor);
            }

            float currentFlashAmount = 0f;
            float elapsedTime = 0f;
            while(elapsedTime < flashTime)
            {
                elapsedTime += Time.deltaTime;

                currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / flashTime));

                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_FlashAmount", currentFlashAmount);
                }

                yield return null;
            }
        }

        protected virtual void HandleDamage()
        {
            CallDamageFlash();
        }

        protected virtual void HandleDeath()
        {
            
        }
    }
}
