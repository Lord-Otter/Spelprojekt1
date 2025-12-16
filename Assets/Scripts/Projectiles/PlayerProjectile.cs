using System.Runtime.CompilerServices;
using UnityEngine;

namespace Spelprojekt1
{
    public class PlayerProjectile : MonoBehaviour
    {
        [SerializeField] private float destroyTimer;

        [Header("Stats")]
        [SerializeField] private float speed;
        [SerializeField] private float damage;
        [SerializeField] private float damageFallOff;
        [SerializeField] private float range;
        [SerializeField] private float knockback;
        [SerializeField] private bool pierce;

        [Header("Collision")]
        [SerializeField] private LayerMask hitLayers;
        [SerializeField] private LayerMask pierceableLayers;
        
        private Rigidbody2D rb;
        private Vector2 direction = new Vector2(0, 1);
        private Vector2 startPosition;

        public virtual void Initialize(Vector2 direction, float damage, float range, float knockback, bool pierce)
        {
            this.damage = damage;
            this.range = range;
            this.knockback = knockback;
            this.pierce = pierce;

            rb.linearVelocity = direction * speed;
            startPosition = transform.position;

            Destroy(gameObject, destroyTimer);
        }

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            float traveledDistance = Vector2.Distance(startPosition, transform.position);
            if (traveledDistance >= range)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            int otherLayer = other.gameObject.layer;

            bool canHit = (hitLayers.value & (1 << otherLayer)) != 0;
            bool canPierce = (pierceableLayers.value & (1 << otherLayer)) != 0;

            if (!canHit && !canPierce)
                return;

            if (other.TryGetComponent(out HealthHandler health))
            {
                health.TakeDamage(Mathf.RoundToInt(damage));
            }

            if (!canPierce)
            {
                Destroy(gameObject);
            }
        }
    }
}