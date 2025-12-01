using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace Spelprojekt1
{
    public class PlayerShooter : MonoBehaviour
    {
        private AimController aimController;
        private PlayerInputHandler playerInputHandler;
        private MovementController movementController;
        private Rigidbody2D rigidBody;
        //[SerializeField] private GameObject arrow;
        [SerializeField] private SpriteRenderer arrowSprite;

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
        [Tooltip("Units are in seconds")] [SerializeField] private float minChargeTime = 0.1f;
        [SerializeField] private float currentCharge = 0f;
        [Tooltip("Units are in seconds")] [SerializeField] private float critWindow = 0.1f;
        [Tooltip("Units are in decimals")] [SerializeField] private float weakDuration = 0.5f;
        [Tooltip("Units are in decimals")] [SerializeField] private float mediumDuration = 1.0f;
        [Tooltip("Units are in seconds")] [SerializeField] private float fireCooldown = 0.2f;
        private float fireCooldownTimer = 0f;

        public bool IsCharging { get; private set;}
        [SerializeField] private float postShotRecovery;
        //private bool canShoot = true;
        public bool IsInShotRecovery { get; private set; }
        private float postShotTimer;

        [Header("Arrow Settings")]
        [SerializeField] private Color baseColor = Color.white;
        [SerializeField] private Color minChargeColor = Color.green;
        [SerializeField] private Color maxChargeColor = Color.red;
        [SerializeField] private float flashDuration = 0.2f;

        private bool hasFlashed = false;
        private float flashTimer = 0f;

        private void Awake()
        {
            aimController = GetComponentInChildren<AimController>();
            playerInputHandler = GetComponent<PlayerInputHandler>();
            movementController = GetComponent<MovementController>();
            rigidBody = GetComponent<Rigidbody2D>();
            arrowSprite = transform.Find("Aimer").transform.Find("Arrow").GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            //Shoot();
            ChargeAttack();
            //UpdateArrowColor();
        }

        private void UpdateArrowColor(float chargeProcent)
        {
            // 1. Handle the flash
            if (hasFlashed && flashTimer > 0f)
            {
                flashTimer -= Time.deltaTime;
                float t = 1f - (flashTimer / flashDuration);

                if (t < 0.5f)
                    arrowSprite.color = Color.Lerp(baseColor, maxChargeColor, t * 2f);
                else
                    arrowSprite.color = Color.Lerp(maxChargeColor, baseColor, (t - 0.5f) * 2f);

                return; // Don't do anything else while flashing
            }

            // 2. Trigger the flash exactly once when reaching full charge
            if (!hasFlashed && chargeProcent >= 1f)
            {
                hasFlashed = true;
                flashTimer = flashDuration;

                arrowSprite.color = baseColor;     // Start flash at white
                return;
            }

            // 3. After flash is done AND charge is full → solid red
            if (hasFlashed && chargeProcent >= 1f)
            {
                arrowSprite.color = maxChargeColor;
                return;
            }

            // 4. Normal charge-up behavior before full charge
            arrowSprite.color = Color.Lerp(minChargeColor, maxChargeColor, chargeProcent);
        }

        private void ChargeAttack()
        {
            fireCooldownTimer += Time.deltaTime;
            IsCharging = playerInputHandler.fire1Held;

            if(fireCooldownTimer >= fireCooldown)
            {
                if(playerInputHandler.fire1Held)
                {
                    movementController.SetMovementLock(true);

                    currentCharge += Time.deltaTime; // Multiply this with eventual custom timescale
                    currentCharge = Mathf.Clamp(currentCharge, 0, maxChargeTime + critWindow + 0.1f);

                    float chargeProcent = Mathf.Clamp01(currentCharge / maxChargeTime);
                    UpdateArrowColor(chargeProcent);
                }

                if (playerInputHandler.fire1Released)
                {
                    if(currentCharge >= minChargeTime)
                    {
                        FireChargedShot(currentCharge);  
                    }
                    else
                    {
                       arrowSprite.color = baseColor; 
                       hasFlashed = false;
                       flashTimer = 0f;
                    }
                      
                    currentCharge = 0;
                    playerInputHandler.fire1Released = false;
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
                    movementController.SetMovementLock(false);
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

            arrowSprite.color = baseColor;
            hasFlashed = false;
            flashTimer = 0f;
        }

        /*private void Shoot()
        {
            fireCooldownTimer += Time.deltaTime;
            if(playerInputHandler.fire1Pressed && fireCooldownTimer >= fireCooldown)
            {
                GameObject projectile = Instantiate(projectileWeak, firePoint.position, firePoint.rotation);
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                rb.linearVelocity = firePoint.right * projectileSpeed;
                fireCooldownTimer = 0;
            }
        }*/
    }
}