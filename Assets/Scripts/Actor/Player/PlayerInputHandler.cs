using UnityEngine;
using UnityEngine.InputSystem;

namespace Spelprojekt1
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public enum ControlState
        {
            KBM,
            Gamepad
        }

        public ControlState CurrentControlState { get; private set; }

        private PlayerInput playerInput;

        [HideInInspector] public Vector2 aimInput;
        [HideInInspector] public Vector2 mousePosition;
        [HideInInspector] public Vector2 mouseDelta;

        [Header("Shooting")]
        public bool fire1Pressed;
        public bool fire1Held;
        public bool fire1Released;

        [Header("Movement")]
        public Vector2 moveInput { get; protected set; }
        public bool dashPressed;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            UpdateControlState();
        }
        private void OnEnable()
        {
            playerInput.onControlsChanged += OnControlsChanged;
        }

        private void OnDisable()
        {
            playerInput.onControlsChanged -= OnControlsChanged;
        }

        private void LateUpdate()
        {
            fire1Pressed = false;
            fire1Released = false;
            dashPressed = false;
        }

        private void OnControlsChanged(PlayerInput input)
        {
            UpdateControlState();
        }

        private void UpdateControlState()
        {
            switch (playerInput.currentControlScheme)
            {
                case "KBM":
                    CurrentControlState = ControlState.KBM;
                    break;
                
                case "Gamepad":
                    CurrentControlState = ControlState.Gamepad;
                    break;
            }
        }

        #region Player Action Map
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
            Vector2 newPos = ctx.ReadValue<Vector2>();
            mouseDelta = newPos - mousePosition;
            mousePosition = newPos;
        }

        // Shooting
        public void OnFire1(InputAction.CallbackContext ctx)
        {
            if(ctx.started)
            {
                fire1Pressed = true;
                fire1Held = true;
            }
            else if(ctx.canceled)
            {
                fire1Held = false;
                fire1Released = true;
            }
        }
        #endregion

        #region UI Action Map
        public void OnPause(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                PauseManager.Instance.TogglePause();
            }
        }
        #endregion
    }   
}