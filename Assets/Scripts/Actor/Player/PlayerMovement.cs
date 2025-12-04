using UnityEngine;

namespace Spelprojekt1
{
    public class PlayerMovement : MovementController
    {
        private PlayerInputHandler inputHandler;

        protected override void Awake()
        {
            base.Awake();
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        protected override void SetDirection()
        {
            inputDirection = inputHandler.moveInput;
        }
    }
}


