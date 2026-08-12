using UnityEngine;
using UnityEngine.AI;
 
public class TerroristTasksNPC : MonoBehaviour
{
    public GameObject player;
    public GameObject MyTerroristNPC;
 
    private NavMeshAgent agent;
    public float stoppingDistance = 4f;

    Vector3 previousPosition;
 
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;

        previousPosition = transform.position; // Store the initial position of the NPC

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p;
            Debug.Log("Player found: " + player.name);
        }
    }
 
    void FixedUpdate()
    {
        moveToPlayer();
    }

    void moveToPlayer()
    {
        if (player == null) return;

        if (Vector3.Distance(previousPosition, player.transform.position) < stoppingDistance)
        {
            return; // If the previous position is the same as the player's position, do nothing
        }

        previousPosition = player.transform.position; // Update the previous position to the player's current position

        agent.SetDestination(player.transform.position);

    }
}
