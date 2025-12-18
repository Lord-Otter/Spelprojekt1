using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Spelprojekt1
{
    public class PlayerMovement : MonoBehaviour
    {
        public enum MoveState
        {
            Idle,
            Move,
            Dash
        }

        public MoveState State { get; private set; }

        private PlayerInputHandler inputHandler;        
        private PlayerShooter shooter;
        private Rigidbody2D rigidBody;
        private Collider2D hurtBox;
        [SerializeField] private CapsuleCollider2D[] collisionBoxes;

        private Vector2 inputDirection;

        [Header("Movement Settings")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float drag;

        [Header("Chargin Movement Adjustment")]
        [SerializeField] private float chargeMoveSpeedMultiplier = 0.5f;

        [Header("Dash Settings")]
        [SerializeField] private float dashSpeed;
        private Vector2 dashDirection;
        [SerializeField] private float dashDuration;
        private float dashTimer;
        [SerializeField] private float dashCooldown;
        private float dashCooldownTimer;
        [SerializeField] private LayerMask excludeLayersDuringDash;

        [Header("Audio")]
        [SerializeField] [Range(0, 1)] private float dashSoundVolume;
        [SerializeField] private List<AudioClip> dashSounds;
        [SerializeField] [Range(0, 1)] private float fabricSoundVolume; // Test
        [SerializeField] private List<AudioClip> fabricSounds; // Test

        public bool MovementLocked { get; private set; }

        public void SetMovementLock(bool locked)
        {
            MovementLocked = locked;
            if (locked)
            {
                rigidBody.linearVelocity = Vector2.zero;
            }
        }

        void Awake()
        {
            inputHandler = GetComponent<PlayerInputHandler>();
            shooter = GetComponent<PlayerShooter>();

            rigidBody = GetComponent<Rigidbody2D>();
            hurtBox = GetComponent<Collider2D>();
            collisionBoxes = GetComponentsInChildren<CapsuleCollider2D>();
        }

        void Start()
        {
            State = MoveState.Idle;
        }

        void Update()
        {
            inputDirection = inputHandler.moveInput;

            UpdateCooldowns();

            switch (State)
            {
                case MoveState.Idle:
                    UpdateIdle();
                    break;

                case MoveState.Move:
                    UpdateMove();
                    break;

                case MoveState.Dash:
                    UpdateDash();
                    break;
            }
        }

        void FixedUpdate()
        {
            switch (State)
            {
                case MoveState.Idle:
                case MoveState.Move:
                    MovementPhysics();
                    break;

                case MoveState.Dash:
                    DashPhysics();
                    break;
            }
        }

        #region State Update
        // Idle State
        private void UpdateIdle()
        {
            if (MovementLocked)
            {
                return;
            }

            if (inputDirection.sqrMagnitude > 0.01f)
            {
                State = MoveState.Move;
            }

            if (inputHandler.dashPressed)
            {
                StartDash();
            }
        }

        // Move State
        private void UpdateMove()
        {
            if (MovementLocked)
            {
                return;
            }

            if (inputDirection.sqrMagnitude <= 0.01f)
            {
                State = MoveState.Idle;
            }

            if (inputHandler.dashPressed)
            {
                StartDash();
            }
        }

        // Dash State
        private void StartDash()
        {
            if(dashCooldownTimer > 0)
                return;
            
            if(inputDirection.sqrMagnitude <= 0.01f)
                return;
            
            SFXManager.instance.PlayRandomSFXClip(dashSounds, transform, dashSoundVolume);
            SFXManager.instance.PlayRandomSFXClip(fabricSounds, transform, fabricSoundVolume);

            State = MoveState.Dash;
            dashTimer = dashDuration;

            dashDirection = inputDirection.sqrMagnitude > 0.01f ? inputDirection.normalized : rigidBody.linearVelocity.normalized;

            SetMovementLock(true); // Disable player movement
            hurtBox.enabled = false; // Disable hurtBox
            collisionBoxes[0].excludeLayers = LayerMask.GetMask("Enemy"); // Exclude "Enemy" Layer from Collisions
            collisionBoxes[1].excludeLayers = LayerMask.GetMask("Enemy"); // Exclude "Enemy" Layer from Collisions

            shooter.ApplyStun(dashDuration);
        }

        private void UpdateDash()
        {
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                SetMovementLock(false); // Enable player movement
                hurtBox.enabled = true; // Enable hurtBox
                collisionBoxes[0].excludeLayers &= ~LayerMask.GetMask("Enemy"); // Stop Excluding "Enemy Layer from Collision
                collisionBoxes[1].excludeLayers &= ~LayerMask.GetMask("Enemy"); // Stop Excluding "Enemy Layer from Collision

                dashCooldownTimer = dashCooldown;
                State = inputDirection.sqrMagnitude > 0.01f ? MoveState.Move : MoveState.Idle;
            }
        }
        #endregion

        private void MovementPhysics()
        {
            if(MovementLocked)
            {
                rigidBody.linearVelocity = Vector2.zero;
                return;
            }

            float speedMultiplier = 1f;
            if (shooter.State == PlayerShooter.ShootState.Charging)
            {
                speedMultiplier = chargeMoveSpeedMultiplier;
            }

            Vector2 targetVelocity;
            if (inputDirection.sqrMagnitude > 0.01f)
            {
                targetVelocity = inputDirection.normalized * maxSpeed * speedMultiplier;
                rigidBody.linearVelocity = Vector2.MoveTowards(rigidBody.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            }
            else
            {
                rigidBody.linearVelocity = Vector2.MoveTowards(rigidBody.linearVelocity, Vector2.zero, drag * Time.fixedDeltaTime);
            }
        }

        private void DashPhysics()
        {
            rigidBody.linearVelocity = dashDirection * dashSpeed;
        }

        private void UpdateCooldowns()
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }
}