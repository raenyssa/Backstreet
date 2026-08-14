using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls the movement of the terrorist task NPC towards the player.
/// </summary>
public class TerroristTasksNPC : MonoBehaviour
{
    /// <summary>
    /// Reference to the player that the NPC will move towards.
    /// </summary>
    public GameObject player;

    /// <summary>
    /// Reference to the terrorist NPC associated with this task NPC.
    /// </summary>
    public GameObject MyTerroristNPC;

    /// <summary>
    /// NavMeshAgent used to control the NPC's movement.
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// Distance the NPC should stop from the player.
    /// </summary>
    public float stoppingDistance = 4f;   

    /// <summary>
    /// Stores the previous position used to determine whether the player has moved.
    /// </summary>
    Vector3 previousPosition;

    /// <summary>
    /// Gets the NavMeshAgent, sets its stopping distance, stores the initial position,
    /// and finds the player if a player reference has not been assigned.
    /// </summary>
    void Start()
    {
        // Get the NavMeshAgent component attached to the NPC.
        agent = GetComponent<NavMeshAgent>();

        // Set the distance at which the NPC stops from the player.
        agent.stoppingDistance = stoppingDistance;

        // Store the initial position of the NPC.
        previousPosition = transform.position; // Store the initial position of the NPC

        // Find the player automatically if no player reference was assigned.
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p;
            Debug.Log("Player found: " + player.name);
        }
    }

    /// <summary>
    /// Updates the NPC's movement towards the player at fixed time intervals.
    /// </summary>
    void FixedUpdate()
    {
        // Move the NPC towards the player.
        moveToPlayer();
    }

    /// <summary>
    /// Moves the NPC towards the player's current position when the player
    /// has moved far enough from the previously stored position.
    /// </summary>
    void moveToPlayer()
    {
        // Stop if no player reference is available.
        if (player == null) return;

        // Check whether the player's current position is within the stopping distance
        // of the previously stored position.
        if (Vector3.Distance(previousPosition, player.transform.position) < stoppingDistance)
        {
            return; // If the previous position is the same as the player's position, do nothing
        }

        // Update the previous position to the player's current position.
        previousPosition = player.transform.position; // Update the previous position to the player's current position

        // Set the player's current position as the NPC's destination.
        agent.SetDestination(player.transform.position);
    }
}