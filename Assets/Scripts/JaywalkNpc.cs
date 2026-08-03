using UnityEngine;
using UnityEngine.AI;

public class JaywalkNPC : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;
    
    private NavMeshAgent agent;
    private Transform currentTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Start by walking to the left point
        currentTarget = leftPoint;
        agent.SetDestination(currentTarget.position);
    }

    void Update()
    {
        // Check if the agent has reached its destination
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Switch targets
            if (currentTarget == leftPoint)
            {
                currentTarget = rightPoint;
            }
            else
            {
                currentTarget = leftPoint;
            }

            // Set the new destination
            agent.SetDestination(currentTarget.position);
        }
    }
}
