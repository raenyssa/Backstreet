using UnityEngine;
using UnityEngine.AI;
 
public class TerroristTasksNPC : MonoBehaviour
{
    public Transform player;
    public GameObject MyTerroristNPC;
 
    private NavMeshAgent agent;
    public float stoppingDistance = 4f;
 
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
