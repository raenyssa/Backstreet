using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Drag your Player GameObject here in the Inspector
    public GameObject player; 
    
    // Drag an empty GameObject or Target Pad here to set the destination
    public Transform destination; 
    public GameObject JaywalkDestination; // Reference to the destination GameObject for the jaywalking mission
    public TMP_Text caughttext; // Reference to the TextMeshProUGUI component for displaying the caught message
    public TMP_Text scoreText; // Reference to the TextMeshProUGUI component for displaying the score
    public GameObject CaughtPanel; // Reference to the panel that shows when the player is caught
    public GameObject JaywalkMissionPanel; // Reference to the panel that shows mission-related information
    public GameObject MenuPanel; // Reference to the panel that shows the menu
    public GameObject ShopliftMissionPanel; // Reference to the panel that shows mission-related information
    public GameObject TerroristMissionPanel; // Reference to the panel that shows mission-related information
    public GameObject JaywalkTaskNPC; // Reference to the JaywalkTaskNPC GameObject
    public GameObject TerroristTaskNPC;
    public GameObject ShopliftTaskNPC;
    public static UIManager Instance { get; internal set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        JaywalkMissionPanel.SetActive(false);
        CaughtPanel.SetActive(false);
        MenuPanel.SetActive(false);
        ShopliftMissionPanel.SetActive(false);
        TerroristMissionPanel.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        // Update the score display in the UI
        scoreText.text = $"Score points: {score}";
    }

    public void UpdateCaughtPanel(string NPCName, int score)
    {
        caughttext.text = $" Congratulations!\nYou have caught the {NPCName} and earned {score} points!";
    }

    // Update is called once per frame
    public void OpenJaywalkMissionPanel()
    {
        JaywalkMissionPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseJaywalkMissionPanel()
    {
        JaywalkMissionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void OpenShopliftMissionPanel()
    {
        ShopliftMissionPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void CloseShopliftMissionPanel()
    {
        ShopliftMissionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void OpenTerroristMissionPanel()
    {
        TerroristMissionPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }
    public void CloseTerroristMissionPanel()
    {
        TerroristMissionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    public void OpenCaughtPanel()
    {
        CaughtPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseCaughtPanel()
    {
        CaughtPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    

    public void ToggleMenuPanel()
    {
        MenuPanel.SetActive(!MenuPanel.activeSelf);
        Cursor.visible = MenuPanel.activeSelf;
        Cursor.lockState = MenuPanel.activeSelf ? 
        CursorLockMode.None : 
        CursorLockMode.Locked;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CompleteMRT()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "Marilyn's scene")
        {
            return;
        }

        Debug.Log("MRT mission completed! Loading JaywalkScene...");
        CloseCaughtPanel();
        Debug.Log("Panel Closed");
        SceneManager.LoadScene("JaywalkScene");
    }

    public void CompleteSupermarket()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "Gladis")
        {
            return;
        }

        Debug.Log("Shoplift mission completed! Loading JaywalkScene...");
        CloseCaughtPanel();
        Debug.Log("Panel Closed");
        SceneManager.LoadScene("JaywalkScene");
    }


    public void TeleportPlayer()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "JaywalkScene")
        {
            return;
        }

        if (player == null || destination == null) return;

        PlayerScript.caughtNPC = false; // Reset the caught state before teleporting;

        Debug.Log("Teleporting player to the destination...");
        
        // Safely move using our helper method
        ExecuteTeleport(player, destination.position);

        Debug.Log("Player has been teleported to the destination!");
        Invoke("CloseCaughtPanel", 1f); // Close the caught panel after 1 second
    }

    public void AcceptJaywalkMission()
    {
        if (player == null || JaywalkDestination == null) return;

        // Logic to accept the jaywalking mission
        Debug.Log("Jaywalking mission accepted!");
        CloseJaywalkMissionPanel();
        
        // Safely move using our helper method
        ExecuteTeleport(player, JaywalkDestination.transform.position);

        Debug.Log("Destroying at" + JaywalkTaskNPC.transform.position);

        Destroy(JaywalkTaskNPC); // Destroy the JaywalkTaskNPC GameObject after teleporting

    }
    public void AcceptTerroristMission()
    {
        Debug.Log("Terrorist mission accepted!");
        CloseTerroristMissionPanel();
        SceneManager.LoadScene("Marilyn's scene");
        Destroy(TerroristTaskNPC);
    }

    public void AcceptShoplifterMission()
    {
        Debug.Log("ShopliftMissionPanel mission accept!");
        CloseShopliftMissionPanel();
        SceneManager.LoadScene("Gladis");
        Destroy(ShopliftTaskNPC);
        
    }


    /// <summary>
    /// Safely updates positions when components like CharacterController or Rigidbody are active.
    /// </summary>
    private void ExecuteTeleport(GameObject targetPlayer, Vector3 targetPosition)
    {
        CharacterController cc = targetPlayer.GetComponent<CharacterController>();
        Rigidbody rb = targetPlayer.GetComponent<Rigidbody>();

        // 1. Disable Character Controller to stop it fighting the transform update
        if (cc != null)
        {
            cc.enabled = false;
        }

        // 2. Clear physics forces if it uses a Rigidbody
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = targetPosition;
        }

        // 3. Update actual transform position
        targetPlayer.transform.position = targetPosition;

        // 4. Force Unity to process physics changes right now
        Physics.SyncTransforms();

        // 5. Re-enable Character Controller
        if (cc != null)
        {
            cc.enabled = true;
        }
    }
}

