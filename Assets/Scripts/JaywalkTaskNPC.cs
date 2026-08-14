using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls the movement of the jaywalking task NPC towards the player.
/// </summary>
public class JaywalkTasksNPC : MonoBehaviour
{
    /// <summary>
    /// Reference to the player that the NPC will move towards.
    /// </summary>
    public Transform player;

    /// <summary>
    /// Reference to the main jaywalking NPC GameObject.
    /// </summary>
    public GameObject MyJaywalkNPC;

    /// <summary>
    /// Distance the NPC should stop from the player.
    /// </summary>
    public float stoppingDistance = 4f;

    /// <summary>
    /// NavMeshAgent used to control the NPC's movement.
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// Gets the NavMeshAgent component, sets its stopping distance,
    /// and finds the player if a player reference has not been assigned.
    /// </summary>
    void Awake()
    {
        // Get the NavMeshAgent component attached to the NPC.
        agent = GetComponent<NavMeshAgent>();

        // Set the distance at which the NPC stops from the player.
        agent.stoppingDistance = stoppingDistance;

        // Find the player automatically if no player reference was assigned.
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    /// <summary>
    /// Continuously updates the NPC's destination to follow the player.
    /// </summary>
    void Update()
    {
        // Stop if no player reference is available.
        if (player == null) return;

        // Set the player's current position as the NPC's destination.
        agent.SetDestination(player.position);
        
    }
}