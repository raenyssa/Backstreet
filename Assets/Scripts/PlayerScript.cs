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
    private GameObject currentShopliftTaskNPC;
    private GameObject currentVandaliseTaskNPC;
    public AudioSource CollectibleAudio;
    [SerializeField] private GameObject cam;


    public static bool caughtNPC = false;

    

    void OnInteract()
    {
        print("Interacting");

        Debug.DrawRay(playerMesh.transform.position + new Vector3(0, 1f, 0),    
            playerMesh.transform.forward * 100, Color.red, 5f);

        if (Physics.Raycast(cam.transform.position,    
            cam.transform.forward, out RaycastHit hit, interactDistance, layerMask))
        {
            
            print(hit.collider.tag);
            if (hit.collider.CompareTag("NPC"))
            {   
                Debug.Log ("Interacting with NPC");
                print($"Looking at {hit.collider.gameObject.name}");
                currentNPC = hit.collider.gameObject;
                caughtNPC = true;
                return;
            }
            if (hit.collider.CompareTag("Door"))
            {
                print($"Looking at {hit.collider.gameObject.name}");
                currentdoor=hit.collider.gameObject;
            }
            if (hit.collider.CompareTag("TaskNPC"))
            {
                print($"Looking at {hit.collider.gameObject.name}");
                currentTaskNPC = hit.collider.gameObject;
                return;
            }
            if (hit.collider.CompareTag("ShopliftTaskNPC"))
            {
                print($"Looking at {hit.collider.gameObject.name}");
                currentShopliftTaskNPC=hit.collider.gameObject;
                return;                
            }
            if (hit.collider.CompareTag("VandaliseTaskNPC"))
            {
                print($"Looking at {hit.collider.gameObject.name}");
                currentVandaliseTaskNPC=hit.collider.gameObject;
                return;
            }
            if (hit.collider.CompareTag("TerroristTaskNPC"))
            {
                print($"Looking at {hit.collider.gameObject.name}");
                currentTerroristTaskNPC=hit.collider.gameObject;
                return;
            }

            currentTerroristTaskNPC=null;
        }
        if (currentdoor!=null)
        {
            InteractableDoorScript door = currentdoor.GetComponentInParent<InteractableDoorScript>();
            print(door);
            print($"Interacting with {currentdoor.name}");
            door.open();
                
            }
        if (currentTaskNPC != null)
        {
            JaywalkTasksNPC taskNPC = currentTaskNPC.GetComponent<JaywalkTasksNPC>();
            print("Interacting with TaskNPC");
            GameManager.instance.uiManager.OpenJaywalkMissionPanel();
            currentTaskNPC = null;
        }
        if (currentTerroristTaskNPC !=null)
        {
            TerroristTasksNPC terroristTaskNPC = currentTerroristTaskNPC.GetComponent<TerroristTasksNPC>();
            print("Interacting with TerroristTaskNPC");
            GameManager.instance.uiManager.OpenTerroristMissionPanel();
        }
        if (currentShopliftTaskNPC!=null)
        {
            ShopliftTaskNPC taskNPC = currentShopliftTaskNPC.GetComponent<ShopliftTaskNPC>();
            print("Interacting with ShopliftTaskNPC");
            GameManager.instance.uiManager.OpenShopliftMissionPanel();
            currentShopliftTaskNPC = null;          
        }
        if (currentVandaliseTaskNPC!=null)
        {
            VandaliseTaskNPC taskNPC = currentVandaliseTaskNPC.GetComponent<VandaliseTaskNPC>();
            print("Interacting with VandaliseTaskNPC");
            GameManager.instance.uiManager.OpenVandaliseMissionPanel();
            currentVandaliseTaskNPC = null;
        }          
        
        else
        {
            currentShopliftTaskNPC = null;
        }

        
    }

    public static bool IsCaught()
    {
        return caughtNPC;
    }
    void OnMenu(InputValue value)
    {
        GameManager.instance.uiManager.ToggleMenuPanel();
    }
        void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Collectible"))
        {
            var SusItem = other.gameObject.GetComponent<Collectible>();
            SusItem.Collect();
                if (CollectibleAudio != null)
                {
                    CollectibleAudio.Play();
                }

        }
    }
}

