using UnityEngine;
using UnityEngine.AI;

public class NPCRoaming : MonoBehaviour
{
    public float roamRadius = 10f;
    public float waitTime = 2f;
    
    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = waitTime;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Check if the NPC has reached its destination or if the wait timer is up
        if (timer >= waitTime && (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance))
        {
            Vector3 newPos = GetRandomNavMeshPoint(transform.position, roamRadius);
            agent.SetDestination(newPos);
            timer = 0f; // Reset timer
        }
    }

    public static Vector3 GetRandomNavMeshPoint(Vector3 center, float radius)
    {
        // 1. Generate a random point inside a sphere
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center;
        
        NavMeshHit hit;
        // 2. Project that point onto the NavMesh
        // Use NavMesh.AllAreas (or an integer bitmask) to sample the area
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        
        return center; // Fallback to current position if no point found
    }
}
