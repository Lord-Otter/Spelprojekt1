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
        //protected PlayerInputHandler inputHandler; 

        protected Rigidbody2D rigidBody;
        protected Vector2 inputDirection;

        public bool MovementLocked { get; private set; }

        public void SetMovementLock(bool locked)
        {
            MovementLocked = locked;

            if(locked)
            {
                rigidBody.linearVelocity = Vector2.zero;
            }
        }

        protected virtual void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            
        }

        protected virtual void Update()
        {
            SetDirection();
        }

        protected virtual void FixedUpdate()
        {
            HandleMovement();
        }

        public virtual void SetInput(Vector2 direction)
        {
            inputDirection = direction;
        }

        protected virtual void HandleMovement()
        {
            if(MovementLocked)
            {
                rigidBody.linearVelocity = Vector2.zero;
                return;
            }

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

        protected virtual void SetDirection()
        {
            //inputDirection = inputHandler.moveInput;
        }
    }
}