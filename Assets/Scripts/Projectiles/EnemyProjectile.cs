using UnityEngine;

namespace Spelprojekt1
{
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 8f;
        [SerializeField] private float destroyTimer = 3f;
        [SerializeField] private int damage = 1;

        [Header("Collision")]
        [SerializeField] private LayerMask hitLayers;

        private Vector2 direction;
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            rb.linearVelocity = direction * speed;
            Destroy(gameObject, destroyTimer);
        }

        public void Initialize(int damage, float speed)
        {
            this.damage = damage;
            this.speed = speed;
        }

        public void SetDirection(Vector2 dir)
        {
            direction = dir.normalized;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((hitLayers.value & (1 << other.gameObject.layer)) == 0)
                return;

            if (other.TryGetComponent(out HealthHandler health))
            {
                health.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}