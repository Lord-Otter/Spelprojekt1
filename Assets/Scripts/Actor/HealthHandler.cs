using System;
using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Spelprojekt1
{
    public class HealthHandler : MonoBehaviour
    {
        [SerializeField] public int maxHealth, currentHealth;
        [SerializeField] protected bool canTakeDamage;

        [Header("Damage Flash Settings")]
        private SpriteRenderer[] spriteRenderers;
        private Material[] materials;
        [SerializeField] private Color flashColor = Color.white;
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

        public virtual void TakeDamage(int damage) // Maybe add knockback, stuns, or other effects that come from taking damage.
        {
            if(!canTakeDamage)
                return;

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
                    Debug.Log("Flash");
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
