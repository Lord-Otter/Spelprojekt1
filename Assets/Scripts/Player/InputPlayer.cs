using UnityEngine;
using UnityEngine.InputSystem;

namespace Spelprojekt1
{
    public class InputPlayer : InputBase
    {
        private PlayerControls controls;

        [HideInInspector] public Vector2 aimInput;
        [HideInInspector] public Vector2 mousePosition;

        public bool fireMousePressed;
        public bool fireStickPressed;
        public bool fireMovePressed;

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

        public void OnFireMouse(InputAction.CallbackContext ctx)
        {
            if(ctx.performed) fireMousePressed = true;
            if(ctx.canceled) fireMousePressed = false;
        }

        public void OnFireStick(InputAction.CallbackContext ctx)
        {
            if(ctx.performed) fireStickPressed = true;
            if(ctx.canceled) fireStickPressed = false;
        }

        public void OnFireMove(InputAction.CallbackContext ctx)
        {
            if(ctx.performed) fireMovePressed = true;
            if(ctx.canceled) fireMovePressed = false;
        }
        //private void OnEnable() => controls.Enable();
        //private void OnDisable() => controls.Disable();
        
    }   
}
