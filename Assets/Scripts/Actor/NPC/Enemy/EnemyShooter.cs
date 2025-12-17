using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Spelprojekt1
{
    public class EnemyShooter : MonoBehaviour
    {
        private EnemyAI enemyAI;

        [Header("Shooting Settings")]
        [SerializeField] private GameObject projectilePrefab;
        private Transform aimer;
        private Transform player;
        public Transform firePoint;

        [Tooltip("Minimum time between shots")]
        [SerializeField] private float minShootTime = 1f;

        [Tooltip("Maximum time between shots")]
        public float maxShootTime = 3f;

        private float shootTimer;
        [SerializeField] private int projectileDamage;
        [SerializeField] private float projectileSpeed;

        [Header("Audio")]
        [SerializeField] [Range(0, 1)] private float shootSoundsVolume;
        [SerializeField] private List<AudioClip> shootSounds;

        void Awake()
        {
            enemyAI = GetComponent<EnemyAI>();
            aimer = transform.Find("Aimer");
        }

        void Start()
        {
            player = GameObject.Find("Player").transform;

            ResetShootTimer();
        }

        void Update()
        {
            shootTimer -= Time.deltaTime;

            if (enemyAI.IsPlayerInSight())
            {
                if(shootTimer <= 0f)
                {
                    Shoot();
                    ResetShootTimer();
                }
            }

            RotateAimerTowardsPlayer();
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

            projectileBehaviour.Initialize(projectileDamage, projectileSpeed);
            projectileBehaviour.SetDirection(firePoint.right);
        }

        void RotateAimerTowardsPlayer()
        {
            Vector3 directionToPlayer = player.position - aimer.position;
            directionToPlayer.z = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToPlayer);
            
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z + 90f);

            aimer.rotation = Quaternion.Slerp(aimer.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}