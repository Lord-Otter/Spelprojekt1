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

        [Header("Damage Particles Settings")]
        [SerializeField] private ParticleSystem damageParticlesBlood;
        [SerializeField] private ParticleSystem damageParticlesImpact;
        private ParticleSystem damageParticlesBloodInstance;
        private ParticleSystem damageParticlesImpactInstance;

        private Transform playerPosition;

        protected override void Start()
        {
            base.Start();
            playerPosition = GameObject.Find("Player").GetComponent<Transform>();
            canTakeDamage = true;
        }

        // Update is called once per frame
        protected override void Update()
        {
            
        }

        protected override void HandleDamage()
        {
            base.HandleDamage();
            // Play Damage SFX
            SFXManager.instance.PlayRandomSFXClip(hurtSounds, transform, hurtSoundsVolume);

            // Play damage flash and/or animation
            SpawnDamageParticles();

            // Stagger or knockback or canceling attacks or any other feedback
            
        }

        protected override void HandleDeath()
        {
            SFXManager.instance.PlayRandomSFXClip(deathSounds, transform, deathSoundsVolume);
            
            SpawnDamageParticles();

            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out HealthHandler health) && other.CompareTag("Player"))
            {
                health.TakeDamage(1);
            }
        }

        private void SpawnDamageParticles()
        {
            Vector2 toPlayer = (playerPosition.position - transform.position).normalized;

            Vector3 spawnPosition = transform.position + (Vector3)(toPlayer * 1f) + new Vector3(0, 0, -0.5f);

            float angle = Mathf.Atan2(-toPlayer.y, -toPlayer.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            damageParticlesBloodInstance = Instantiate(damageParticlesBlood, spawnPosition, rotation);
            damageParticlesImpactInstance = Instantiate(damageParticlesImpact, spawnPosition, rotation);
        }
    }
}
