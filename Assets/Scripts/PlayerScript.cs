using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

/// <summary>
/// Handles player interactions with NPCs, doors, mission NPCs, collectibles,
/// and the game menu.
/// </summary>
public class PlayerScript : MonoBehaviour
{
    /// <summary>
    /// Stores the player's score.
    /// </summary>
    int score = 0;

    /// <summary>
    /// Reference to the player's mesh used for raycasting.
    /// </summary>
    [SerializeField]
    private GameObject playerMesh;

    /// <summary>
    /// Maximum distance at which the player can interact with objects.
    /// </summary>
    [SerializeField]
    private float interactDistance = 6f;

    /// <summary>
    /// Determines which layers can be detected by the interaction raycast.
    /// </summary>
    [SerializeField]
    private LayerMask layerMask;

    /// <summary>
    /// Stores the NPC currently being interacted with.
    /// </summary>
    private GameObject currentNPC;

    /// <summary>
    /// Stores the current score object.
    /// </summary>
    private GameObject currentscore;

    /// <summary>
    /// Stores the door currently being interacted with.
    /// </summary>
    private GameObject currentdoor;

    /// <summary>
    /// Stores the current jaywalking task NPC being interacted with.
    /// </summary>
    private GameObject currentTaskNPC;

    /// <summary>
    /// Stores the current terrorist task NPC being interacted with.
    /// </summary>
    private GameObject currentTerroristTaskNPC;

    /// <summary>
    /// Stores the current shoplifting task NPC being interacted with.
    /// </summary>
    private GameObject currentShopliftTaskNPC;

    /// <summary>
    /// Stores the current vandalism task NPC being interacted with.
    /// </summary>
    private GameObject currentVandaliseTaskNPC;

    /// <summary>
    /// Audio source used when collecting collectible objects.
    /// </summary>
    public AudioSource CollectibleAudio;

    /// <summary>
    /// Reference to the camera used to detect objects the player is looking at.
    /// </summary>
    [SerializeField] private GameObject cam;

    /// <summary>
    /// Keeps track of whether an NPC has been caught by the player.
    /// </summary>
    public static bool caughtNPC = false;

    /// <summary>
    /// Handles player interaction input and determines which interactable object
    /// the player is currently looking at.
    /// </summary>
    void OnInteract()
    {
        print("Interacting");

        // Draw a debug ray from the player mesh to visualise the interaction direction.
        Debug.DrawRay(playerMesh.transform.position + new Vector3(0, 1f, 0),    
            playerMesh.transform.forward * 100, Color.red, 5f);

        // Cast a ray from the camera to detect interactable objects within range.
        if (Physics.Raycast(cam.transform.position,    
            cam.transform.forward, out RaycastHit hit, interactDistance, layerMask))
        {
            
            print(hit.collider.tag);

            // Check if the player is interacting with an NPC.
            if (hit.collider.CompareTag("NPC"))
            {   
                Debug.Log ("Interacting with NPC");
                print($"Looking at {hit.collider.gameObject.name}");
                currentNPC = hit.collider.gameObject;
                caughtNPC = true;
                return;
            }

            // Check if the player is interacting with a door.
            if (hit.collider.CompareTag("Door"))
            {
                print($"Looking at {hit.collider.gameObject.name}");
                currentdoor=hit.collider.gameObject;
            }

            // Check if the player is interacting with a jaywalking task NPC.
            if (hit.collider.CompareTag("TaskNPC"))
            {
                print($"Looking at {hit.collider.gameObject.name}");
                currentTaskNPC = hit.collider.gameObject;
                
            }

            // Check if the player is interacting with a shoplifting task NPC.
            if (hit.collider.CompareTag("ShopliftTaskNPC"))
            {
                print($"Looking at {hit.collider.gameObject.name}");
                currentShopliftTaskNPC=hit.collider.gameObject;
                return;                
            }

            // Check if the player is interacting with a vandalism task NPC.
            if (hit.collider.CompareTag("VandaliseTaskNPC"))
            {
                print($"Looking at {hit.collider.gameObject.name}");
                currentVandaliseTaskNPC=hit.collider.gameObject;
                
            }

            // Check if the player is interacting with a terrorist task NPC.
            if (hit.collider.CompareTag("TerroristTaskNPC"))
            {
                print($"Looking at {hit.collider.gameObject.name}");
                currentTerroristTaskNPC=hit.collider.gameObject;
                return;
            }

            // Clear the terrorist task NPC reference when another object is detected.
            currentTerroristTaskNPC=null;
        }

        // Open or close the currently selected door.
        if (currentdoor!=null)
        {
            InteractableDoorScript door = currentdoor.GetComponentInParent<InteractableDoorScript>();
            print(door);
            print($"Interacting with {currentdoor.name}");
            door.open();
            
        }

        // Open the jaywalking mission panel when interacting with the task NPC.
        if (currentTaskNPC != null)
        {
            JaywalkTasksNPC taskNPC = currentTaskNPC.GetComponent<JaywalkTasksNPC>();
            print("Interacting with TaskNPC");
            GameManager.instance.uiManager.OpenJaywalkMissionPanel();
            currentTaskNPC = null;
        }

        // Open the terrorist mission panel when interacting with the terrorist task NPC.
        if (currentTerroristTaskNPC !=null)
        {
            TerroristTasksNPC terroristTaskNPC = currentTerroristTaskNPC.GetComponent<TerroristTasksNPC>();
            print("Interacting with TerroristTaskNPC");
            GameManager.instance.uiManager.OpenTerroristMissionPanel();
        }

        // Open the shoplifting mission panel when interacting with the shoplifting task NPC.
        if (currentShopliftTaskNPC!=null)
        {
            ShopliftTaskNPC taskNPC = currentShopliftTaskNPC.GetComponent<ShopliftTaskNPC>();
            print("Interacting with ShopliftTaskNPC");
            GameManager.instance.uiManager.OpenShopliftMissionPanel();
            currentShopliftTaskNPC = null;          
        }

        // Open the vandalism mission panel when interacting with the vandalism task NPC.
        if (currentVandaliseTaskNPC!=null)
        {
            VandaliseTaskNPC taskNPC = currentVandaliseTaskNPC.GetComponent<VandaliseTaskNPC>();
            print("Interacting with VandaliseTaskNPC");
            GameManager.instance.uiManager.OpenVandaliseMissionPanel();
            currentVandaliseTaskNPC = null;
        }          
            
        else
        {
            // Clear the shoplifting task NPC reference.
            currentShopliftTaskNPC = null;
        }

        
    }

    /// <summary>
    /// Returns whether an NPC has been caught by the player.
    /// </summary>
    /// <returns>True if an NPC has been caught; otherwise, false.</returns>
    public static bool IsCaught()
    {
        return caughtNPC;
    }

    /// <summary>
    /// Toggles the game's menu panel when the menu input is triggered.
    /// </summary>
    /// <param name="value">The input value associated with the menu action.</param>
    void OnMenu(InputValue value)
    {
        GameManager.instance.uiManager.ToggleMenuPanel();
    }

    /// <summary>
    /// Detects when the player enters a collectible's trigger area and collects it.
    /// </summary>
    /// <param name="other">The collider that entered the player's trigger.</param>
    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is a collectible.
        if(other.gameObject.CompareTag("Collectible"))
        {
            // Get the Collectible component from the collected object.
            var SusItem = other.gameObject.GetComponent<Collectible>();

            // Collect the item.
            SusItem.Collect();

            // Play the collection sound if an AudioSource has been assigned.
            if (CollectibleAudio != null)
            {
                CollectibleAudio.Play();
            }

        }
    }
}