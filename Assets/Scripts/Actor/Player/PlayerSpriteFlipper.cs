using UnityEngine;
using UnityEngine.Splines;

namespace Spelprojekt1
{
    public class PlayerSpriteFlipper : MonoBehaviour
    {
        private SpriteRenderer playerSprite;
        private PlayerInputHandler input;
        private AimController aimController;
        private PlayerShooter shooter;
        private MovementController movement;

        private Rigidbody2D rb;

        private const int north = 90;
        private const int east = 0;
        private const int west = 180;
        private const int south = 270;

        private bool isFacingUp;
        private bool isFacingRight;

        void Awake()
        {
            playerSprite = transform.Find("Visual").GetComponent<SpriteRenderer>();
            input = GetComponent<PlayerInputHandler>();
            aimController = GetComponent<AimController>();
            shooter = GetComponent<PlayerShooter>();
            movement = GetComponent<MovementController>();

            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            DirectionCheck();
            FlipSprite();

            Debug.Log($"isFacingUp: {isFacingUp}");
            Debug.Log($"isFacingRight: {isFacingRight}");
        }

        private void DirectionCheck()
        {
            if (shooter.IsCharging) // While charging an attack
            {
                if(aimController.aimAngle > east && aimController.aimAngle < west) // When aiming upwards
                {
                    isFacingUp = true;
                }
                else if (aimController.aimAngle > west || aimController.aimAngle < east) // When aiming downwards
                {
                    isFacingUp = false;
                }

                if(aimController.aimAngle < north || aimController.aimAngle > south) // When aiming right
                {
                    isFacingRight = true;
                }
                else if(aimController.aimAngle > north && aimController.aimAngle < south || aimController.aimAngle < south && aimController.aimAngle < north) // When aiming left
                {
                    isFacingRight = false;
                }
            }
            else // While walking normally
            {
                if(rb.linearVelocity.x > 0)
                {
                    isFacingRight = true;
                }
                else if (rb.linearVelocity.x < 0)
                {
                    isFacingRight = false;
                }

                if(rb.linearVelocity.y > 0)
                {
                    isFacingUp = true;
                }
                else if(rb.linearVelocity.y < 0)
                {
                    isFacingUp = false;
                }
            }
            
        }

        private void FlipSprite()
        {
            if(isFacingRight)
            {
                playerSprite.flipX = false;
            }
            else
            {
                playerSprite.flipX = true;
            }

            if(isFacingUp)
            {
                // Change to upwards sprite
            }
            else
            {
                // Change to downwards sprite
            }
            
        }
    }
}
