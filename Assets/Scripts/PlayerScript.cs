using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

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
    private GameObject currentdoor;
    private GameObject currentTaskNPC;
    private GameObject currentTerroristTaskNPC;
    public UIManager MyUIManager; // Reference to the UIManager script

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
            else if (hit.collider.CompareTag("Door"))
            {
                print($"Looking at {hit.collider.gameObject.name}");
                currentdoor=hit.collider.gameObject;
            }
            else if (hit.collider.CompareTag("TaskNPC"))
            {
                print($"Looking at {hit.collider.gameObject.name}");
                currentTaskNPC=hit.collider.gameObject;
            }
            else if (hit.collider.CompareTag("TerroristTaskNPC"))
            {
                print($"Looking at {hit.collider.gameObject.name}");
                currentTerroristTaskNPC=hit.collider.gameObject;
            }
            else
            {
                    currentTerroristTaskNPC=null;
            }
        }
        if (currentTaskNPC != null)
        {
            JaywalkTasksNPC taskNPC = currentTaskNPC.GetComponent<JaywalkTasksNPC>();
            print("Interacting with TaskNPC");
            MyUIManager.OpenJaywalkMissionPanel();
        }
        else
        {
            currentTaskNPC = null;
        }
        if (currentTerroristTaskNPC !=null)
        {
            TerroristTasksNPC terroristTaskNPC = currentTerroristTaskNPC.GetComponent<TerroristTasksNPC>();
            print("Interacting with TerroristTaskNPC");
            MyUIManager.OpenTerroristMissionPanel();
        }
        else
        {
            currentTerroristTaskNPC = null;
        }
    }

    public static bool IsCaught()
    {
        return caughtNPC;
    }
    void OnMenu(InputValue value)
    {
        MyUIManager.ToggleMenuPanel();
    }
}

