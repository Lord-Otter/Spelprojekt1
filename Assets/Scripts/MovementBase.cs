using System.Net;
using UnityEngine;

namespace Spelprojekt1
{
    public class MovementBase : MonoBehaviour
    {
        [SerializeField] protected InputBase input;
        protected Rigidbody2D rb;

        [SerializeField] protected float maxSpeed = 5f;
        [SerializeField] protected float acceleration = 12f;

        private Vector2 currentVelocity;

        protected virtual void Awake()
        {
            input = GetComponent<InputBase>();
            rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            
        }

        protected virtual void FixedUpdate()
        {
            Move();
        }

        protected virtual void Move()
        {
            Vector2 target = input.moveInput * maxSpeed;
            
            currentVelocity = Vector2.Lerp(currentVelocity, target, acceleration * Time.fixedDeltaTime);

            rb.linearVelocity = currentVelocity;
        }
    }
}