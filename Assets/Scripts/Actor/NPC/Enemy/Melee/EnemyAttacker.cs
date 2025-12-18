using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Spelprojekt1
{
    public class EnemyAttacker : MonoBehaviour
    {
        private EnemyAIMelee enemyAIMelee;
        private AIPath aiPath;
        private Animator animator;
        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;

        private string currentAnimation;
        private string lastDirection = "down";
        private const float deadzone = 0.1f;

        [Header("Attack Settings")]
        private Transform aimer;
        private Transform player;

        [SerializeField] private int attackDamage;
        [SerializeField] private float attackRange = 5f;
        [SerializeField] private float attackMoveSpeed;
        [SerializeField] private float attackDuration;
        [Tooltip("Minimum time between shots")]
        [SerializeField] private float minAttackCooldown = 2f;

        [Tooltip("Maximum time between shots")]
        [SerializeField] private float maxAttackCooldown = 5f;
        private bool hasAttacked = false;

        private AttackState currentState = AttackState.Ready;
        [SerializeField] private float telegraphingTime;
        private float recoveryTime;
        private float stateTimer;

        [Header("Audio")]
        [SerializeField] [Range(0, 1)] private float shootSoundsVolume;
        [SerializeField] private List<AudioClip> shootSounds;

        public enum AttackState
        {
            Ready,
            Telegraphing,
            Attacking,
            Recovery
        }

        void Awake()
        {
            enemyAIMelee = GetComponent<EnemyAIMelee>();
            aiPath = GetComponent<AIPath>();
            animator = GetComponentInChildren<Animator>();
            aimer = transform.Find("Aimer");
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        void Start()
        {
            player = GameObject.Find("Player").transform;
        }

        void Update()
        {
            stateTimer -= Time.deltaTime;

            RotateAimerTowardsPlayer();

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            
            switch (currentState)
            {
                case AttackState.Ready:
                    if (enemyAIMelee.IsPlayerInSight() && distanceToPlayer <= attackRange)
                    {
                        TransitionToTelegraphing();
                    }
                    break;

                case AttackState.Telegraphing:
                    TelegraphAttack();
                    InterpolateColor();
                    if (stateTimer <= 0f)
                    {
                        TransitionToAttacking();
                    }
                    break;

                case AttackState.Attacking:
                    Attack();
                    if(stateTimer <= 0f)
                    {
                        TransitionToRecovery();
                    }
                    
                    break;

                case AttackState.Recovery:
                    if (stateTimer <= 0f)
                    {
                        TransitionToReady();
                    }
                    break;
            }
        }

        private void TransitionToTelegraphing()
        {
            currentState = AttackState.Telegraphing;
            stateTimer = telegraphingTime;
        }

        private void TransitionToAttacking()
        {
            hasAttacked = false;
            stateTimer = attackDuration;
            spriteRenderer.color = Color.white;
            rb.linearDamping = 0;
            currentState = AttackState.Attacking;    
        }

        private void TransitionToRecovery()
        {
            currentState = AttackState.Recovery;
            recoveryTime = Random.Range(minAttackCooldown, maxAttackCooldown);
            stateTimer = recoveryTime;
            rb.linearVelocity = Vector2.zero;
            aiPath.enabled = true;
            rb.linearDamping = 3;
        }

        private void TransitionToReady()
        {
            aiPath.enabled = true;
            currentState = AttackState.Ready;
            
        }

        private void TelegraphAttack()
        {
            aiPath.enabled = false;
            rb.linearVelocity = Vector2.zero;
        }

        private void Attack()
        {
            if(hasAttacked)
                return;

            aiPath.enabled = false;
            rb.linearVelocity = Vector2.zero;

            Vector2 directionToPlayer = (player.position - transform.position).normalized;

            float offsetAngle = 0f;

            Vector2 offsetDirection = new Vector2(
                directionToPlayer.x * Mathf.Cos(Mathf.Deg2Rad * offsetAngle) - directionToPlayer.y * Mathf.Sin(Mathf.Deg2Rad * offsetAngle),
                directionToPlayer.x * Mathf.Sin(Mathf.Deg2Rad * offsetAngle) + directionToPlayer.y * Mathf.Cos(Mathf.Deg2Rad * offsetAngle)
            );

            rb.linearVelocity = offsetDirection * attackMoveSpeed;

            hasAttacked = true;
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

            moveDir = GetDirectionFromAngle(aimer.rotation.eulerAngles.z);
            string anim;

            if (currentState == AttackState.Attacking)
            {
                anim = "attack";
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

        private void InterpolateColor()
        {
            // Calculate the normalized time in the telegraphing phase
            float normalizedTime = Mathf.Clamp01(stateTimer / telegraphingTime);

            // If in the first half of the telegraphing time, interpolate to red
            if (normalizedTime > 0.5f)
            {
                float secondHalfFactor = (normalizedTime - 0.5f) * 2f;  // Scale time for the second half (0 to 1)
                spriteRenderer.color = Color.Lerp(Color.red, Color.white, secondHalfFactor); // Fade back to white
            }
            else
            {
                float firstHalfFactor = normalizedTime * 2f;  // Scale time for the first half (0 to 1)
                spriteRenderer.color = Color.Lerp(Color.white, Color.red, firstHalfFactor); // Fade to red
            }
        }
    }
}