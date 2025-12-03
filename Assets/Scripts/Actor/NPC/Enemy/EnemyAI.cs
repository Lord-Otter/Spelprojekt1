using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform target;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        agent.SetDestination(target.position);
    }
}
