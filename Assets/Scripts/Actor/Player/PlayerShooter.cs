using UnityEditor.Rendering;
using UnityEngine;

namespace Spelprojekt1
{
    public class PlayerShooter : MonoBehaviour
    {
        public enum ShootState
        {
            Ready,
            Charging,
            ChargedRelease,
            Recovery,
            Stunned
        }

        public ShootState State { get; private set; } = ShootState.Ready;

        private AimController aimController;
        private PlayerInputHandler playerInputHandler;
        private MovementController movementController;
        private Rigidbody2D rigidBody;
        private SpriteRenderer arrowSprite;

        [Header("Projectile Settings")]
        [SerializeField] private GameObject projectileWeak;
        [SerializeField] private GameObject projectileMedium;
        [SerializeField] private GameObject projectileStrong;
        [SerializeField] private GameObject projectileCrit;

        [SerializeField] private Transform firePoint;
        [SerializeField] private int projectileBaseDamage = 100;
        //[SerializeField] private float projectileSpeed = 10f;
        [SerializeField] private float maxProjectileRange;
        [SerializeField] private float minProjectileRange;

        [Header("Charge Settings")]
        [SerializeField] private float maxChargeTime = 1.0f;
        [SerializeField] private float minChargeTime = 0.1f;
        [SerializeField] private float critWindow = 0.1f;
        [SerializeField] private float weakDuration = 0.5f;
        [SerializeField] private float mediumDuration = 1.0f;
        [SerializeField] private float fireCooldown = 0.2f;

        private float currentCharge = 0f;
        private float fireCooldownTimer = 0f;

        [Header("Recovery Settings")]
        [SerializeField] private float postShotRecovery = 0.3f;
        private float recoveryTimer = 0f;

        [Header("Stunned Settings")]
        [SerializeField] private float stunDuration;
        private float stunTimer;

        [Header("Arrow Settings")]
        [SerializeField] private Color baseColor = Color.white;
        [SerializeField] private Color minChargeColor = Color.green;
        [SerializeField] private Color maxChargeColor = Color.red;
        [SerializeField] private float flashDuration = 0.2f;

        private bool hasFlashed = false;
        private float flashTimer = 0f;

        [Header("Audio")]
        private AudioSource audioSource;
        private AudioSource chargeSFXInstance;

        [SerializeField] private AudioClip chargeStartSFX;
        [SerializeField] private AudioClip shotNormalSFX;
        [SerializeField] private AudioClip shotCritSFX;
        [SerializeField] private AudioClip fullChargeSFX;

        private bool wasCharging = false;
        private bool fullChargeReached = false;

        private void Awake()
        {
            aimController = GetComponentInChildren<AimController>();
            playerInputHandler = GetComponent<PlayerInputHandler>();
            movementController = GetComponent<MovementController>();
            rigidBody = GetComponent<Rigidbody2D>();
            arrowSprite = transform.Find("Aimer").transform.Find("Arrow").GetComponent<SpriteRenderer>();

            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            UpdateCooldowns();
            
            if (State == ShootState.Stunned)
            {
                Stunned();
                return;
            }

            switch (State)
            {
                case ShootState.Ready:
                    Ready();
                    break;

                case ShootState.Charging:
                    Charging();
                    break;
                
                case ShootState.ChargedRelease:
                    ChargedRelease();
                    break;
            
                case ShootState.Recovery:
                    Recovery();
                    break;

                case ShootState.Stunned:
                    Stunned();
                    break;
            }
        }

        // Ready State
        private void Ready()
        {
            if(playerInputHandler.fire1Held && fireCooldownTimer >= fireCooldown)
            {
                BeginCharging();
            }
        }

        private void BeginCharging()
        {
            State = ShootState.Charging;
            ClearCharge();
        }

        // Charging State
        private void Charging()
        {
            if (playerInputHandler.fire1Held)
            {
                if (!wasCharging) // Play charge up SFX while assigning the instance to a variable
                {
                    chargeSFXInstance = SFXManager.instance.PlaySFXClip(chargeStartSFX, transform, 1f);
                    wasCharging = true;
                }

                currentCharge += Time.deltaTime;
                currentCharge = Mathf.Clamp(currentCharge, 0, maxChargeTime + critWindow + 0.1f);

                float chargePercent = Mathf.Clamp01(currentCharge / maxChargeTime);
                if(chargePercent >= 1 && !fullChargeReached)
                {
                    SFXManager.instance.PlaySFXClip(fullChargeSFX, transform, 1f);
                    fullChargeReached = true;
                }
                UpdateArrowColor(chargePercent);
                return;
            }

            if (playerInputHandler.fire1Released)
            {
                State = ShootState.ChargedRelease;
            }
        }

        // Charged Release State
        private void ChargedRelease()
        {
            if (chargeSFXInstance != null)
            {
                chargeSFXInstance.Stop();
                Destroy(chargeSFXInstance.gameObject);
                chargeSFXInstance = null;
            }

            float finalCharge = currentCharge;
            if (finalCharge >= minChargeTime)
            {
                FireChargedShot(finalCharge);
            }

            ResetChargeVisuals();

            fireCooldownTimer = 0f;

            State = ShootState.Recovery;
            recoveryTimer = postShotRecovery;
        }

        // Recovery State
        private void Recovery()
        {
            recoveryTimer -= Time.deltaTime;

            if(recoveryTimer <= 0)
            {
                State = ShootState.Ready;
            }
        }

        // Firing Logic
        private void FireChargedShot(float charge)
        {
            float chargePercent = charge / maxChargeTime;
            GameObject projectileToUse;

            if (chargePercent < weakDuration)
            {
                projectileToUse = projectileWeak;
            }
            else if (chargePercent < mediumDuration)
            {
                projectileToUse = projectileMedium;
            }
            else
            {
                projectileToUse = (charge < maxChargeTime + critWindow) ? projectileCrit : projectileStrong;
            }

            // Play SFX
            if(projectileToUse == projectileCrit)
            {
                SFXManager.instance.PlaySFXClip(shotCritSFX, transform, 1f);
            }
            else
            {
                SFXManager.instance.PlaySFXClip(shotNormalSFX, transform, 1f);
            }

            // Instantiate Projectile
            GameObject projectile = Instantiate(projectileToUse, firePoint.position, firePoint.rotation);
            PlayerProjectile playerProjectile = projectile.GetComponent<PlayerProjectile>();

            // Damage Calculation
            float damage;
            bool pierce;
            if(chargePercent < 1)
            {
                damage = projectileBaseDamage * chargePercent;
                pierce = true;
            }
            else
            {
                if(charge <= maxChargeTime + critWindow)
                {
                    damage = projectileBaseDamage * 1.2f;
                    pierce = true;
                }
                else
                {
                    damage = projectileBaseDamage;
                    pierce = true;
                }
            }

            float projectileRange = Mathf.Lerp(minProjectileRange, maxProjectileRange, chargePercent);

            playerProjectile.Initialize(firePoint.right, damage, projectileRange, 1, pierce); // pierce is set to true for now. We'll see if we change it. Knockback is set to 1 for now. Idk if I want it to be a flat number or a multiplier. Range is a flat 7.5f untill I do varable range depending on charge.
        }

        // Stunned State
        private void Stunned()
        {
            stunTimer -= Time.deltaTime;

            if(stunTimer <= 0)
            {
                State = ShootState.Ready;
            }
        }

        public void ApplyStun(float duration)
        {
            stunDuration = duration;
            stunTimer = duration;

            // Set State to Stunned
            State = ShootState.Stunned;

            // Cancel charge SFX
            if(chargeSFXInstance != null)
            {
                chargeSFXInstance.Stop();
                Destroy(chargeSFXInstance.gameObject);
                chargeSFXInstance = null;
            }

            // Reset charging visuals
            ResetChargeVisuals();

            // Clear charge
            ClearCharge();
        }

        private void ClearCharge()
        {
            currentCharge = 0;
            wasCharging = false;
            fullChargeReached = false;
            hasFlashed = false;
            flashTimer = 0;
        }

        //-----------------------
        private void ResetChargeVisuals()
        {
            arrowSprite.color = baseColor;
            hasFlashed = false;
            flashTimer = 0f;
        }

        private void UpdateArrowColor(float chargePercent)
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

                return;
            }

            if (!hasFlashed && chargePercent >= 1f)
            {
                hasFlashed = true;
                flashTimer = flashDuration;

                arrowSprite.color = baseColor;
                return;
            }

            if (hasFlashed && chargePercent >= 1f)
            {
                arrowSprite.color = maxChargeColor;
                return;
            }

            arrowSprite.color = Color.Lerp(minChargeColor, maxChargeColor, chargePercent);
        }

        private void UpdateCooldowns()
        {
            fireCooldownTimer += Time.deltaTime;
        }
    }
}