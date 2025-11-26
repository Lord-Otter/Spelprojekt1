using UnityEngine;

namespace Spelprojekt1
{
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] private AimController aimController;
        [SerializeField] private InputPlayer inputPlayer;

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
            // Count down cooldown timer
            if (fireCooldownTimer > 0f)
                fireCooldownTimer -= Time.deltaTime;

            bool wantsToShoot = false;

            switch (aimController.CurrentMode)
            {
                case AimController.RotationMode.MouseAim:
                    wantsToShoot = inputPlayer.fireMousePressed;
                    break;

                case AimController.RotationMode.StickAim:
                    wantsToShoot = inputPlayer.fireStickPressed;
                    break;

                case AimController.RotationMode.MoveAim:
                    wantsToShoot = inputPlayer.fireMovePressed;
                    break;
            }

            if (wantsToShoot)
                TryShoot();
        }

        private void TryShoot()
        {
            if (fireCooldownTimer > 0f)
                return; // Still cooling down

            Shoot();
            fireCooldownTimer = fireCooldown; // Reset cooldown
        }

        private void Shoot()
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.linearVelocity = firePoint.right * projectileSpeed;
        }
    }
}

