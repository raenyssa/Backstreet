using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TMP_Text caughttext; // Reference to the TextMeshProUGUI component for displaying the caught message
    public TMP_Text scoreText; // Reference to the TextMeshProUGUI component for displaying the score
    public GameObject CaughtPanel; // Reference to the panel that shows when the player is caught
    public GameObject MissionPanel; // Reference to the panel that shows mission-related information
    public GameObject MenuPanel; // Reference to the panel that shows the menu
    public static UIManager Instance { get; internal set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MissionPanel.SetActive(false);
        CaughtPanel.SetActive(false);
        MenuPanel.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        // Update the score display in the UI
        scoreText.text = $"Score points: {score}";
    }
    public void UpdateCaughtPanel(string NPCName)
    {
        caughttext.text = $" Congratulations!\nYou have caught the {NPCName} and earned 1000 points!";
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
    public void CompleteJaywalk()
    {
        
    }
}
