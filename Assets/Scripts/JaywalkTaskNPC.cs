using UnityEngine;
using UnityEngine.AI;
 
public class JaywalkTasksNPC : MonoBehaviour
{
    public Transform player;
    public GameObject MyJaywalkNPC;
 
    public float stoppingDistance = 4f;
 
    private NavMeshAgent agent;
 
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
 
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }
 
    void Update()
    {
        if (player == null) return;

        agent.SetDestination(player.position);
        
    }
}