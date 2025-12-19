using System.Collections.Generic;
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

        [Header("Projectile Settings")]
        [SerializeField] private GameObject projectileWeak;
        [SerializeField] private GameObject projectileMedium;
        [SerializeField] private GameObject projectileStrong;
        [SerializeField] private GameObject projectileCrit;

        [SerializeField] private Transform firePoint;
        [SerializeField] private int projectileBaseDamage = 100;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float maxProjectileRange;
        [SerializeField] private float minProjectileRange;
        [SerializeField] private float maxKnockbackForce;
        [SerializeField] private float minKnockbackForce;
        [SerializeField] private float knockbackDuration;

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
        private SpriteRenderer arrowSprite;

        // Moving the Arrow
        private Transform arrowHeadTransform;
        private SpriteRenderer arrowHeadSR;
        private Transform arrowShaftTransform;
        private SpriteRenderer arrowShaftSR;
        [SerializeField] private float headOffset;
        [SerializeField] private float shaftOffset;

        // Changing the Arrows Color
        private bool isFlashing = false;
        private bool isIncreasingAlpha = false;
        [SerializeField] private float flashDuration = 0.2f;
        private float flashTimer;
        private bool hasFlashed = false;        

        [Header("Audio")]
        private AudioSource audioSource;
        private AudioSource chargeSFXInstance;

        [SerializeField] [Range(0, 1)] private float chargeStartSFXVolume;
        [SerializeField] private AudioClip chargeStartSFX;
        [SerializeField] [Range(0, 1)] private float shotNormalSFXVolume;
        [SerializeField] private List<AudioClip> shotNormalSFX;
        [SerializeField] [Range(0, 1)] private float shotCritSFXVolume;
        [SerializeField] private AudioClip shotCritSFX;
        [SerializeField] [Range(0, 1)] private float fullChargeSFXVolume;
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
            arrowHeadTransform = transform.Find("Aimer").transform.Find("ArrowHead").transform;
            arrowHeadSR = transform.Find("Aimer").transform.Find("ArrowHead").GetComponent<SpriteRenderer>();
            arrowShaftTransform = transform.Find("Aimer").transform.Find("ArrowShaft").transform;
            arrowShaftSR = transform.Find("Aimer").transform.Find("ArrowShaft").GetComponent<SpriteRenderer>();

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
                    chargeSFXInstance = SFXManager.instance.PlaySFXClip(chargeStartSFX, transform, chargeStartSFXVolume);
                    wasCharging = true;
                }

                currentCharge += Time.deltaTime;
                currentCharge = Mathf.Clamp(currentCharge, 0, maxChargeTime + critWindow + 0.1f);

                float chargePercent = Mathf.Clamp01(currentCharge / maxChargeTime);

                if(chargePercent >= 1 && !fullChargeReached)
                {
                    SFXManager.instance.PlaySFXClip(fullChargeSFX, transform, fullChargeSFXVolume);
                    fullChargeReached = true;
                }

                if(currentCharge >= minChargeTime)
                UpdateArrowVisuals(chargePercent);
                return;
            }

            if (playerInputHandler.fire1Released)
            {
                State = ShootState.ChargedRelease;
            }
        }

        private void UpdateArrowVisuals(float chargePercent)
        {
            //UpdateArrowColor(chargePercent);

            // ArrowHead
            arrowHeadSR.enabled = true;
            float maxHeadRange = Mathf.Lerp(minProjectileRange, maxProjectileRange, chargePercent);
            Vector3 newHeadPosition = new Vector3(maxHeadRange, 0f, 0f);
            arrowHeadTransform.transform.localPosition = newHeadPosition + new Vector3(headOffset, 0, 0);

            // ArrowShaft
            arrowShaftSR.enabled = true;
            Vector3 scale = arrowSprite.transform.localScale;
            scale.x = Mathf.Lerp(minProjectileRange, maxProjectileRange, chargePercent);
            arrowShaftTransform.transform.localScale = scale + new Vector3(shaftOffset, -0.25f, 0);

            float maxShaftRange = Mathf.Lerp(minProjectileRange, maxProjectileRange, chargePercent);
            Vector3 newShaftPosition = new Vector3(maxShaftRange, 0f, 0f);
            arrowShaftTransform.transform.localPosition = (newShaftPosition + new Vector3(shaftOffset, 0, 0))  / 2f;

            if(chargePercent >= 1f && !isFlashing && !hasFlashed)
            {
                isFlashing = true;
                isIncreasingAlpha = true;
                flashTimer = flashDuration;

                SetArrowColorAlpha(50);
            }

            if (isFlashing && !hasFlashed)
            {
                flashTimer -= Time.deltaTime;

                if(isIncreasingAlpha)
                {
                    float alpha = Mathf.Lerp(50f, 255f, 1f - (flashTimer / flashDuration));

                    SetArrowColorAlpha(alpha);

                    if(flashTimer <= 0f)
                    {
                        isIncreasingAlpha = false;
                        flashTimer = flashDuration;
                    }
                }
                else
                {
                    float alpha = Mathf.Lerp(255f, 50f, 1f - (flashTimer / flashDuration));

                    SetArrowColorAlpha(alpha);

                    if(flashTimer <= 0f)
                    {
                        SetArrowColorAlpha(50);
                        hasFlashed = true;
                        isFlashing = false;
                    }
                }
            }
        }

        private void SetArrowColorAlpha(float alpha)
        {
            Color headColor = arrowHeadSR.color;
            headColor.a = alpha / 255f;
            arrowHeadSR.color = headColor;

            Color shaftColor = arrowShaftSR.color;
            shaftColor.a = alpha / 255f;
            arrowShaftSR.color = shaftColor;
        }

        private void UpdateArrowColor(float chargePercent)
        {
            if (!isFlashing) 
            {
                //arrowSprite.color = Color.Lerp(minChargeColor, maxChargeColor, chargePercent);
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
            arrowHeadSR.enabled = false;
            arrowShaftSR.enabled = false;

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
                SFXManager.instance.PlaySFXClip(shotCritSFX, transform, shotCritSFXVolume);
            }
            else
            {
                SFXManager.instance.PlayRandomSFXClip(shotNormalSFX, transform, shotNormalSFXVolume);
            }

            // Instantiate Projectile
            GameObject projectile = Instantiate(projectileToUse, firePoint.position, firePoint.rotation);
            PlayerProjectile playerProjectile = projectile.GetComponent<PlayerProjectile>();

            // Damage Calculation
            int damage;
            bool pierce;
            if(chargePercent < 1)
            {
                damage = Mathf.RoundToInt(projectileBaseDamage * chargePercent);
                pierce = true;
            }
            else
            {
                if(charge <= maxChargeTime + critWindow)
                {
                    damage = Mathf.RoundToInt(projectileBaseDamage * 1.2f);
                    pierce = true;
                }
                else
                {
                    damage = projectileBaseDamage;
                    pierce = true;
                }
            }

            float knockbackForce = Mathf.Lerp(minKnockbackForce, maxKnockbackForce, chargePercent);
            float projectileRange = Mathf.Lerp(minProjectileRange, maxProjectileRange, chargePercent);

            playerProjectile.Initialize(firePoint.right, damage, projectileSpeed, projectileRange, knockbackForce, knockbackDuration, pierce); // pierce is set to true for now. We'll see if we change it.
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
            isFlashing = false;

            SetArrowColorAlpha(50);
        }

        //-----------------------
        private void ResetChargeVisuals()
        {
            arrowHeadSR.enabled = false;
            arrowShaftSR.enabled = false;

            //arrowSprite.color = baseColor;
            hasFlashed = false;
            flashTimer = 0f;

            SetArrowColorAlpha(50);
        }

        private void UpdateCooldowns()
        {
            fireCooldownTimer += Time.deltaTime;
        }
    }
}