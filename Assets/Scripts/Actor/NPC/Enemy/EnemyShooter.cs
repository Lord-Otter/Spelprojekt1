using System.Collections.Generic;
using UnityEngine;

namespace Spelprojekt1
{
    public class EnemyShooter : MonoBehaviour
    {
        [Header("Shooting Settings")]
        public GameObject projectilePrefab;
        public Transform firePoint;

        [Tooltip("Minimum time between shots")]
        public float minShootTime = 1f;

        [Tooltip("Maximum time between shots")]
        public float maxShootTime = 3f;

        private float shootTimer;
        [SerializeField] private float projectileSpeed;

        [Header("Audio")]
        [SerializeField] [Range(0, 1)] private float shootSoundsVolume;
        [SerializeField] private List<AudioClip> shootSounds;

        void Start()
        {
            ResetShootTimer();
        }

        void Update()
        {
            shootTimer -= Time.deltaTime;

            if (shootTimer <= 0f)
            {
                Shoot();
                ResetShootTimer();
            }
        }

        void ResetShootTimer()
        {
            shootTimer = Random.Range(minShootTime, maxShootTime);
        }

        void Shoot()
        {
            SFXManager.instance.PlayRandomSFXClip(shootSounds, transform, shootSoundsVolume);

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            EnemyProjectile projectileBehaviour = projectile.GetComponent<EnemyProjectile>();

            projectileBehaviour.SetDirection(firePoint.right);
        }
    }
}