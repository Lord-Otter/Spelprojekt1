using UnityEngine;

namespace Spelprojekt1
{
    public class PlayerMovement : MovementBase
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

        /*protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }*/

        protected override void FixedUpdate()
        {
            if(IsPlayerCharging())
            { 
                rb.linearVelocity = Vector2.zero;
                return;
            }
            if (IsPlayerShotRecovery())
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }
            base.FixedUpdate();
        }

        /*protected override void Move()
        {
            /*if(IsPlayerCharging())
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            base.Move();
        }*/

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
}


