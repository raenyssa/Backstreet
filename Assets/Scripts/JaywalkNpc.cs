using UnityEngine;
using UnityEngine.AI;

public class FleeNPC : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5.0f;
    public float fleeDistance = 5.0f;
    public float runSpeed = 6.0f;
    public float walkSpeed = 3.5f;

    public Transform leftPoint;
    public Transform rightPoint;

    private NavMeshAgent agent;
    private Transform currentPatrolTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentPatrolTarget = leftPoint;
        agent.speed = walkSpeed;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            FleeFromPlayer();
        }
        else
        {
            PatrolBetweenPoints();
        }
    }

    void FleeFromPlayer()
    {
        agent.speed = runSpeed;

        // Calculate direction away from the player
        Vector3 directionAwayFromPlayer = transform.position - player.position;
        directionAwayFromPlayer.y = 0; // Keep movement on a flat plane
        directionAwayFromPlayer.Normalize();

        // Target a position in that opposite direction
        Vector3 fleeTargetPosition = transform.position + (directionAwayFromPlayer * fleeDistance);

        // Verify the target position is valid on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeTargetPosition, out hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void PatrolBetweenPoints()
    {
        agent.speed = walkSpeed;

        // If the NPC was fleeing, it might have stopped. Ensure it has a destination.
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            if (currentPatrolTarget == leftPoint)
                currentPatrolTarget = rightPoint;
            else
                currentPatrolTarget = leftPoint;
        }

        if (currentPatrolTarget != null)
        {
            agent.SetDestination(currentPatrolTarget.position);
        }
    }
}
