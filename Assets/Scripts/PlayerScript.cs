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
            else
            {
                currentNPC = null;
            }
            if (hit.collider.CompareTag("Door"))
            {
                print($"Looking at {hit.collider.gameObject.name}");
                currentdoor=hit.collider.gameObject;
            }
            else
            {
                    currentdoor=null;
            }
            if (hit.collider.CompareTag("TaskNPC"))
            {
                print($"Looking at {hit.collider.gameObject.name}");
                currentTaskNPC=hit.collider.gameObject;
            }
            else
            {
                    currentTaskNPC=null;
            }
        }
        if (currentTaskNPC != null)
        {
            TasksNPC taskNPC = currentTaskNPC.GetComponent<TasksNPC>();
            print("Interacting with TaskNPC");
            MyUIManager.OpenMissionPanel();
        }
        else
        {
            return;
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