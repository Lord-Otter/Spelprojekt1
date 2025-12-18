using System.Collections.Generic;
using UnityEngine;

namespace Spelprojekt1
{
    public class EnemyShooter : MonoBehaviour
    {
        private EnemyAI enemyAI;
        [SerializeField] private Animator animator;

        private string currentAnimation;
        private string lastDirection = "down";
        private const float deadzone = 0.1f;

        [Header("Shooting Settings")]
        [SerializeField] private GameObject projectilePrefab;
        private Transform aimer;
        private Transform player;
        public Transform firePoint;

        [SerializeField] private int projectileDamage;
        [SerializeField] private float projectileSpeed;
        [Tooltip("Minimum time between shots")]
        [SerializeField] private float minShootTime = 1f;

        [Tooltip("Maximum time between shots")]
        public float maxShootTime = 3f;

        private ShooterState currentState = ShooterState.Ready;
        [SerializeField] private float aimTime;
        [SerializeField] private float holdingTime;
        private float recoveryTime;
        private float stateTimer;

        [Header("Audio")]
        [SerializeField] [Range(0, 1)] private float shootSoundsVolume;
        [SerializeField] private List<AudioClip> shootSounds;

        public enum ShooterState
        {
            Ready,
            Aiming,
            Shooting,
            Holding,
            Recovery
        }

        void Awake()
        {
            enemyAI = GetComponent<EnemyAI>();
            animator = GetComponentInChildren<Animator>();
            aimer = transform.Find("Aimer");
        }

        void Start()
        {
            player = GameObject.Find("Player").transform;
        }

        void Update()
        {
            stateTimer -= Time.deltaTime;
            RotateAimerTowardsPlayer();
            switch (currentState)
            {
                case ShooterState.Ready:
                    if (enemyAI.IsPlayerInSight())
                    {
                        TransitionToAiming();
                    }
                    break;

                case ShooterState.Aiming:
                    if (stateTimer <= 0f)
                    {
                        TransitionToShooting();
                    }
                    break;

                case ShooterState.Shooting:
                    Shoot();
                    TransitionToHolding();
                    break;

                case ShooterState.Holding:
                    if (stateTimer <= 0f)
                    {
                        TransitionToRecovery();
                    }
                    break;

                case ShooterState.Recovery:
                    if (stateTimer <= 0f)
                    {
                        TransitionToReady();
                    }
                    break;
            }
        }

        private void TransitionToAiming()
        {
            currentState = ShooterState.Aiming;
            stateTimer = aimTime;
        }

        private void TransitionToShooting()
        {
            currentState = ShooterState.Shooting;
        }

        private void TransitionToHolding()
        {
            currentState = ShooterState.Holding;
            stateTimer = holdingTime;
        }

        private void TransitionToRecovery()
        {
            currentState = ShooterState.Recovery;
            recoveryTime = Random.Range(minShootTime, maxShootTime);
            stateTimer = recoveryTime;
        }

        private void TransitionToReady()
        {
            currentState = ShooterState.Ready;
        }

        private void Shoot()
        {
            SFXManager.instance.PlayRandomSFXClip(shootSounds, transform, shootSoundsVolume);

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            EnemyProjectile projectileBehaviour = projectile.GetComponent<EnemyProjectile>();

            projectileBehaviour.Initialize(projectileDamage, projectileSpeed);
            projectileBehaviour.SetDirection(firePoint.right);
        }

        private void RotateAimerTowardsPlayer()
        {
            Vector3 directionToPlayer = player.position - aimer.position;
            directionToPlayer.z = 0f;

            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            aimer.rotation = Quaternion.Euler(0f, 0f, angle);

            SetMovementAnimation();
        }

        private void SetMovementAnimation()
        {
            Vector3 directionToPlayer = player.position - transform.position;
            string moveDir;

            if (currentState == ShooterState.Aiming || currentState == ShooterState.Shooting || currentState == ShooterState.Holding)
            {
                moveDir = GetDirectionFromAngle(aimer.rotation.eulerAngles.z);
            }
            else
            {
                moveDir = GetDirectionFromVelocity(directionToPlayer);
            }

            string anim;
            if(currentState == ShooterState.Aiming || currentState == ShooterState.Shooting || currentState == ShooterState.Holding)
            {
                anim = $"aim_{moveDir}";
            }
            else
            {
                anim = $"run_{moveDir}";
            }
            PlayAnimation(anim);
        }

        private void PlayAnimation(string animName)
        {
            if (currentAnimation == animName)
                return;

            currentAnimation = animName;
            animator.Play(animName);
        }

        private string GetDirectionFromVelocity(Vector3 velocity)
        {
            if (Mathf.Abs(velocity.x) >= Mathf.Abs(velocity.y))
                return velocity.x > 0 ? "right" : "left";
            else
                return velocity.y > 0 ? "up" : "down";
        }

        private string GetDirectionFromAngle(float angle)
        {
            angle = Mathf.Repeat(angle + 360f, 360f);

            if (angle >= 45f && angle < 135f) return "up";
            if (angle >= 135f && angle < 225f) return "left";
            if (angle >= 225f && angle < 315f) return "down";
            return "right";
        }
    }
}
