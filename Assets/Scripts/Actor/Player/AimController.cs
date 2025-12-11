using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Spelprojekt1
{
    public class AimController : MonoBehaviour
    {
        private Transform aimerTransform;
        [SerializeField] private RotationMode rotationMode;
        public RotationMode CurrentMode => rotationMode;
        private PlayerInputHandler playerInputHandler;
        private Camera mainCamera;

        public float aimAngle { get; private set; }

        public enum RotationMode
        {
            StickAim,
            MouseAim
        }

        void Awake()
        {
            playerInputHandler = GetComponent<PlayerInputHandler>();
            aimerTransform = transform.Find("Aimer").GetComponent<Transform>();

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        void Update()
        {
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

        private void MoveRotation()
        {
            Vector2 move = playerInputHandler.moveInput;
            if(move.sqrMagnitude < 0.01f)
            {
                return;
            }

            aimAngle = Mathf.Repeat(Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg + 360f, 360f);
            aimerTransform.rotation = Quaternion.Euler(0, 0, aimAngle);
        }

        private void StickRotation()
        {
            Vector2 aim = playerInputHandler.aimInput;
            if(aim.sqrMagnitude < 0.01f)
            {
                return;
            }

            float angle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
            aimerTransform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void MouseRotation()
        {
            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(playerInputHandler.mousePosition);
            Vector2 direction = mouseWorld - transform.position;

            aimAngle = Mathf.Repeat(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 360f, 360f);
            aimerTransform.rotation = Quaternion.Euler(0, 0, aimAngle);
        }

        public void SetMode(RotationMode newMode) => rotationMode = newMode;
    }
}