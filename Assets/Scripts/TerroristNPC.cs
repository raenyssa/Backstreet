using UnityEngine;
using UnityEngine.AI;
using System.Collections;

enum TerroristNPCState
{
    Idle,
    Running,
    Caught
}

public class TerroristNPC : MonoBehaviour
{
    [SerializeField] TerroristNPCState npcState = TerroristNPCState.Idle;
    public Transform player;
    public float detectionRange = 5.0f;
    public float fleeDistance = 5.0f;
    public float runSpeed = 4.0f;
    public float walkSpeed = 2f;
    public float catchDistance = 1.0f;
    private GameObject currentscore;

    public Transform leftPoint;
    public Transform rightPoint;
    public int score = 0;
    public string NPCName = "Terrorist";

    private NavMeshAgent agent;
    private Transform currentPatrolTarget;
    private bool isCaught = false;
    public UIManager MyUIManager; // Reference to the UIManager script

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
            if (agent.isOnNavMesh)
                {
                    agent.SetDestination(hit.position);
                }
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
            score += 5000;
            MyUIManager.UpdateScore(score);
            MyUIManager.UpdateCaughtPanel(NPCName, score);
            StartCoroutine(OpenCaughtPanelAfterDelay(1f));

        }
        else
        {
            return;
        }

        // TODO: Trigger caught animation or UI screen here
    }
    private IEnumerator OpenCaughtPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        MyUIManager.OpenCaughtPanel();
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
            npcState = TerroristNPCState.Caught;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            npcState = TerroristNPCState.Running;
        }
        else
        {
            npcState = TerroristNPCState.Idle;
        }

        switch (npcState)
        {
            case TerroristNPCState.Idle:
                PatrolBetweenPoints();
                break;
            case TerroristNPCState.Running:
                FleeFromPlayer();
                break;
            case TerroristNPCState.Caught:
                TriggerCaughtState();
                break;
        }

        //NPC caught 
        if (isCaught) return;
    }
}