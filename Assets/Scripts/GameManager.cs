using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public string levelName;

    public static GameManager instance;

    public static int score;

    public UIManager uiManager;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        IncreaseScore(score > 0 ? score : 0);
    }

    void Start()
    {
        Debug.Log($"This is GameManager in {levelName}");
    }

    public void IncreaseScore(int s)
    {
        score += s;
        uiManager.UpdateScore(score);
    }

    public void UpdateCaught(string npcName)
    {
        uiManager.UpdateCaughtPanel(npcName, score);
    }
}