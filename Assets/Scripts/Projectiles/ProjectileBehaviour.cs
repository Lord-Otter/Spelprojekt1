using Spelprojekt1;
using UnityEngine;

namespace Spelprojekt1
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        [SerializeField] private float destroyTimer;
        [SerializeField] private float damage;
        [SerializeField] private float knockback;
        [SerializeField] private float range;

        private Vector2 direction = new Vector2(0, 1);

        public void Initialize(float damage, float knockback, float range)
        {
            this.damage = damage;
            this.knockback = knockback;
            this.range = range;
        }

        void Awake()
        {
            
        }

        void Start()
        {
            Destroy(gameObject, destroyTimer);
        }

        void Update()
        {
            
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.CompareTag("Player"))
            {
                if(other.CompareTag("Enemy"))
                {
                    HealthHandler enemyHealth = other.GetComponent<HealthHandler>();
                    enemyHealth.TakeDamage((int)damage);
                }
                Destroy(gameObject);
            }
        }
    }
}