using UnityEngine;

namespace Spelprojekt1
{
    public class EnemyHealth : HealthHandler
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
            canTakeDamage = true;
        }

        // Update is called once per frame
        protected override void Update()
        {
            
        }

        protected override void HandleDamage()
        {
            // Play damage flash and/or animation
            // Stagger or knockback or canceling attacks or any other feedback
            
        }

        protected override void HandleDeath()
        {
            Destroy(gameObject);
        }
    }
}
