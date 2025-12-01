using Spelprojekt1;
using UnityEngine;

namespace Spelprojekt1
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        [SerializeField] private float destroyTimer;
        [SerializeField] private float damage;
        [SerializeField] private float Knockback;
        [SerializeField] private float range;

        private Vector2 direction = new Vector2(0, 1);

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
                Destroy(gameObject);
            }
        }
    }
}