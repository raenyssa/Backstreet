/*
 * Author: Raenyssa Lim
 * Date: 11th August 2026
 * File: HoldingJailDoor
 * Description: Controls the opening and closing animation of the holding jail door.
 
 */
using UnityEngine;

/// <summary>
/// Controls the opening and closing animation of the holding jail door.
/// </summary>
public class HoldingJailDoor : MonoBehaviour
{
    /// <summary>
    /// Stores the Animator component used to control the door animation.
    /// </summary>
    private Animator _doorAnim;

    /// <summary>
    /// Keeps track of whether the door is currently open.
    /// </summary>
    private bool isOpen = true;

    /// <summary>
    /// Toggles the holding jail door between its open and closed states.
    /// </summary>
    public void Open()
    {
        // Get the Animator component attached to the door.
        var animatorComponent = GetComponent<Animator>();

        // Update the animation state using the current open state.
        animatorComponent.SetBool("IsHoldingJailOpen", isOpen);

        // Toggle the door's open state for the next interaction.
        isOpen = !isOpen;
    }
}