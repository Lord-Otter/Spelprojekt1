using UnityEngine;
using UnityEngine.Splines;

namespace Spelprojekt1
{
    public class PlayerSpriteFlipper : MonoBehaviour
    {
        private SpriteRenderer playerSprite;
        private PlayerInputHandler input;
        private AimController aimController;
        private MovementController movement;

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
            movement = GetComponent<MovementController>();
        }

        void Update()
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
            
            FlipSprite();
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
        }
    }
}
