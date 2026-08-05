using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    int score = 0;

    [SerializeField]
    private GameObject playerMesh;

    [SerializeField]
    private float interactDistance = 6f;

    [SerializeField]
    private LayerMask layerMask;
    private GameObject currentNPC;
    private GameObject currentscore;

    public static bool caughtNPC = false;

    [SerializeField]
    GameObject currentCollider;

    void OnInteract()
    {
        print("Interacting");

        if (Physics.Raycast(playerMesh.transform.position + new Vector3(0, 0.5f, 0),    
            playerMesh.transform.forward, out RaycastHit hit, interactDistance, layerMask))
        {
            
            Debug.Log("hit");
            if (hit.collider.CompareTag("NPC"))
            {   

                print($"Looking at {hit.collider.gameObject.name}");
                currentNPC = hit.collider.gameObject;
                caughtNPC = true;
            }
            else
            {
                currentNPC = null;
            }
        }
    }

    public static bool IsCaught()
    {
        return caughtNPC;
    }
}