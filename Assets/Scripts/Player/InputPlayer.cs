using UnityEngine;
using UnityEngine.InputSystem;

namespace Spelprojekt1
{
    public class InputPlayer : InputBase
    {
        private PlayerControls controls;

        [HideInInspector] public Vector2 aimInput;
        [HideInInspector] public Vector2 mousePosition;

        public bool fire1Pressed;

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

        public void onAim(InputAction.CallbackContext ctx)
        {
            aimInput = ctx.ReadValue<Vector2>();
        }

        public void OnMousePosition(InputAction.CallbackContext ctx)
        {
            mousePosition = ctx.ReadValue<Vector2>();
        }

        public void OnFire1(InputAction.CallbackContext ctx)
        {
            if(ctx.performed) fire1Pressed = true;
            if(ctx.canceled) fire1Pressed = false;
        }
        //private void OnEnable() => controls.Enable();
        //private void OnDisable() => controls.Disable();
        
    }   
}