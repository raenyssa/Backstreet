using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static PersistentUI Instance { get; private set; }

    private void Awake()
    {
        // Prevent duplicate UI managers from spawning
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        // Persist the entire UI hierarchy across scenes
        DontDestroyOnLoad(gameObject);
    }
}
