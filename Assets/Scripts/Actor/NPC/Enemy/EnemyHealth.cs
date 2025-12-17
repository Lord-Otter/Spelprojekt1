using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

namespace Spelprojekt1
{
    public class EnemyHealth : HealthHandler
    {
        [Header("Audio")]
        [SerializeField] [Range(0, 1)] private float hurtSoundsVolume;
        [SerializeField] private List<AudioClip> hurtSounds;
        [SerializeField] [Range(0, 1)] private float deathSoundsVolume;
        [SerializeField] private List<AudioClip> deathSounds;

        protected override void Start()
        {
            base.Start();
            canTakeDamage = true;
        }

        protected override void Update()
        {
            
        }

        protected override void HandleDamage()
        {
            // Play damage flash and/or animation
            // Stagger or knockback or canceling attacks or any other feedback

            // Play damage SFX
            SFXManager.instance.PlayRandomSFXClip(hurtSounds, transform, hurtSoundsVolume);
        }

        protected override void HandleDeath()
        {
            //Play death SFX
            SFXManager.instance.PlayRandomSFXClip(deathSounds, transform, deathSoundsVolume);

            Destroy(gameObject);
        }
    }
}
