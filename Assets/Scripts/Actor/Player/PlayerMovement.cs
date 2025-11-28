/*using UnityEngine;

namespace Spelprojekt1
{
    public class PlayerMovement : MovementController
    {
        private PlayerShooter playerShooter;

        private CapsuleCollider2D collider;

        [SerializeField] private bool canMove;

        [SerializeField] private bool isDashing = false;
        [SerializeField] private bool canDash = true;
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashRange;
        [SerializeField] private float dashCooldown;

        protected override void Awake()
        {
            base.Awake();
            playerShooter = GetComponent<PlayerShooter>();
            collider = GetComponent<CapsuleCollider2D>();
        }

        protected override void FixedUpdate()
        {
            if(IsPlayerCharging())
            { 
                rigidBody.linearVelocity = Vector2.zero;
                return;
            }
            if (IsPlayerShotRecovery())
            {
                rigidBody.linearVelocity = Vector2.zero;
                return;
            }
            base.FixedUpdate();
        }

        protected override void HandleMovement()
        {
            if(IsPlayerCharging())
            {
                rigidBody.linearVelocity = Vector2.zero;
                return;
            }

            base.HandleMovement();
        }

        protected bool IsPlayerCharging()
        {
            if (playerShooter == null)
            {
                return false;
            }

            return playerShooter.IsCharging;
        }

        protected bool IsPlayerShotRecovery()
        {
            if (playerShooter == null)
            {
                return false;
            }

            return playerShooter.IsInShotRecovery;
        }
    }
}*/


