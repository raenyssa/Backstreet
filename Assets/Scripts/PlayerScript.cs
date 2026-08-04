using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    int score = 0;
    public int keycards = 0;

    [SerializeField]
    private GameObject playerCamera;

    [SerializeField]
    private float interactDistance = 3f;

    public AudioSource audioSource;
    private GameObject currentNPC;

    public static bool caughtNPC = false;

    [SerializeField]
    GameObject currentCollider;

    void OnInteract(InputValue value)
    {

        if (Physics.Raycast(playerCamera.transform.position,
            playerCamera.transform.forward, out RaycastHit hit, interactDistance))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.CompareTag("NPC"))
            {   

                print($"Looking at {hit.collider.gameObject.name}");
                currentNPC = hit.collider.gameObject;
                caughtNPC = true;
            }
            else
            {
                currentNPC = null;
            }
        }
    }

    public static bool IsCaught()
    {
        return caughtNPC;
    }
}