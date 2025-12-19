using System.Collections.Generic;
using UnityEngine;

namespace Spelprojekt1
{
    public class EnemyHealth : HealthHandler
    {
        [SerializeField] [Range(0, 1)] private float hurtSoundsVolume;
        [SerializeField] private List<AudioClip> hurtSounds;
        [SerializeField] [Range(0, 1)] private float deathSoundsVolume;
        [SerializeField] private List<AudioClip> deathSounds;
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
            // Play Damage SFX
            SFXManager.instance.PlayRandomSFXClip(hurtSounds, transform, hurtSoundsVolume);

            // Play damage flash and/or animation
            // Stagger or knockback or canceling attacks or any other feedback
            
        }

        protected override void HandleDeath()
        {
            SFXManager.instance.PlayRandomSFXClip(deathSounds, transform, deathSoundsVolume);

            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out HealthHandler health))
            {
                health.TakeDamage(1);
            }
        }
    }
}
