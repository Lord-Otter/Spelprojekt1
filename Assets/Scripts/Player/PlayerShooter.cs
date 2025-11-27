using UnityEngine;

namespace Spelprojekt1
{
    public class PlayerShooter : MonoBehaviour
    {
        private AimController aimController;
        private InputPlayer inputPlayer;

        [Header("Projectile Settings")]
        [SerializeField] private GameObject projectileWeak;
        [SerializeField] private GameObject projectileMedium;
        [SerializeField] private GameObject projectileStrong;
        [SerializeField] private GameObject projectileCrit;

        [SerializeField] private Transform firePoint;
        [SerializeField] private int projectileBaseDamage = 100;
        [SerializeField] private float projectileSpeed = 10f;

        [Header("Charge Settings")]
        [Tooltip("Units are in seconds")] [SerializeField] private float maxChargeTime = 1.0f;
        [SerializeField] private float currentCharge = 0f;
        [Tooltip("Units are in seconds")] [SerializeField] private float critWindow = 0.1f;
        [Tooltip("Units are in decimals")] [SerializeField] private float weakDuration = 0.5f;
        [Tooltip("Units are in decimals")] [SerializeField] private float mediumDuration = 1.0f;
        [Tooltip("Units are in seconds")] [SerializeField] private float fireCooldown = 0.2f;
        private float fireCooldownTimer = 0f;

        public bool IsCharging { get; private set;}
        [SerializeField] private float postShotRecovery;
        public bool IsInShotRecovery { get; private set; }
        private float postShotTimer;

        private void Awake()
        {
            if (!aimController) aimController = GetComponentInChildren<AimController>();
            if (!inputPlayer) inputPlayer = GetComponent<InputPlayer>();
        }

        private void Update()
        {
            //Shoot();
            ChargeAttack();
        }

        private void ChargeAttack()
        {
            fireCooldownTimer += Time.deltaTime;
            IsCharging = inputPlayer.fire1Held;

            if(fireCooldownTimer >= fireCooldown)
            {
                if(inputPlayer.fire1Held)
                {
                    currentCharge += Time.deltaTime; // Multiply this with eventual custom timescale
                    currentCharge = Mathf.Clamp(currentCharge, 0, maxChargeTime + critWindow + 0.1f);
                }

                if (inputPlayer.fire1Released)
                {
                    FireChargedShot(currentCharge);

                    currentCharge = 0;
                    inputPlayer.fire1Released = false;
                    fireCooldownTimer = 0;

                    IsInShotRecovery = true;
                    postShotTimer = postShotRecovery;
                }
            }

            if (IsInShotRecovery)
            {
                postShotTimer -= Time.deltaTime;
                if(postShotTimer <= 0f)
                {
                    IsInShotRecovery = false;
                }
            }
        }

        private void FireChargedShot(float charge)
        {
            GameObject projectileToUse;

            float chargeProcent = charge / maxChargeTime;

            if (chargeProcent < weakDuration)
            {
                projectileToUse = projectileWeak;
            }
            else if (chargeProcent < mediumDuration)
            {
                projectileToUse = projectileMedium;
            }
            else
            {
                if(charge < maxChargeTime + critWindow)
                {
                    projectileToUse = projectileCrit;
                }
                else
                {
                    projectileToUse = projectileStrong;
                }
                    
            }
            /* The projectiles damage should be projectileBaseDamage * chargeProcent.
            *  If the player crits do projectileBaseDamage * 1.2f.
            *  If not then do projectileBaseDamage * 1. */ 
            
            Debug.Log(chargeProcent);
            GameObject projectile = Instantiate(projectileToUse, firePoint.position, firePoint.rotation); 
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.linearVelocity = firePoint.right * projectileSpeed;
        }

        private void Shoot()
        {
            fireCooldownTimer += Time.deltaTime;
            if(inputPlayer.fire1Pressed && fireCooldownTimer >= fireCooldown)
            {
                GameObject projectile = Instantiate(projectileWeak, firePoint.position, firePoint.rotation);
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                rb.linearVelocity = firePoint.right * projectileSpeed;
                fireCooldownTimer = 0;
            }
        }
    }
}