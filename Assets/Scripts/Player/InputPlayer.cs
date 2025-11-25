using UnityEngine;
using UnityEngine.InputSystem;

namespace Spelprojekt1
{
    public class InputPlayer : InputBase
    {
        private PlayerControls controls;

        protected void Awake()
        {
            controls = new PlayerControls();

            /*controls.Player.Move.performed += ctx => SetMoveInput(ctx.ReadValue<Vector2>());
            controls.Player.Move.canceled += ctx => SetMoveInput(Vector2.zero);*/
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            if(ctx.performed)
            {
                SetMoveInput(ctx.ReadValue<Vector2>());
            }
            else if (ctx.canceled)
            {
                SetMoveInput(Vector2.zero);
            }
        }

        private void OnEnable() => controls.Enable();
        private void OnDisable() => controls.Disable();
        
    }   
}
