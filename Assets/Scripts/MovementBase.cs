using System.Net;
using UnityEngine;

namespace Spelprojekt1
{
    public class MovementBase : MonoBehaviour
    {
        private TimeManager timeManager;
        public InputBase input;

        [SerializeField] private Transform selfTransform;
        [SerializeField] private Rigidbody2D rb;

        private Vector2 velocity;
        private Vector2 unscaledVelocity;


        [SerializeField] private bool canMove = true;
        [SerializeField] private float maxMoveSpeed;
        [SerializeField] private float moveSpeed;

        protected virtual void Awake()
        {
            timeManager = GameObject.Find("GameManager").GetComponent<TimeManager>();
            input = GetComponent<InputBase>();

            selfTransform = GetComponent<Transform>();
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
            Movement();
        }

        protected virtual void Movement()
        {
            velocity = rb.linearVelocity;

            float horizontalSpeed;
            float verticalSpeed;

            //if(Mathf.Abs())

            rb.linearVelocity = velocity;
        }
    }
}