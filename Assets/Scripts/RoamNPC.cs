/*
 * Author: MarilynTan
 * Date: 12th August 2026
 * File: RoamNPC
 * Description: Controls random roaming behaviour for an NPC using a NavMeshAgent.
 
 */
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls random roaming behaviour for an NPC using a NavMeshAgent.
/// </summary>
public class NPCRoaming : MonoBehaviour
{
    /// <summary>
    /// The maximum distance from the NPC's current position that it can roam.
    /// </summary>
    public float roamRadius = 10f;

    /// <summary>
    /// The amount of time the NPC waits before selecting a new destination.
    /// </summary>
    public float waitTime = 2f;

    /// <summary>
    /// NavMeshAgent used to move the NPC around the environment.
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// Tracks the amount of time that has passed since the last destination was selected.
    /// </summary>
    private float timer;

    /// <summary>
    /// Gets the NavMeshAgent component and initialises the roaming timer.
    /// </summary>
    void Start()
    {
        // Get the NavMeshAgent component attached to the NPC.
        agent = GetComponent<NavMeshAgent>();

        // Start the timer using the configured wait time.
        timer = waitTime;
    }

    /// <summary>
    /// Updates the roaming timer and selects a new random NavMesh destination
    /// when the NPC has reached its current destination or the wait time has passed.
    /// </summary>
    void Update()
    {
        // Increase the timer based on the time passed since the previous frame.
        timer += Time.deltaTime;

        // Check if the NPC has reached its destination or if the wait timer is up.
        if (timer >= waitTime && (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance))
        {
            // Find a random valid position on the NavMesh within the roaming radius.
            Vector3 newPos = GetRandomNavMeshPoint(transform.position, roamRadius);

            // Set the random position as the NPC's new destination.
            agent.SetDestination(newPos);

            // Reset the timer after selecting a new destination.
            timer = 0f; // Reset timer
        }
    }

    /// <summary>
    /// Finds a random valid position on the NavMesh within a specified radius.
    /// </summary>
    /// <param name="center">The centre position from which the random point is generated.</param>
    /// <param name="radius">The maximum distance from the centre position.</param>
    /// <returns>A valid NavMesh position, or the centre position if no valid position is found.</returns>
    public static Vector3 GetRandomNavMeshPoint(Vector3 center, float radius)
    {
        // 1. Generate a random point inside a sphere.
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center;
        
        NavMeshHit hit;

        // 2. Project that point onto the NavMesh.
        // Use NavMesh.AllAreas (or an integer bitmask) to sample the area.
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        
        // Return the current position if no valid NavMesh point is found.
        return center; // Fallback to current position if no point found
    }
}