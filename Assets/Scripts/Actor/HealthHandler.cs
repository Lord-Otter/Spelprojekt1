using System.Collections;
using UnityEngine;

namespace Spelprojekt1
{
    public class HealthHandler : MonoBehaviour
    {
        public int maxHealth, currentHealth;
        protected bool canTakeDamage;

        protected virtual void Awake()
        {
            
        }

        protected virtual void Start()
        {
            currentHealth = maxHealth;
        }

        protected virtual void Update()
        {
            
        }

        public virtual void TakeDamage(int damage) // Maybe add knockback, stuns, or other effects that come from taking damage.
        {
            /*if(!canTakeDamage)
                return;

            currentHealth -= damage;
            if(currentHealth > 0)
            {
                HandleDamage(); // Maybe make this a coroutine for a sequence of events.
            }
            else
            {
                HandleDeath(); // Maybe make this a coroutine for a sequence of events.
            }*/
        }

        protected virtual void HandleDamage()
        {
            
        }

        protected virtual void HandleDeath()
        {
            
        }
    }
}
