using System;
using UnityEngine;

namespace Spelprojekt1
{
    public class CameraTargetBehaviour : MonoBehaviour
    {
        [Header("References")]
        private Transform player;

        [Header("Settings")]
        [SerializeField] private float maxDistanceX = 3f;   // Max horizontal offset
        [SerializeField] private float maxDistanceY = 5f;   // Max vertical offset
        [SerializeField] [Range(0f, 1f)] private float mouseMultiplier = 0.1f; // Fraction of mouse offset applied
        [SerializeField] private float yBias = 1.5f;        // Bias vertical movement
        [SerializeField] private float speed;

        private Camera mainCamera;

        void Awake()
        {
            mainCamera = Camera.main;
            player = transform.parent;
        }

        void LateUpdate()
        {
            // Mouse position in world
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            // Vector from player to mouse
            Vector3 offset = mouseWorldPos - player.position;

            // Apply Y bias
            offset.y *= yBias;

            // Apply multiplier (percentage of offset)
            offset *= mouseMultiplier;

            // Clamp to max distances
            offset.x = Mathf.Clamp(offset.x, -maxDistanceX, maxDistanceX);
            offset.y = Mathf.Clamp(offset.y, -maxDistanceY, maxDistanceY);

            // Set camera target position
            //transform.position = player.position + offset;

            Vector3 targetPosition = player.position + offset;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}