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
            Debug.Log("BAM BAM SHOOTY BOOTY");
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            ProjectileBehaviour projectileBehaviour = projectile.GetComponent<ProjectileBehaviour>();
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            projectileBehaviour.Initialize(1, 1, 1);
            rb.linearVelocity = firePoint.right * projectileSpeed;
        }
    }
}