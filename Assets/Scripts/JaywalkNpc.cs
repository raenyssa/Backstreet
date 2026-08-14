/*
 * Author: Gladis Koh
 * Date: 11th August 2026
 * File: Jaywalk
 * Description: Represents the different states that the jaywalking NPC can be in.
 
 */
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// Represents the different states that the jaywalking NPC can be in.
/// </summary>
enum NPCState
{
    Idle,
    Running,
    Caught
}

/// <summary>
/// Controls the behaviour of a jaywalking NPC, including patrolling,
/// fleeing from the player, and being caught.
/// </summary>
public class JayWalkNPC : MonoBehaviour
{
    /// <summary>
    /// Stores the current state of the NPC.
    /// </summary>
    [SerializeField] NPCState npcState = NPCState.Idle;

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
    /// Audio source used to play the police warning sound.
    /// </summary>
    public AudioSource PoliceWarningAudio;

    /// <summary>
    /// Stores the name of the NPC.
    /// </summary>
    public string NPCName = "Jaywalker";

    /// <summary>
    /// NavMeshAgent used to control the NPC's movement.
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// Stores the current patrol destination of the NPC.
    /// </summary>
    private Transform currentPatrolTarget;

    /// <summary>
    /// Keeps track of whether the NPC has already been caught.
    /// </summary>
    private bool isCaught = false;

    /// <summary>
    /// Gets the NavMeshAgent and sets the NPC's initial patrol target and walking speed.
    /// </summary>
    void Start()
    {
        // Get the NavMeshAgent component attached to the NPC.
        agent = GetComponent<NavMeshAgent>();

        // Start the NPC by moving towards the left patrol point.
        currentPatrolTarget = leftPoint;

        // Set the NPC's initial movement speed.
        agent.speed = walkSpeed;
    }

    /// <summary>
    /// Updates the NPC's state every frame.
    /// </summary>
    void Update()
    {
        // Run the NPC state machine to determine its current behaviour.
        StateMachine();
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
    /// Changes the NPC into its caught state and updates the game score and UI.
    /// </summary>
    void TriggerCaughtState()
    {
        // Only trigger the caught behaviour once.
        if (!isCaught)
        {
            isCaught = true;

            // Stop and disable the NPC's NavMeshAgent.
            agent.isStopped = true;
            agent.enabled = false;

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

            // Add points to the player's score when the NPC is caught.
            GameManager.instance.IncreaseScore(1000);

            // Update the caught NPC information on the UI.
            GameManager.instance.UpdateCaught(NPCName);

            // Delay opening the caught panel.
            StartCoroutine(OpenCaughtPanelAfterDelay(1f));
        }

        // TODO: Trigger caught animation or UI screen here
    }

    /// <summary>
    /// Waits for the specified delay before opening the caught panel.
    /// </summary>
    /// <param name="delay">The amount of time to wait before opening the panel.</param>
    private IEnumerator OpenCaughtPanelAfterDelay(float delay)
    {
        // Wait for the specified amount of time.
        yield return new WaitForSeconds(delay);

        // Open the caught NPC panel through the UI manager.
        GameManager.instance.uiManager.OpenCaughtPanel();
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
            npcState = NPCState.Caught;
            npcState = NPCState.Idle;
        }

        // Make the NPC run away when the player is within detection range.
        else if (distanceToPlayer <= detectionRange)
        {
            npcState = NPCState.Running;

            // Play the police warning sound when the NPC detects the player.
            if (PoliceWarningAudio != null)
            {
                PoliceWarningAudio.Play();
            }
        }

        // Return the NPC to its idle patrol state when the player is outside detection range.
        else
        {
            npcState = NPCState.Idle;
        }

        // Perform the behaviour associated with the current NPC state.
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

        // Stop further processing once the NPC has been caught.
        if (isCaught)
        {
            return;
        }
    }
}