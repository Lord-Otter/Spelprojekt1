using UnityEngine;

namespace Spelprojekt1
{
    public class PlayerMovement : MovementBase
    {
        private CapsuleCollider2D collider;

        [SerializeField] private bool isDashing = false;
        [SerializeField] private bool canDash = true;
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashRange;
        [SerializeField] private float dashCooldown;

        protected override void Awake()
        {
            base.Awake();
            collider = GetComponent<CapsuleCollider2D>();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}


