using UnityEngine;
using UnityEngine.InputSystem;

public class LeftDoor : MonoBehaviour
{
    public float distance = 2f;
    public float speed = 3f;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen = false;
    private bool playerNearby = false;

    void Start()
    {
        closedPos = transform.position;
        openPos = closedPos - transform.right * distance;
    }

    void Update()
{
    if (playerNearby && Keyboard.current.eKey.wasPressedThisFrame)
    {
        isOpen = !isOpen;
    }

    Vector3 target = isOpen ? openPos : closedPos;

    transform.position = Vector3.MoveTowards(
        transform.position,
        target,
        speed * Time.deltaTime
    );
}

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        playerNearby = true;
        Debug.Log(gameObject.name + " Player entered");
    }
}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}