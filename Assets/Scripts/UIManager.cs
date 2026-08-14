/*
 * Author: Marilyn Tan
 * Date: 4th August 2026
 * File: TerroristNPC
 * Description: Manages the game's user interface, including mission panels, menus,
score displays, scene transitions, player teleportation, and game states.
 fleeing from the player, dropping an object, and handling the mission timer.
 
 */
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the game's user interface, including mission panels, menus,
/// score displays, scene transitions, player teleportation, and game states.
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the player GameObject.
    /// </summary>
    // Drag your Player GameObject here in the Inspector
    public GameObject player; 
    
    /// <summary>
    /// Reference to the Transform where the player will be teleported.
    /// </summary>
    // Drag an empty GameObject or Target Pad here to set the destination
    public Transform destination; 

    /// <summary>
    /// Reference to the destination GameObject for the jaywalking mission.
    /// </summary>
    public GameObject JaywalkDestination;

    /// <summary>
    /// Reference to the destination GameObject for the vandalism mission.
    /// </summary>
    public GameObject VandaliseDestination;

    /// <summary>
    /// Text component used to display the caught NPC message.
    /// </summary>
    public TMP_Text caughttext;

    /// <summary>
    /// Text component used to display the player's score.
    /// </summary>
    public TMP_Text scoreText;

    /// <summary>
    /// Panel displayed when an NPC is caught.
    /// </summary>
    public GameObject CaughtPanel;

    /// <summary>
    /// Panel displaying information about the jaywalking mission.
    /// </summary>
    public GameObject JaywalkMissionPanel;

    /// <summary>
    /// Panel displaying the game menu.
    /// </summary>
    public GameObject MenuPanel;

    /// <summary>
    /// Panel displaying information about the shoplifting mission.
    /// </summary>
    public GameObject ShopliftMissionPanel;

    /// <summary>
    /// Panel displaying information about the terrorist mission.
    /// </summary>
    public GameObject TerroristMissionPanel;

    /// <summary>
    /// Panel displaying information about the vandalism mission.
    /// </summary>
    public GameObject VandaliseMissionPanel;

    /// <summary>
    /// Panel displayed when the player loses a mission.
    /// </summary>
    public GameObject LostPanel;

    /// <summary>
    /// Panel displayed at the start of the game.
    /// </summary>
    public GameObject StartMenu;

    /// <summary>
    /// Panel displaying instructions on how to play the game.
    /// </summary>
    public GameObject HowToPlay;

    /// <summary>
    /// Reference to the NPC used to start the jaywalking mission.
    /// </summary>
    public GameObject JaywalkTaskNPC;

    /// <summary>
    /// Reference to the NPC used to start the vandalism mission.
    /// </summary>
    public GameObject VandaliseTaskNPC;

    /// <summary>
    /// Reference to the NPC used to start the terrorist mission.
    /// </summary>
    public GameObject TerroristTaskNPC;

    /// <summary>
    /// Reference to the NPC used to start the shoplifting mission.
    /// </summary>
    public GameObject ShopliftTaskNPC;

    /// <summary>
    /// Panel displayed when the player successfully completes a mission.
    /// </summary>
    public GameObject WinPanel;

    /// <summary>
    /// Stores the singleton instance of the UIManager.
    /// </summary>
    public static UIManager Instance { get; internal set; }

    /// <summary>
    /// Audio source used for the supermarket bell sound.
    /// </summary>
    public AudioSource SupermarketBellAudio;

    /// <summary>
    /// Initialises the UI by hiding all panels when the UIManager starts.
    /// </summary>
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Hide all UI panels when the scene starts.
        JaywalkMissionPanel.SetActive(false);
        CaughtPanel.SetActive(false);
        MenuPanel.SetActive(false);
        ShopliftMissionPanel.SetActive(false);
        TerroristMissionPanel.SetActive(false);
        LostPanel.SetActive(false);
        HowToPlay.SetActive(false);
        WinPanel.SetActive(false);
        VandaliseMissionPanel.SetActive(false);
    }

    /// <summary>
    /// Updates the score text displayed on the UI.
    /// </summary>
    /// <param name="score">The player's current score.</param>
    public void UpdateScore(int score)
    {
        // Update the score display in the UI
        scoreText.text = $"Score points: {score}";
    }

    /// <summary>
    /// Updates the caught panel with the NPC's name and the score earned.
    /// </summary>
    /// <param name="NPCName">The name of the NPC that was caught.</param>
    /// <param name="score">The number of points earned.</param>
    public void UpdateCaughtPanel(string NPCName, int score)
    {
        // Display the caught NPC information and earned points.
        caughttext.text = $" Congratulations!\nYou have caught the {NPCName} and earned {score} points!";
    }

    /// <summary>
    /// Opens the start menu and makes the cursor visible.
    /// </summary>
    public void OpenStartMenu()
    {
        StartMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;        
    }

    /// <summary>
    /// Closes the start menu and hides the cursor.
    /// </summary>
    public void CloseStartMenu()
    {
        StartMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;        
    }

    /// <summary>
    /// Opens the How To Play panel and makes the cursor visible.
    /// </summary>
    public void OpenHowToPlayPanel()
    {
        HowToPlay.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;        
    }

    /// <summary>
    /// Closes the How To Play panel and hides the cursor.
    /// </summary>
    public void CloseHowToPlayPanel()
    {
        HowToPlay.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;        
    }

    /// <summary>
    /// Closes the start menu and opens the How To Play panel.
    /// </summary>
    public void Enter()
    {
        CloseStartMenu();
        OpenHowToPlayPanel();
    }

    // Update is called once per frame

    /// <summary>
    /// Opens the jaywalking mission panel and displays the cursor.
    /// </summary>
    public void OpenJaywalkMissionPanel()
    {
        JaywalkMissionPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Closes the jaywalking mission panel and locks the cursor.
    /// </summary>
    public void CloseJaywalkMissionPanel()
    {
        JaywalkMissionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Opens the vandalism mission panel and displays the cursor.
    /// </summary>
    public void OpenVandaliseMissionPanel()
    {
        VandaliseMissionPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Closes the vandalism mission panel and locks the cursor.
    /// </summary>
    public void CloseVandaliseMissionPanel()
    {
        VandaliseMissionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Opens the shoplifting mission panel and displays the cursor.
    /// </summary>
    public void OpenShopliftMissionPanel()
    {
        ShopliftMissionPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Closes the shoplifting mission panel and locks the cursor.
    /// </summary>
    public void CloseShopliftMissionPanel()
    {
        ShopliftMissionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Opens the terrorist mission panel and displays the cursor.
    /// </summary>
    public void OpenTerroristMissionPanel()
    {
        TerroristMissionPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    /// <summary>
    /// Closes the terrorist mission panel and locks the cursor.
    /// </summary>
    public void CloseTerroristMissionPanel()
    {
        TerroristMissionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    /// <summary>
    /// Opens the caught panel and displays the cursor.
    /// </summary>
    public void OpenCaughtPanel()
    {
        CaughtPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Closes the caught panel and locks the cursor.
    /// </summary>
    public void CloseCaughtPanel()
    {
        CaughtPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Opens the win panel and displays the cursor.
    /// </summary>
    public void OpenWinPanel()
    {
        WinPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Opens the lost panel and displays the cursor.
    /// </summary>
    public void OpenLostPanel()
    {
        LostPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Closes the lost panel and locks the cursor.
    /// </summary>
    public void CloseLostPanel()
    {
        LostPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Toggles the visibility of the game menu and updates the cursor state.
    /// </summary>
    public void ToggleMenuPanel()
    {
        // Toggle the menu panel between active and inactive.
        MenuPanel.SetActive(!MenuPanel.activeSelf);

        // Show the cursor when the menu is open.
        Cursor.visible = MenuPanel.activeSelf;

        // Unlock the cursor when the menu is open and lock it when closed.
        Cursor.lockState = MenuPanel.activeSelf ? 
        CursorLockMode.None : 
        CursorLockMode.Locked;
    }

    /// <summary>
    /// Restarts the currently active scene.
    /// </summary>
    public void Restart()
    {
        // Reload the current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Handles the player's loss by closing the lost panel and restarting the scene.
    /// </summary>
    public void Lose()
    {
        Debug.Log("You have DIED!");

        // Close the lost panel before restarting the scene.
        CloseLostPanel();

        // Reload the current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Completes the MRT mission and loads the JaywalkScene.
    /// </summary>
    public void CompleteMRT()
    {
        // Get the name of the currently active scene.
        string sceneName = SceneManager.GetActiveScene().name;

        // Only complete the MRT mission from Marilyn's scene.
        if (sceneName != "Marilyn's scene")
        {
            return;
        }

        Debug.Log("MRT mission completed! Loading JaywalkScene...");

        // Close the caught panel before changing scenes.
        CloseCaughtPanel();

        Debug.Log("Panel Closed");

        // Load the next mission scene.
        SceneManager.LoadScene("JaywalkScene");
    }

    /// <summary>
    /// Completes the supermarket mission and loads the JaywalkScene.
    /// </summary>
    public void CompleteSupermarket()
    {
        // Get the name of the currently active scene.
        string sceneName = SceneManager.GetActiveScene().name;

        // Only complete the supermarket mission from the Gladis scene.
        if (sceneName != "Gladis")
        {
            return;
        }

        Debug.Log("Shoplift mission completed! Loading JaywalkScene...");

        // Close the caught panel before changing scenes.
        CloseCaughtPanel();

        Debug.Log("Panel Closed");

        // Load the next mission scene.
        SceneManager.LoadScene("JaywalkScene");
    }

    /// <summary>
    /// Exits the game. When running in the Unity Editor, it stops Play Mode instead.
    /// </summary>
    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    /// <summary>
    /// Teleports the player to the configured destination in the JaywalkScene.
    /// </summary>
    public void TeleportPlayer()
    {
        // Get the name of the currently active scene.
        string sceneName = SceneManager.GetActiveScene().name;

        // Only perform the teleport in the JaywalkScene.
        if (sceneName != "JaywalkScene")
        {
            return;
        }

        // Stop if the player or destination has not been assigned.
        if (player == null || destination == null) return;

        // Reset the caught state before teleporting.
        PlayerScript.caughtNPC = false; // Reset the caught state before teleporting;

        Debug.Log("Teleporting player to the destination...");
        
        // Safely move using our helper method
        ExecuteTeleport(player, destination.position);

        Debug.Log("Player has been teleported to the destination!");

        // Close the caught panel after one second.
        Invoke("CloseCaughtPanel", 1f); // Close the caught panel after 1 second
    }

    /// <summary>
    /// Accepts the jaywalking mission, teleports the player to the mission area,
    /// and removes the task NPC.
    /// </summary>
    public void AcceptJaywalkMission()
    {
        // Stop if the player or mission destination has not been assigned.
        if (player == null || JaywalkDestination == null) return;

        // Logic to accept the jaywalking mission
        Debug.Log("Jaywalking mission accepted!");

        // Close the mission panel.
        CloseJaywalkMissionPanel();
        
        // Safely move using our helper method
        ExecuteTeleport(player, JaywalkDestination.transform.position);

        Debug.Log("Destroying at" + JaywalkTaskNPC.transform.position);

        // Destroy the JaywalkTaskNPC GameObject after teleporting.
        Destroy(JaywalkTaskNPC); // Destroy the JaywalkTaskNPC GameObject after teleporting
    }

    /// <summary>
    /// Accepts the vandalism mission, teleports the player to the mission area,
    /// and removes the task NPC.
    /// </summary>
    public void AcceptVandaliseMission()
    {
        // Stop if the player or mission destination has not been assigned.
        if (player == null || VandaliseDestination == null) return;

        // Logic to accept the vandalise mission
        Debug.Log("Vandalise mission accepted!");

        // Close the mission panel.
        CloseVandaliseMissionPanel();
        
        // Safely move using our helper method
        ExecuteTeleport(player, VandaliseDestination.transform.position);

        Debug.Log("Destroying at" + VandaliseTaskNPC.transform.position);

        // Destroy the VandaliseTaskNPC GameObject after teleporting.
        Destroy(VandaliseTaskNPC); // Destroy the VandaliseTaskNPC GameObject after teleporting
    }

    /// <summary>
    /// Accepts the terrorist mission, closes its mission panel,
    /// loads Marilyn's scene, and removes the terrorist task NPC.
    /// </summary>
    public void AcceptTerroristMission()
    {
        Debug.Log("Terrorist mission accepted!");

        // Close the terrorist mission panel.
        CloseTerroristMissionPanel();

        // Load the terrorist mission scene.
        SceneManager.LoadScene("Marilyn's scene");

        // Destroy the terrorist task NPC.
        Destroy(TerroristTaskNPC);
    }

    /// <summary>
    /// Accepts the shoplifting mission, closes its mission panel,
    /// loads the supermarket scene, removes the task NPC, and plays the supermarket bell.
    /// </summary>
    public void AcceptShoplifterMission()
    {
        Debug.Log("ShopliftMissionPanel mission accept!");

        // Close the shoplifting mission panel.
        CloseShopliftMissionPanel();

        // Load the supermarket mission scene.
        SceneManager.LoadScene("Gladis");

        // Destroy the shoplifting task NPC.
        Destroy(ShopliftTaskNPC);

        // Play the supermarket bell sound if an AudioSource has been assigned.
        if (SupermarketBellAudio != null)
        {
            SupermarketBellAudio.Play();
        }
    }

    /// <summary>
    /// Safely teleports a player while handling active CharacterController
    /// and Rigidbody components.
    /// </summary>
    /// <param name="targetPlayer">The player GameObject to teleport.</param>
    /// <param name="targetPosition">The destination position for the player.</param>
    /// <remarks>
    /// Safely updates positions when components like CharacterController or Rigidbody are active.
    /// </remarks>
    private void ExecuteTeleport(GameObject targetPlayer, Vector3 targetPosition)
    {
        // Get the CharacterController attached to the player.
        CharacterController cc = targetPlayer.GetComponent<CharacterController>();

        // Get the Rigidbody attached to the player.
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