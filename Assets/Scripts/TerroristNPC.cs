/*
 * Author: Marilyn Tan
 * Date: 8th August 2026
 * File: TerroristNPC
 * Description:Controls the behaviour of the terrorist NPC, including patrolling, 
 fleeing from the player, dropping an object, and handling the mission timer.
 
 */
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// Represents the different states that the terrorist NPC can be in.
/// </summary>
enum TerroristNPCState
{
    Idle,
    Running,
    Caught
}

/// <summary>
/// Controls the behaviour of the terrorist NPC, including patrolling,
/// fleeing from the player, dropping an object, and handling the mission timer.
/// </summary>
public class TerroristNPC : MonoBehaviour
{
    /// <summary>
    /// Stores the current state of the terrorist NPC.
    /// </summary>
    [SerializeField] TerroristNPCState npcState = TerroristNPCState.Idle;

    /// <summary>
    /// Reference to the player that the NPC detects and flees from.
    /// </summary>
    public Transform player;

    /// <summary>
    /// Distance at which the NPC detects the player.
    /// </summary>
    public float detectionRange = 5.0f;

    /// <summary>
    /// Distance the NPC attempts to flee from the player.
    /// </summary>
    public float fleeDistance = 5.0f;

    /// <summary>
    /// Movement speed used when the NPC is fleeing.
    /// </summary>
    public float runSpeed = 4.0f;

    /// <summary>
    /// Movement speed used when the NPC is patrolling.
    /// </summary>
    public float walkSpeed = 2f;

    /// <summary>
    /// Distance used to determine when the NPC is caught.
    /// </summary>
    public float catchDistance = 1.0f;

    /// <summary>
    /// Stores the GameObject associated with the current score.
    /// </summary>
    private GameObject currentscore;

    /// <summary>
    /// Point on the NavMesh used as the NPC's left patrol destination.
    /// </summary>
    public Transform leftPoint;

    /// <summary>
    /// Point on the NavMesh used as the NPC's right patrol destination.
    /// </summary>
    public Transform rightPoint;

    /// <summary>
    /// Stores the score awarded when the NPC is caught.
    /// </summary>
    public int score = 0;

    /// <summary>
    /// Stores the name of the NPC.
    /// </summary>
    public string NPCName = "Terrorist";

    /// <summary>
    /// Audio source used when the NPC drops the object.
    /// </summary>
    public AudioSource DropBagAudio;

    /// <summary>
    /// NavMeshAgent used to control the NPC's movement.
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// Stores the current patrol destination of the NPC.
    /// </summary>
    private Transform currentPatrolTarget;

    /// <summary>
    /// Maximum amount of time allowed to catch the NPC.
    /// </summary>
    [SerializeField] private float timeLimit = 10f;      // seconds allowed to catch the NPC

    /// <summary>
    /// Visual effect object used when the timer expires.
    /// </summary>
    [SerializeField] private GameObject Explosionvfx;    // assign your VFX object in the Inspector

    /// <summary>
    /// Dust visual effect object used when the timer expires.
    /// </summary>
    [SerializeField] private GameObject Dustvfx;    // assign your VFX object in the Inspector

    /// <summary>
    /// Flash visual effect object used when the timer expires.
    /// </summary>
    [SerializeField] private GameObject Flashvfx;    // assign your VFX object in the Inspector

    /// <summary>
    /// Sparks visual effect object used when the timer expires.
    /// </summary>
    [SerializeField] private GameObject Sparksvfx;    // assign your VFX object in the Inspector

    /// <summary>
    /// Stores the remaining time available to catch the NPC.
    /// </summary>
    private float timer;

    /// <summary>
    /// Keeps track of whether the NPC has already been caught.
    /// </summary>
    private bool isCaught = false;

    /// <summary>
    /// Keeps track of whether the mission timer is currently running.
    /// </summary>
    private bool timerRunning = false;

    /// <summary>
    /// Object that the NPC drops when the player gets close enough.
    /// </summary>
    [SerializeField] private GameObject objectToDrop;

    /// <summary>
    /// Distance between the player and NPC required for the NPC to drop the object.
    /// </summary>
    [SerializeField] private float dropDistance = 2f;

    /// <summary>
    /// Audio source used when the explosion occurs after the timer expires.
    /// </summary>
    public AudioSource ExplosionAudio;

    /// <summary>
    /// Audio source used to play the police warning sound.
    /// </summary>
    public AudioSource PoliceWarningAudio;

    /// <summary>
    /// Keeps track of whether the NPC has already dropped its object.
    /// </summary>
    private bool hasDropped = false;

    /// <summary>
    /// Gets the NavMeshAgent, sets the initial patrol target and walking speed,
    /// and starts the mission timer.
    /// </summary>
    void Start()
    {
        // Get the NavMeshAgent component attached to the NPC.
        agent = GetComponent<NavMeshAgent>();

        // Start the NPC by moving towards the left patrol point.
        currentPatrolTarget = leftPoint;

        // Set the NPC's initial movement speed.
        agent.speed = walkSpeed;

        // Start the mission countdown timer.
        StartTimer();
    }

    /// <summary>
    /// Starts or resets the mission timer and disables the failure visual effects.
    /// </summary>
    public void StartTimer()
    {
        // Reset the timer to the configured time limit.
        timer = timeLimit;

        // Reset the caught state.
        isCaught = false;

        // Start the timer.
        timerRunning = true;

        // Disable all failure visual effects when the timer starts.
        if (Explosionvfx != null && Dustvfx!=null && Flashvfx!=null && Sparksvfx!=null)
        {
            Explosionvfx.SetActive(false);
            Dustvfx.SetActive(false);
            Flashvfx.SetActive(false);
            Sparksvfx.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the NPC's state, checks the player's distance for dropping the object,
    /// and manages the mission countdown timer.
    /// </summary>
    void Update()
    {
        // Update the NPC's current behaviour.
        StateMachine();

        // Check whether the NPC should drop its object when the player gets close.
        if (!hasDropped && player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= dropDistance)
            {
                DropObject();
            }
        }

        // Stop updating the timer if it is not running or the NPC has already been caught.
        if (!timerRunning || isCaught) return;

        // Decrease the timer based on the time passed since the previous frame.
        timer -= Time.deltaTime;

        // Check if the timer has reached zero.
        if (timer <= 0f)
        {
            timerRunning = false;
            OnTimeExpired();
        }
    }

    /// <summary>
    /// Handles what happens when the mission timer reaches zero.
    /// Activates the failure visual effects and opens the lost panel.
    /// </summary>
    private void OnTimeExpired()
    {
        // Activate all failure visual effects and play the explosion audio.
        if (Explosionvfx != null && Dustvfx!=null && Flashvfx!=null && Sparksvfx!=null)
        {
            Explosionvfx.SetActive(true);
            Dustvfx.SetActive(true);
            Flashvfx.SetActive(true);
            Sparksvfx.SetActive(true);

            // Play the explosion audio if an AudioSource has been assigned.
            if (ExplosionAudio != null)
            {
                ExplosionAudio.Play();
            }

            // Delay opening the lost panel.
            StartCoroutine(OpenLostPanelAfterDelay(1f));
        }

        // optional: any other "failed" logic here, e.g. mission fail state
    }

    /// <summary>
    /// Makes the NPC move away from the player when fleeing.
    /// </summary>
    void FleeFromPlayer()
    {
        // Increase the NPC's movement speed while fleeing.
        agent.speed = runSpeed;

        // Calculate the direction away from the player.
        Vector3 directionAwayFromPlayer = transform.position - player.position;
        directionAwayFromPlayer.y = 0;
        directionAwayFromPlayer.Normalize();

        // Calculate a position in the direction away from the player.
        Vector3 fleeTargetPosition = transform.position + (directionAwayFromPlayer * fleeDistance);

        NavMeshHit hit;

        // Find a valid position on the NavMesh for the NPC to flee towards.
        if (NavMesh.SamplePosition(fleeTargetPosition, out hit, fleeDistance, NavMesh.AllAreas))
        {
            // Set the NPC's destination if it is currently on the NavMesh.
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    /// <summary>
    /// Makes the NPC patrol back and forth between the left and right patrol points.
    /// </summary>
    void PatrolBetweenPoints()
    {
        // Set the NPC to its normal walking speed.
        agent.speed = walkSpeed;

        // Check whether the NPC has reached its current patrol point.
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            // Switch from the left patrol point to the right patrol point.
            if (currentPatrolTarget == leftPoint)
                currentPatrolTarget = rightPoint;
            else
                currentPatrolTarget = leftPoint;
        }

        // Set the NPC's destination if a patrol target exists.
        if (currentPatrolTarget != null)
        {
            agent.SetDestination(currentPatrolTarget.position);
        }
    }

    /// <summary>
    /// Changes the NPC into its caught state, stops its movement,
    /// disables the timer and failure effects, and updates the score and UI.
    /// </summary>
    void TriggerCaughtState()
    {
        // Only trigger the caught behaviour once.
        if (isCaught != true)
        {
            isCaught = true;

            // Stop and disable the NPC's NavMeshAgent.
            agent.isStopped = true;
            agent.enabled = false;

            // Stop the mission timer.
            timerRunning = false;

            // Disable the failure visual effects.
            if (Explosionvfx != null && Dustvfx!=null && Flashvfx!=null && Sparksvfx!=null)
            {
                Explosionvfx.SetActive(false);
                Dustvfx.SetActive(false);
                Flashvfx.SetActive(false);
                Sparksvfx.SetActive(false);
            }

            print(agent.isStopped);

            // Get the NPC's Rigidbody component.
            Rigidbody rb = GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Stop the NPC's current movement and make the Rigidbody kinematic.
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            Debug.Log("NPC has been Caught!");

            // Add the score awarded for catching the NPC.
            score += 5000;

            // Increase the player's overall game score.
            GameManager.instance.IncreaseScore(score);

            // Update the caught NPC information on the UI.
            GameManager.instance.uiManager.UpdateCaughtPanel(NPCName, score);

            // Delay opening the caught panel.
            StartCoroutine(OpenCaughtPanelAfterDelay(1f));
        }
        else
        {
            return;
        }

        // TODO: Trigger caught animation or UI screen here
    }

    /// <summary>
    /// Waits for the specified delay before opening the caught panel.
    /// </summary>
    /// <param name="delay">The amount of time to wait before opening the caught panel.</param>
    private IEnumerator OpenCaughtPanelAfterDelay(float delay)
    {
        // Wait for the specified amount of time.
        yield return new WaitForSeconds(delay);

        // Open the caught panel through the UI manager.
        GameManager.instance.uiManager.OpenCaughtPanel();
    }

    /// <summary>
    /// Waits for the specified delay before opening the lost panel.
    /// </summary>
    /// <param name="delay">The amount of time to wait before opening the lost panel.</param>
    private IEnumerator OpenLostPanelAfterDelay(float delay)
    {
        // Wait for the specified amount of time.
        yield return new WaitForSeconds(delay);

        // Open the lost panel through the UI manager.
        GameManager.instance.uiManager.OpenLostPanel();
    }

    /// <summary>
    /// Checks whether the player collides with the NPC and triggers the caught state.
    /// </summary>
    /// <param name="collision">The collision information from the collision event.</param>
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object that collided with the NPC is the player.
        if (collision.transform == player && !isCaught)
        {
            TriggerCaughtState();
        }
    }

    /// <summary>
    /// Determines the NPC's current state based on its distance from the player
    /// and whether the player has been caught.
    /// </summary>
    void StateMachine()
    {
        // Calculate the distance between the NPC and the player.
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check whether the player has been caught.
        if (PlayerScript.IsCaught())
        {
            npcState = TerroristNPCState.Caught;
            npcState = TerroristNPCState.Idle;
        }

        // Make the NPC run away when the player is within detection range.
        else if (distanceToPlayer <= detectionRange)
        {
            npcState = TerroristNPCState.Running;

            // Play the police warning sound when the NPC detects the player.
            if (PoliceWarningAudio != null)
            {
                PoliceWarningAudio.Play();
            }
        }

        // Return the NPC to its idle patrol state when the player is outside detection range.
        else
        {
            npcState = TerroristNPCState.Idle;
        }

        // Perform the behaviour associated with the current NPC state.
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

        // NPC caught 
        if (isCaught) 
        {
            return;
        }
    }

    /// <summary>
    /// Removes the object from the NPC and enables physics on it when the player
    /// gets within the configured drop distance.
    /// </summary>
    private void DropObject()
    {
        // Mark the object as dropped so it cannot be dropped again.
        hasDropped = true;

        // Stop if there is no object assigned to drop.
        if (objectToDrop == null)
            return;

        // Remove it from the NPC.
        objectToDrop.transform.SetParent(null);

        // Get the Rigidbody attached to the object.
        Rigidbody rb = objectToDrop.GetComponent<Rigidbody>();

        // Make the dropped object affected by physics.
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // Play the DropBag sound.
        if (DropBagAudio != null)
        {
            DropBagAudio.Play();
        }

        Debug.Log("NPC dropped the object!");
    }
}