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
        [SerializeField] private bool pierce;

        private Vector2 direction = new Vector2(0, 1);

        public void Initialize(float damage, float knockback, float range, bool pierce)
        {
            this.damage = damage;
            this.knockback = knockback;
            this.range = range;
            this.pierce = pierce;
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
            if(this.CompareTag("Projectile_Ally"))
            {
                if(!other.CompareTag("Player"))
                {
                    if(other.CompareTag("Enemy"))
                    {
                        HealthHandler enemyHealth = other.GetComponent<HealthHandler>();
                        enemyHealth.TakeDamage((int)damage);
                    }
                    if(!other.CompareTag("Projectile_Enemy") && !other.CompareTag("Projectile_Ally"))
                    {
                        Destroy(gameObject);
                    }
                    
                }
            }
            else if (this.CompareTag("Projectile_Enemy"))
            {
                if(!other.CompareTag("Enemy"))
                {
                    if(other.CompareTag("Player"))
                    {
                        HealthHandler enemyHealth = other.GetComponent<HealthHandler>();
                        enemyHealth.TakeDamage((int)damage);
                    }
                    if(!other.CompareTag("Projectile_Enemy") && !other.CompareTag("Projectile_Ally"))
                    {
                        Destroy(gameObject);
                    }
                }
            }
            
        }
    }
}