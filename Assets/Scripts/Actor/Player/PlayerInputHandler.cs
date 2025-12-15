using UnityEngine;
using UnityEngine.InputSystem;

namespace Spelprojekt1
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public enum LastInputDevice
        {
            KBM,
            Gamepad
        }

        public LastInputDevice lastDevice { get; private set; } = LastInputDevice.Gamepad;

        private PlayerInput playerInput;

        [Header("Movement")]
        public Vector2 moveInput { get; protected set; }
        public bool dashPressed;

        [Header("Aiming")]
        public Vector2 aimStick;
        public Vector2 mousePosition;

        [Header("Shooting")]
        public bool fire1Pressed;
        public bool fire1Held;
        public bool fire1Released;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            if (playerInput.currentControlScheme == "KBM")
            {
                lastDevice = LastInputDevice.KBM;
                Cursor.visible = true;
            }
            else if(playerInput.currentControlScheme == "Gamepad")
            {
                lastDevice = LastInputDevice.Gamepad;
                Cursor.visible = false;
            }

            Debug.Log(lastDevice);
        }

        private void LateUpdate()
        {
            fire1Pressed = false;
            fire1Released = false;
            dashPressed = false;
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

        public void OnAimStick(InputAction.CallbackContext ctx)
        {
            aimStick = ctx.ReadValue<Vector2>();
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