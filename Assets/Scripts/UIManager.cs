using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText; // Reference to the TextMeshProUGUI component for displaying the score
    public GameObject CaughtPanel; // Reference to the panel that shows when the player is caught
    public static UIManager Instance { get; internal set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CaughtPanel.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        // Update the score display in the UI
        scoreText.text = $"Score points: {score}";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
