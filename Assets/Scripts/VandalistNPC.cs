/*
 * Author: Raenyssa
 * Date: 13th August 2026
 * File: VandaliseNPC
 * Description: Represents the different states that the vandalist NPC can be in.
 
 */
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// Represents the different states that the vandalist NPC can be in.
/// </summary>
enum VandalistNPCState
{
    /// <summary>
    /// The NPC is patrolling between its assigned points.
    /// </summary>
    Idle,

    /// <summary>
    /// The NPC is running away from the player.
    /// </summary>
    Running,

    /// <summary>
    /// The NPC has been caught by the player.
    /// </summary>
    Caught
}

/// <summary>
/// Controls the behaviour of the vandalist NPC, including patrolling,
/// fleeing from the player, and handling the caught state.
/// </summary>
public class VandalistNPC : MonoBehaviour
{
    /// <summary>
    /// The current state of the vandalist NPC.
    /// </summary>
    [SerializeField] VandalistNPCState npcState = VandalistNPCState.Idle;

    /// <summary>
    /// Reference to the player's Transform.
    /// </summary>
    public Transform player;

    /// <summary>
    /// The distance at which the NPC detects the player.
    /// </summary>
    public float detectionRange = 5.0f;

    /// <summary>
    /// The distance the NPC attempts to move away from the player when fleeing.
    /// </summary>
    public float fleeDistance = 5.0f;

    /// <summary>
    /// The movement speed of the NPC when fleeing.
    /// </summary>
    public float runSpeed = 4.0f;

    /// <summary>
    /// The movement speed of the NPC while patrolling.
    /// </summary>
    public float walkSpeed = 2f;

    /// <summary>
    /// The distance at which the NPC can be caught by the player.
    /// </summary>
    public float catchDistance = 1.0f;

    /// <summary>
    /// Reference to the current score GameObject.
    /// </summary>
    private GameObject currentscore;

    /// <summary>
    /// The left patrol point that the NPC moves towards.
    /// </summary>
    public Transform leftPoint;

    /// <summary>
    /// The right patrol point that the NPC moves towards.
    /// </summary>
    public Transform rightPoint;

    /// <summary>
    /// Audio source used to play the police warning sound.
    /// </summary>
    public AudioSource PoliceWarningAudio;

    /// <summary>
    /// The name of the NPC used when updating the caught status.
    /// </summary>
    public string NPCName = "Vandalist";

    /// <summary>
    /// The NavMeshAgent component used to control the NPC's movement.
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// The current patrol point that the NPC is moving towards.
    /// </summary>
    private Transform currentPatrolTarget;

    /// <summary>
    /// Keeps track of whether the NPC has already been caught.
    /// </summary>
    private bool isCaught = false;

    /// <summary>
    /// Initializes the NPC's NavMeshAgent and sets its initial patrol point and walking speed.
    /// </summary>
    void Start()
    {
        // Get the NavMeshAgent component attached to the NPC.
        agent = GetComponent<NavMeshAgent>();

        // Start the NPC's patrol at the left patrol point.
        currentPatrolTarget = leftPoint;

        // Set the NPC's initial movement speed to its walking speed.
        agent.speed = walkSpeed;
    }

    /// <summary>
    /// Continuously updates the NPC's state and behaviour.
    /// </summary>
    void Update()
    {
        // Run the NPC state machine every frame.
        StateMachine();
    }

    /// <summary>
    /// Makes the NPC flee away from the player's current position.
    /// </summary>
    void FleeFromPlayer()
    {
        // Increase the NPC's movement speed while fleeing.
        agent.speed = runSpeed;

        // Calculate the direction from the player to the NPC.
        Vector3 directionAwayFromPlayer = transform.position - player.position;

        // Prevent the NPC from changing its vertical movement direction.
        directionAwayFromPlayer.y = 0;

        // Normalize the direction so it has a magnitude of one.
        directionAwayFromPlayer.Normalize();

        // Calculate a position in the direction away from the player.
        Vector3 fleeTargetPosition = transform.position + (directionAwayFromPlayer * fleeDistance);

        NavMeshHit hit;

        // Find a valid position on the NavMesh near the calculated flee position.
        if (NavMesh.SamplePosition(fleeTargetPosition, out hit, fleeDistance, NavMesh.AllAreas))
        {
            // Only set the destination if the agent is currently on a NavMesh.
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    /// <summary>
    /// Moves the NPC between its left and right patrol points.
    /// </summary>
    void PatrolBetweenPoints()
    {
        // Set the NPC's movement speed to its walking speed.
        agent.speed = walkSpeed;

        // Check whether the NPC has reached its current patrol point.
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            // Switch from the left patrol point to the right patrol point.
            if (currentPatrolTarget == leftPoint)
                currentPatrolTarget = rightPoint;
            else
                // Switch from the right patrol point back to the left patrol point.
                currentPatrolTarget = leftPoint;
        }

        // Make sure a patrol target has been assigned before setting the destination.
        if (currentPatrolTarget != null)
        {
            agent.SetDestination(currentPatrolTarget.position);
        }
    }

    /// <summary>
    /// Handles the NPC's behaviour when it has been caught.
    /// </summary>
    void TriggerCaughtState()
    {
        // Only trigger the caught behaviour once.
        if (!isCaught)
        {
            isCaught = true;

            // Stop and disable the NavMeshAgent so the NPC can no longer move.
            agent.isStopped = true;
            agent.enabled = false;

            print(agent.isStopped);

            // Get the NPC's Rigidbody component.
            Rigidbody rb = GetComponent<Rigidbody>();

            // Stop all Rigidbody movement and make it kinematic.
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            Debug.Log("NPC has been Caught!");

            // Increase the player's score for catching the vandalist.
            GameManager.instance.IncreaseScore(2000);

            // Update the caught NPC information in the GameManager.
            GameManager.instance.UpdateCaught(NPCName);

            // Wait before opening the caught panel.
            StartCoroutine(OpenCaughtPanelAfterDelay(1f));
        }

        // TODO: Trigger caught animation or UI screen here
    }

    /// <summary>
    /// Waits for a specified amount of time before opening the caught panel.
    /// </summary>
    /// <param name="delay">The amount of time to wait before opening the panel.</param>
    private IEnumerator OpenCaughtPanelAfterDelay(float delay)
    {
        // Wait for the specified delay.
        yield return new WaitForSeconds(delay);

        // Open the caught panel through the UI Manager.
        GameManager.instance.uiManager.OpenCaughtPanel();
    }

    /// <summary>
    /// Detects when the NPC collides with the player and triggers the caught state.
    /// </summary>
    /// <param name="collision">The collision information from the object that collided with the NPC.</param>
    private void OnCollisionEnter(Collision collision)
    {
        // Check whether the object that collided with the NPC is the player
        // and make sure the NPC has not already been caught.
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
            // Set the NPC's state to caught.
            npcState = VandalistNPCState.Caught;

            // Set the NPC's state back to idle.
            npcState = VandalistNPCState.Idle;
        }
        // Check whether the player is within the NPC's detection range.
        else if (distanceToPlayer <= detectionRange)
        {
            // Change the NPC's state to running.
            npcState = VandalistNPCState.Running;

            // Play the police warning audio if an audio source has been assigned.
            if (PoliceWarningAudio != null)
            {
                PoliceWarningAudio.Play();
            }
        }
        else
        {
            // Keep the NPC in the idle/patrol state when the player is outside its detection range.
            npcState = VandalistNPCState.Idle;
        }

        // Perform the behaviour associated with the current NPC state.
        switch (npcState)
        {
            case VandalistNPCState.Idle:
                // Patrol between the assigned patrol points.
                PatrolBetweenPoints();
                break;

            case VandalistNPCState.Running:
                // Flee away from the player.
                FleeFromPlayer();
                break;

            case VandalistNPCState.Caught:
                // Trigger the caught behaviour.
                TriggerCaughtState();
                break;
        }

        // NPC caught
        if (isCaught)
        {
            return;
        }
    }
}