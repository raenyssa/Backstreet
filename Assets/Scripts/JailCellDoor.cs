using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Controls the interaction and opening or closing of a jail cell door.
/// </summary>
public class JailCellDoor : MonoBehaviour
{
    /// <summary>
    /// Stores the amount the door can rotate when opened.
    /// </summary>
    public Vector3 rotateAmount = new Vector3(0, 90, 0);

    /// <summary>
    /// Keeps track of whether the jail cell door is currently open.
    /// </summary>
    bool isOpen = false;

    /// <summary>
    /// Toggles the jail cell door between its open and closed states.
    /// </summary>
    public void Interact()
    {
        // Get the Animator component attached to the door.
        var animator = GetComponent<Animator>();

        // Toggle the door animation based on its current state.
        animator.SetBool("isOpen", !isOpen);

        // Update the stored door state.
        isOpen = !isOpen;

        // Print a message to confirm that the door was interacted with.
        print("Door Interacted");

    }
}
