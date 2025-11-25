using UnityEngine;

namespace Spelprojekt1
{
    public class VelocityScript : MonoBehaviour
    {
        public static Rigidbody2D rb;

        public static Vector2 velocity;
        public static Vector2 unscaledVelocity;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}