using UnityEngine;

namespace Spelprojekt1
{
    public class PlayerShooter : MonoBehaviour
    {
        private AimController aimController;
        private InputPlayer inputPlayer;

        [Header("Projectile Settings")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float projectileSpeed = 10f;

        [Header("Firing")]
        [SerializeField] private float fireCooldown = 0.2f; // Time between shots
        private float fireCooldownTimer = 0f;

        private void Awake()
        {
            if (!aimController) aimController = GetComponentInChildren<AimController>();
            if (!inputPlayer) inputPlayer = GetComponent<InputPlayer>();
        }

        private void Update()
        {
            Shoot();
        }

        private void Shoot()
        {
            fireCooldownTimer += Time.deltaTime;
            if(inputPlayer.fire1Pressed && fireCooldownTimer >= fireCooldown)
            {
                GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
                rb.linearVelocity = firePoint.right * projectileSpeed;
                fireCooldownTimer = 0;
            }
        }
    }
}

