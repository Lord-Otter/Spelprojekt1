using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Spelprojekt1
{
    public class AimController : MonoBehaviour
    {
        private Transform aimerTransform;
        private PlayerInputHandler inputHandler;
        private Camera mainCamera;

        [SerializeField] private RotationMode rotationMode;
        public RotationMode CurrentMode => rotationMode;

        public float aimAngle { get; private set; }

        private const float AIM_DEADZONE = 0.01f;
        private RotationMode lastAimSource = RotationMode.MouseAim;

        public enum RotationMode
        {
            StickAim,
            MouseAim
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
                case RotationMode.StickAim:
                    StickRotation();
                    break;
                
                case RotationMode.MouseAim:
                    MouseRotation();
                    break;
            }
        }

        private void UpdateRotationMode()
        {
            // Stick aim if using Gamepad
            if (inputHandler.CurrentControlState == PlayerInputHandler.ControlState.Gamepad)
            {
                rotationMode = RotationMode.StickAim;
            }
            // Mouse aim if using KBM
            else
            {
                rotationMode = RotationMode.MouseAim;
            }
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

        private void StickRotation()
        {
            Vector2 aim = inputHandler.aimInput;

            if (aim.sqrMagnitude < 0.01f)
            {
                aimerTransform.rotation = Quaternion.Euler(0, 0, aimAngle);
                return;
            }

            aimAngle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
            aimerTransform.rotation = Quaternion.Euler(0, 0, aimAngle);
        }

        private void MouseRotation()
        {
            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(inputHandler.mousePosition);
            Vector2 direction = mouseWorld - transform.position;

            aimAngle = Mathf.Repeat(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 360f, 360f);
            aimerTransform.rotation = Quaternion.Euler(0, 0, aimAngle);
        }

        public void SetMode(RotationMode newMode) => rotationMode = newMode;
    }
}