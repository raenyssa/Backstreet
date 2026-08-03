using UnityEngine;
using UnityEngine.AI;

public class JayWalkNPC : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5.0f;
    public float fleeDistance = 5.0f;
    public float runSpeed = 6.0f;
    public float walkSpeed = 3.5f;
    public float catchDistance = 1.0f;

    public Transform leftPoint;
    public Transform rightPoint;

    private NavMeshAgent agent;
    private Transform currentPatrolTarget;
    private bool isCaught = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentPatrolTarget = leftPoint;
        agent.speed = walkSpeed;
    }

    void Update()
    {
        if (isCaught) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= catchDistance)
        {
            TriggerCaughtState();
            return;
        }

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

        Vector3 directionAwayFromPlayer = transform.position - player.position;
        directionAwayFromPlayer.y = 0;
        directionAwayFromPlayer.Normalize();

        Vector3 fleeTargetPosition = transform.position + (directionAwayFromPlayer * fleeDistance);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeTargetPosition, out hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void PatrolBetweenPoints()
    {
        agent.speed = walkSpeed;

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

    void TriggerCaughtState()
    {
        isCaught = true;

        agent.isStopped = true;
        agent.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Debug.Log("NPC has been Caught!");

        // TODO: Trigger caught animation or UI screen here
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == player && !isCaught)
        {
            TriggerCaughtState();
        }
    }
}