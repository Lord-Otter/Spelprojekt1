using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

namespace Spelprojekt1
{
    public class AimController : MonoBehaviour
    {
        private Transform aimerTransform;
        [HideInInspector] public PlayerInputHandler inputHandler;
        private Camera mainCamera;

        [SerializeField] private RotationMode rotationMode;
        public RotationMode CurrentMode => rotationMode;

        public float aimAngle { get; private set; }

        private RotationMode lastAimSource = RotationMode.MouseAim;
        private Vector3 lastMouseWorldPos;
        private const float STICK_DEADZONE = 0.1f;

        [SerializeField] private float moveStickFallBackDelay = 0.2f;
        private float aimStickReleaseTimer;

        public enum RotationMode
        {
            MouseAim,
            StickAim
        }

        void Awake()
        {
            inputHandler = GetComponent<PlayerInputHandler>();
            aimerTransform = transform.Find("Aimer");

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        void Update()
        {
            UpdateRotationMode();

            switch (rotationMode)
            {                
                case RotationMode.MouseAim:
                    MouseRotation();
                    break;

                case RotationMode.StickAim:
                    StickRotation();
                    break;
            }
        }

        private void UpdateRotationMode()
        {
            // Check last used control scheme
            if (inputHandler.lastDevice == PlayerInputHandler.LastInputDevice.Gamepad)
            {
                lastAimSource = RotationMode.StickAim;
            }
            else if (inputHandler.lastDevice == PlayerInputHandler.LastInputDevice.KBM)
            {
                lastAimSource = RotationMode.MouseAim;
            }

            rotationMode = lastAimSource;
        }

        /*private void MoveRotation()
        {
            Vector2 move = inputHandler.moveInput;
            if(move.sqrMagnitude < 0.01f)
            {
                return;
            }

            aimAngle = Mathf.Repeat(Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg + 360f, 360f);
            aimerTransform.rotation = Quaternion.Euler(0, 0, aimAngle);
        }*/

        private void MouseRotation()
        {
            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(inputHandler.mousePosition);
            Vector2 direction = mouseWorld - transform.position;

            aimAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            aimerTransform.rotation = Quaternion.Euler(0, 0, aimAngle);
        }

        private void StickRotation()
        {
            Vector2 aimStick = inputHandler.aimStick;
            
            bool aimStickActive = aimStick.sqrMagnitude > STICK_DEADZONE * STICK_DEADZONE;

            if (aimStickActive)
            {
                aimStickReleaseTimer = moveStickFallBackDelay;

                aimAngle = Mathf.Atan2(aimStick.y, aimStick.x) * Mathf.Rad2Deg;
                aimerTransform.rotation = Quaternion.Euler(0, 0, aimAngle);
                return;
            }

            if (aimStickReleaseTimer > 0f)
            {
                aimStickReleaseTimer -= Time.deltaTime;

                aimerTransform.rotation = Quaternion.Euler(0, 0, aimAngle);
                return;
            }

            
            MoveRotation();
        }

        private void MoveRotation()
        {
            Vector2 moveStick = inputHandler.moveInput;

            bool moveStickActive = moveStick.sqrMagnitude > STICK_DEADZONE * STICK_DEADZONE;

            if (moveStickActive)
            {
                aimAngle = Mathf.Atan2(moveStick.y, moveStick.x) * Mathf.Rad2Deg;
                aimerTransform.rotation = Quaternion.Euler(0, 0, aimAngle);
            }
            else
            {
                aimerTransform.rotation = Quaternion.Euler(0, 0, aimAngle);
            }
        }

        public void SetMode(RotationMode newMode) => rotationMode = newMode;
    }
}