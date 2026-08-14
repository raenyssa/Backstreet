/*
 * Author: Gladis Koh
 * Date: 9th August 2026
 * File: LeftDoor
 * Description: Controls the movement and interaction of the left door.
 
 */
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the movement and interaction of the left door.
/// The door moves between its closed and open positions when the player presses E nearby.
/// </summary>
public class LeftDoor : MonoBehaviour
{
    /// <summary>
    /// The distance the door moves when opening.
    /// </summary>
    public float distance = 2f;

    /// <summary>
    /// The speed at which the door moves between its open and closed positions.
    /// </summary>
    public float speed = 3f;

    /// <summary>
    /// Stores the door's original closed position.
    /// </summary>
    private Vector3 closedPos;

    /// <summary>
    /// Stores the door's position when it is fully open.
    /// </summary>
    private Vector3 openPos;

    /// <summary>
    /// Keeps track of whether the door is currently open.
    /// </summary>
    private bool isOpen = false;

    /// <summary>
    /// Keeps track of whether the player is within interaction range of the door.
    /// </summary>
    private bool playerNearby = false;

    /// <summary>
    /// Sets the initial closed position and calculates the door's open position.
    /// </summary>
    void Start()
    {
        // Store the door's starting position as its closed position.
        closedPos = transform.position;

        // Calculate the position the door moves to when opened.
        openPos = closedPos - transform.right * distance;
    }

    /// <summary>
    /// Checks for player interaction and smoothly moves the door
    /// towards its current target position.
    /// </summary>
    void Update()
    {
        // Toggle the door's open state when the player presses E nearby.
        if (playerNearby && Keyboard.current.eKey.wasPressedThisFrame)
        {
            isOpen = !isOpen;
        }

        // Choose the open or closed position based on the current door state.
        Vector3 target = isOpen ? openPos : closedPos;

        // Smoothly move the door towards the target position.
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );
    }

    /// <summary>
    /// Detects when the player enters the door's trigger area.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player.
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log(gameObject.name + " Player entered");
        }
    }

    /// <summary>
    /// Detects when the player leaves the door's trigger area.
    /// </summary>
    /// <param name="other">The collider that exited the trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving the trigger is the player.
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}