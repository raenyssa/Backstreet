using UnityEngine;
using UnityEngine.AI;

enum NPCState
{
    Idle,
    Running,
    Caught
}

public class JayWalkNPC : MonoBehaviour
{
    [SerializeField] NPCState npcState = NPCState.Idle;
    public Transform player;
    public float detectionRange = 5.0f;
    public float fleeDistance = 5.0f;
    public float runSpeed = 4.0f;
    public float walkSpeed = 2f;
    public float catchDistance = 1.0f;
    private GameObject currentscore;

    public Transform leftPoint;
    public Transform rightPoint;

    private NavMeshAgent agent;
    private Transform currentPatrolTarget;
    private bool isCaught = false;
    int score = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentPatrolTarget = leftPoint;
        agent.speed = walkSpeed;
    }

    void Update()
    {
        StateMachine();
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
        if (isCaught != true)
        {

            isCaught = true;

            agent.isStopped = true;
            agent.enabled = false;

            print(agent.isStopped);

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            Debug.Log("NPC has been Caught!");
            score += 1000;
            UIManager.Instance.UpdateScore(score);

        }
        else
        {
            return;
        }

        // TODO: Trigger caught animation or UI screen here
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == player && !isCaught)
        {
            TriggerCaughtState();
        }
    }


    void StateMachine()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (PlayerScript.IsCaught())
        {
            npcState = NPCState.Caught;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            npcState = NPCState.Running;
        }
        else
        {
            npcState = NPCState.Idle;
        }

        switch (npcState)
        {
            case NPCState.Idle:
                PatrolBetweenPoints();
                break;
            case NPCState.Running:
                FleeFromPlayer();
                break;
            case NPCState.Caught:
                TriggerCaughtState();
                break;
        }

        //NPC caught 
        if (isCaught) return;
    }
}