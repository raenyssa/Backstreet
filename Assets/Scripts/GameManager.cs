using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages game-wide information such as the player's score and caught NPC information.
/// This object persists between scene changes.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Stores the name of the current level.
    /// </summary>
    public string levelName;

    /// <summary>
    /// Provides a single shared instance of the GameManager.
    /// </summary>
    public static GameManager instance;

    /// <summary>
    /// Stores the player's current score across scenes.
    /// </summary>
    public static int score;

    /// <summary>
    /// Reference to the UIManager used to update game interface elements.
    /// </summary>
    public UIManager uiManager;

    /// <summary>
    /// Ensures only one GameManager instance exists and keeps it when loading new scenes.
    /// </summary>
    private void Awake()
    {
        // Destroy this GameManager if another instance already exists.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set this object as the shared GameManager instance.
        instance = this;

        // Keep the GameManager when changing scenes.
        DontDestroyOnLoad(gameObject);

        // Initialise the score using the existing score if it is greater than zero.
        IncreaseScore(score > 0 ? score : 0);
    }

    /// <summary>
    /// Logs the name of the level when the GameManager starts.
    /// </summary>
    void Start()
    {
        Debug.Log($"This is GameManager in {levelName}");
    }

    /// <summary>
    /// Increases the player's score and updates the score displayed on the UI.
    /// </summary>
    /// <param name="s">The number of points to add to the current score.</param>
    public void IncreaseScore(int s)
    {
        // Add the given points to the player's total score.
        score += s;

        // Update the score displayed on the UI.
        uiManager.UpdateScore(score);
    }

    /// <summary>
    /// Updates the caught NPC panel with the NPC's name and the current score.
    /// </summary>
    /// <param name="npcName">The name of the NPC that was caught.</param>
    public void UpdateCaught(string npcName)
    {
        // Update the UI with the caught NPC's information and current score.
        uiManager.UpdateCaughtPanel(npcName, score);
    }
}