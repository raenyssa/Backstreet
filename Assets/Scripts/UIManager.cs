using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText; // Reference to the TextMeshProUGUI component for displaying the score
    public GameObject CaughtPanel; // Reference to the panel that shows when the player is caught
    public GameObject MissionPanel; // Reference to the panel that shows mission-related information
    public static UIManager Instance { get; internal set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MissionPanel.SetActive(false);
        CaughtPanel.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        // Update the score display in the UI
        scoreText.text = $"Score points: {score}";
    }
    // Update is called once per frame
    public void OpenMissionPanel()
    {
        MissionPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMissionPanel()
    {
        MissionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
