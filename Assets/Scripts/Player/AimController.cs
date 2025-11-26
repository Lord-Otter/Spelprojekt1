using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Spelprojekt1
{
    public class AimController : MonoBehaviour
    {
        [SerializeField] private RotationMode rotationMode;
        public RotationMode CurrentMode => rotationMode;
        private InputPlayer inputPlayer;
        private Camera mainCamera;

        public enum RotationMode
        {
            MoveAim,
            StickAim,
            MouseAim
        }

        void Awake()
        {
            inputPlayer = GetComponent<InputPlayer>();

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        void Update()
        {
            switch (rotationMode)
            {
                case RotationMode.MoveAim:
                    MoveRotation();
                    break;
                
                case RotationMode.StickAim:
                    StickRotation();
                    break;
                
                case RotationMode.MouseAim:
                    MouseRotation();
                    break;
            }
        }

        private void MoveRotation()
        {
            Vector2 move = inputPlayer.moveInput;
            if(move.sqrMagnitude < 0.01f)
            {
                return;
            }

            float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void StickRotation()
        {
            Vector2 aim = inputPlayer.aimInput;
            if(aim.sqrMagnitude < 0.01f)
            {
                return;
            }

            float angle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void MouseRotation()
        {
            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(inputPlayer.mousePosition);
            Vector2 direction = mouseWorld - transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        public void SetMode(RotationMode newMode) => rotationMode = newMode;
    }
}

