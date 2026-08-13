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
    [SerializeField] private float timeLimit = 10f;      // seconds allowed to catch the NPC
    [SerializeField] private GameObject Explosionvfx;    // assign your VFX object in the Inspector
    [SerializeField] private GameObject Dustvfx;    // assign your VFX object in the Inspector
    [SerializeField] private GameObject Flashvfx;    // assign your VFX object in the Inspector
    [SerializeField] private GameObject Sparksvfx;    // assign your VFX object in the Inspector
    

    private float timer;
    private bool isCaught = false;
    private bool timerRunning = false;
    [SerializeField] private GameObject objectToDrop;
    [SerializeField] private float dropDistance = 2f;

    private bool hasDropped = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentPatrolTarget = leftPoint;
        agent.speed = walkSpeed;
        StartTimer();
    }
    public void StartTimer()
    {
        timer = timeLimit;
        isCaught = false;
        timerRunning = true;

        if (Explosionvfx != null && Dustvfx!=null && Flashvfx!=null && Sparksvfx!=null)
            {
                Explosionvfx.SetActive(false);
                Dustvfx.SetActive(false);
                Flashvfx.SetActive(false);
                Sparksvfx.SetActive(false);
            }

        
    }

    void Update()
    {
        StateMachine();

        if (!hasDropped && player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= dropDistance)
            {
                DropObject();
            }
        }

        if (!timerRunning || isCaught) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timerRunning = false;
            OnTimeExpired();
        }
    }
    private void OnTimeExpired()
    {
            if (Explosionvfx != null && Dustvfx!=null && Flashvfx!=null && Sparksvfx!=null)
                {
                    Explosionvfx.SetActive(true);
                    Dustvfx.SetActive(true);
                    Flashvfx.SetActive(true);
                    Sparksvfx.SetActive(true);
                    StartCoroutine(OpenLostPanelAfterDelay(1f));
                }
        // optional: any other "failed" logic here, e.g. mission fail state
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
            timerRunning = false;
            if (Explosionvfx != null && Dustvfx!=null && Flashvfx!=null && Sparksvfx!=null)
                {
                    Explosionvfx.SetActive(false);
                    Dustvfx.SetActive(false);
                    Flashvfx.SetActive(false);
                    Sparksvfx.SetActive(false);
                }
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
            GameManager.instance.IncreaseScore(score);
            GameManager.instance.uiManager.UpdateCaughtPanel(NPCName, score);
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
        GameManager.instance.uiManager.OpenCaughtPanel();
    }
    private IEnumerator OpenLostPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.instance.uiManager.OpenLostPanel();
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
            npcState = TerroristNPCState.Idle;
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
        if (isCaught) 
        {
            return;
        }
    }
    private void DropObject()
    {
        hasDropped = true;

        if (objectToDrop == null)
            return;

        // Remove it from the NPC
        objectToDrop.transform.SetParent(null);

        // Make it affected by physics
        Rigidbody rb = objectToDrop.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        Debug.Log("NPC dropped the object!");
    }
}