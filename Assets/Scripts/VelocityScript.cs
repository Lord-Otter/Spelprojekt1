using UnityEngine;

namespace Spelprojekt1
{
    public class Velocity : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;

        [HideInInspector] public Vector2 velocity;
        [HideInInspector] public Vector2 unscaledVelocity;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();   
        }

        void Start()
        {
            
        }

        void Update()
        {
            
        }
    }
}