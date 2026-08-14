/*
 * Author: Gladis Koh
 * Date: 13th August 2026
 * File: VandaliseTaskNPC
 * Description: Represents the different states that the jaywalking NPC can be in.
 
 */
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls the NPC responsible for the vandalism task.
/// The NPC follows the player using a NavMeshAgent.
/// </summary>
public class VandaliseTaskNPC : MonoBehaviour
{
    /// <summary>
    /// Reference to the player's Transform.
    /// </summary>
    public Transform player;

    /// <summary>
    /// The minimum distance the NPC will maintain from the player.
    /// </summary>
    public float stoppingDistance = 4f;

    /// <summary>
    /// The NavMeshAgent component used to move the NPC.
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// Initializes the NPC's NavMeshAgent and finds the player if no player has been assigned.
    /// </summary>
    void Awake()
    {
        // Get the NavMeshAgent component attached to this NPC.
        agent = GetComponent<NavMeshAgent>();

        // Set the distance at which the NPC stops from the player.
        agent.stoppingDistance = stoppingDistance;

        // If no player has been assigned in the Inspector, find the Player using its tag.
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    /// <summary>
    /// Updates the NPC's destination to the player's current position.
    /// </summary>
    void Update()
    {
        // Stop updating the destination if the player cannot be found.
        if (player == null) return;

        // Make the NPC move towards the player's current position.
        agent.SetDestination(player.position);
    }
}