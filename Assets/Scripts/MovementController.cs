using UnityEngine;

namespace Spelprojekt1
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float maxSpeed;
        public float acceleration;
        public float drag;

        [Header("Optional Input Source")]
        private PlayerInputHandler inputHandler; 

        private Rigidbody2D rigidBody;
        private Vector2 inputDirection;

        void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        void Update()
        {
            SetDirection();
        }

        void FixedUpdate()
        {
            HandleMovement();
        }

        public void SetInput(Vector2 direction)
        {
            inputDirection = direction;
        }

        void HandleMovement()
        {
            Vector2 targetVelocity = inputDirection.normalized * maxSpeed;

            if (inputDirection.sqrMagnitude > 0.01f)
            {
                rigidBody.linearVelocity = Vector2.MoveTowards(rigidBody.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            }
            else
            {
                rigidBody.linearVelocity = Vector2.MoveTowards(rigidBody.linearVelocity, Vector2.zero, drag * Time.fixedDeltaTime);
            }
        }

        void SetDirection()
        {
            inputDirection = inputHandler.moveInput;
        }
    }
}