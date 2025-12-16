using System;
using UnityEngine;

namespace Spelprojekt1
{
    public class CameraTargetBehaviour : MonoBehaviour
    {
        [Header("References")]
        private Transform player;
        private AimController aimController;

        [Header("Mouse Settings")]
        [SerializeField] private float mouseMaxDistanceX = 2f;
        [SerializeField] private float mouseMaxDistanceY = 3f;
        [SerializeField] [Range(0f, 1f)] private float mouseMultiplier = 0.175f;
        [SerializeField] private float mouseYBias = 1.6f;
        [SerializeField] private float mouseSpeed = 20f;

        [Header("Stick Settings")]
        [SerializeField] private float stickMaxDistanceX = 2f;
        [SerializeField] private float stickMaxDistanceY = 2f;
        [SerializeField] private float stickYBias = 1.0f;
        [SerializeField] private float stickSpeed = 15f;


        private Camera mainCamera;

        void Awake()
        {
            mainCamera = Camera.main;
            player = transform.parent;
            aimController = GetComponentInParent<AimController>();
        }

        void LateUpdate()
        {
            Vector3 targetPosition = player.position;

            if(aimController.CurrentMode == AimController.RotationMode.MouseAim)
            {
                Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0f;

                Vector3 offset = mouseWorldPos - player.position;

                offset.y *= mouseYBias;
                offset *= mouseMultiplier;

                offset.x = Mathf.Clamp(offset.x, -mouseMaxDistanceX, mouseMaxDistanceX);
                offset.y = Mathf.Clamp(offset.y, -mouseMaxDistanceY, mouseMaxDistanceY);

                targetPosition += offset;

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, mouseSpeed * Time.deltaTime);
            }
            else if(aimController.CurrentMode == AimController.RotationMode.StickAim)
            {
                Vector2 stick = aimController.inputHandler.aimStick;

                Vector3 offset = new Vector3( stick.x * stickMaxDistanceX,  stick.y * stickMaxDistanceY * stickYBias, 0f);

                targetPosition += offset;

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, stickSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, stickSpeed * Time.deltaTime);
            }
            
        }
    }
}