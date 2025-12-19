using System.Runtime.CompilerServices;
using UnityEngine;

namespace Spelprojekt1
{
    public class PlayerProjectile : MonoBehaviour
    {
        [SerializeField] private float destroyTimer;

        [Header("Stats")]
        [SerializeField] private float speed;
        [SerializeField] private int damage;
        [SerializeField] private int baseDamage;
        [SerializeField] private float damageFallOff;
        [SerializeField] private float range;
        [SerializeField] private float knockbackForce;
        [SerializeField] private float knockbackDuration;
        [SerializeField] private bool pierce;

        [Header("Collision")]
        [SerializeField] private LayerMask hitLayers;
        [SerializeField] private LayerMask pierceableLayers;
        
        private Rigidbody2D rb;
        private Vector2 direction = new Vector2(0, 1);
        private Vector2 startPosition;

        public void Initialize(Vector2 direction, int damage, float speed, float range, float knockbackForce, float knockbackDuration, bool pierce)
        {
            this.damage = damage;
            this.baseDamage = damage;
            this.speed = speed;
            this.range = range;
            this.knockbackForce = knockbackForce;
            this.knockbackDuration = knockbackDuration;
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

                damage = Mathf.RoundToInt(damage * (1 - damageFallOff));

                damage = Mathf.Max(damage, Mathf.RoundToInt(baseDamage * 0.3f));

            }

            if(other.TryGetComponent(out EnemyAI enemyAI))
            {
                float angleInRadians = Mathf.Deg2Rad * transform.eulerAngles.z;

                Vector2 knockbackDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized;

                enemyAI.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
            }

            if(other.TryGetComponent(out EnemyAIMelee enemyAIMelee))
            {
                float angleInRadians = Mathf.Deg2Rad * transform.eulerAngles.z;

                Vector2 knockbackDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized;

                enemyAIMelee.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
            }

            if (!canPierce)
            {
                Destroy(gameObject);
            }
        }
    }
}