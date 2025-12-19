using UnityEngine;
using UnityEngine.AI;
using Pathfinding;
using Unity.VisualScripting;

namespace Spelprojekt1
{
    public class EnemyAI : MonoBehaviour
    {
        private Transform player;
        private Rigidbody2D rb;
        private Seeker seeker;
        private AIPath aiPath;
        private Path path;
        private AIDestinationSetter aiDestinationSetter;

        //[SerializeField] private float speed = 200f; // Unused part of path following
        private bool isKnockBack = false;
        private float knockbackTimer = 0f;

        //[SerializeField] private float nextWaypointDistance = 3f; // Unused part of path following
        
        int currentWaypoint = 0;
        bool reachedEndOfPath = false;

        [Header("Line of Sight Settings")]
        [SerializeField] private float sightRange = 10f;
        [SerializeField] private float sightHeight = 0.2f;
        [SerializeField] private float sightWidth = 0.2f;
        [SerializeField] private LayerMask obstacleLayer;

        [Header("Pathfinding")]
        [SerializeField] protected float endReachedDistanceInSight = 5f;
        [SerializeField] protected float endReachedDistanceOutOfSight = 2f;
        private bool playerInSightLastFrame;
        

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            seeker = GetComponent<Seeker>();
            aiPath = GetComponent<AIPath>();
            aiDestinationSetter = GetComponent<AIDestinationSetter>();
        }

        void Start()
        {
            player = GameObject.Find("Player").GetComponent<Transform>();

            aiDestinationSetter.target = player;
        }

        void Update()
        {
            // Knockback
            if (isKnockBack)
            {
                aiPath.enabled = false;  // Disable pathfinding while the knockback is applied
                knockbackTimer -= Time.deltaTime;

                if (knockbackTimer <= 0)
                {
                    isKnockBack = false;
                    aiPath.enabled = true;  // Re-enable pathfinding after knockback ends
                }
            }

            // Adjust Max Distance to Player
            bool playerInSight = IsPlayerInSight();

            if(playerInSight != playerInSightLastFrame)
            {
                OnSightStateChanged(playerInSight);
                playerInSightLastFrame = playerInSight;
            }

        }

        public void ApplyKnockback(Vector2 knockbackDirection, float knockbackForce, float duration)
        {
            isKnockBack = true;
            knockbackTimer = duration;

            rb.linearVelocity = knockbackDirection * knockbackForce;
        }

        private void OnSightStateChanged(bool inSight)
        {
            if (aiPath == null)
                return;

            aiPath.endReachedDistance = inSight ? endReachedDistanceInSight : endReachedDistanceOutOfSight;
        }

        public bool IsPlayerInSight()
        {
            if (player == null)
                return false;

            Vector2 origin = transform.position;

            Vector2 targetPoint = (Vector2)player.position + Vector2.up * -0.4f;
            Vector2 direction = targetPoint - origin;
            float distance = direction.magnitude;
            direction.Normalize();

            Vector2 boxSize = new Vector2(sightWidth, sightHeight);

            RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, direction, distance, obstacleLayer);

            DrawBoxCast(origin, boxSize, direction, distance, hit ? Color.red : Color.green);

            return hit.collider == null;
        }

        void DrawBoxCast(Vector2 origin, Vector2 size, Vector2 direction, float distance, Color color)
        {
            Vector2 halfSize = size * 0.5f;

            Vector2 start = origin;
            Vector2 end = origin + direction * distance;

            Vector2 perp = new Vector2(-direction.y, direction.x);

            Vector2 sTL = start + perp * halfSize.x + Vector2.up * halfSize.y;
            Vector2 sTR = start - perp * halfSize.x + Vector2.up * halfSize.y;
            Vector2 sBL = start + perp * halfSize.x - Vector2.up * halfSize.y;
            Vector2 sBR = start - perp * halfSize.x - Vector2.up * halfSize.y;

            Vector2 eTL = sTL + direction * distance;
            Vector2 eTR = sTR + direction * distance;
            Vector2 eBL = sBL + direction * distance;
            Vector2 eBR = sBR + direction * distance;

            Debug.DrawLine(sTL, sTR, color);
            Debug.DrawLine(sTR, sBR, color);
            Debug.DrawLine(sBR, sBL, color);
            Debug.DrawLine(sBL, sTL, color);

            Debug.DrawLine(eTL, eTR, color);
            Debug.DrawLine(eTR, eBR, color);
            Debug.DrawLine(eBR, eBL, color);
            Debug.DrawLine(eBL, eTL, color);

            Debug.DrawLine(sTL, eTL, color);
            Debug.DrawLine(sTR, eTR, color);
            Debug.DrawLine(sBL, eBL, color);
            Debug.DrawLine(sBR, eBR, color);
        }
    }
}