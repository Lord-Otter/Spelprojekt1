using UnityEngine;
using UnityEngine.AI;
using Pathfinding;
using Unity.VisualScripting;

namespace Spelprojekt1
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [SerializeField] private float speed = 200f;
        [SerializeField] private float nextWaypointDistance = 3f;

        Path path;
        int currentWaypoint = 0;
        bool reachedEndOfPath = false;

        Seeker seeker;
        Rigidbody2D rb;

        void Awake()
        {
            seeker = GetComponent<Seeker>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            target = GameObject.Find("Player").GetComponent<Transform>();

            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }

        void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }

        void FixedUpdate()
        {
            if(path == null)
                return;

            if(currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if(distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
    }
}