using UnityEngine;
using UnityEngine.InputSystem;

namespace Spelprojekt1
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private PlayerControls controls;

        [HideInInspector] public Vector2 aimInput;
        [HideInInspector] public Vector2 mousePosition;

        [Header("Shooting")]
        public bool fire1Pressed;
        public bool fire1Held;
        public bool fire1Released;

        [Header("Movement")]
        public Vector2 moveInput { get; protected set; }
        public bool dashPressed;

        [Header("UI")]
        public bool menuButtonPressed;

        protected void Awake()
        {
            controls = new PlayerControls();
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

        private void LateUpdate()
        {
            fire1Pressed = false;
            fire1Released = false;
            dashPressed = false;
            menuButtonPressed = false;
        }

        // Movement
        public void OnMove(InputAction.CallbackContext ctx)
        {
            moveInput = ctx.ReadValue<Vector2>();
        }

        // Dash
                public void OnDash(InputAction.CallbackContext ctx)
                {
                    if (ctx.started)
                    {
                        dashPressed = true;
                    }
                }  

        // Aiming
        public void OnAim(InputAction.CallbackContext ctx)
        {
            aimInput = ctx.ReadValue<Vector2>();
        }

        public void OnMousePosition(InputAction.CallbackContext ctx)
        {
            mousePosition = ctx.ReadValue<Vector2>();
        }

        // Shooting
        public void OnFire1(InputAction.CallbackContext ctx)
        {
            if(ctx.started)
            {
                fire1Pressed = true;
                fire1Held = true;
                fire1Released = false;
            }
            else if(ctx.canceled)
            {
                fire1Held = false;
                fire1Released = true;
            }
        }

        // UI
        public void OnPause(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                menuButtonPressed = true;
            }
        }
        
    }   
}