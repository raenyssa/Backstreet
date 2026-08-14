/*
 * Author: Raenyssa Lim
 * Date: 5th August 2026
 * File: PoliceSlidingDoor
 * Description: Controls the opening and closing animation of the police sliding door.
 
 */
using UnityEngine;

/// <summary>
/// Controls the opening and closing animation of the police sliding door.
/// </summary>
public class PoliceSlidingDoor : MonoBehaviour
{
    /// <summary>
    /// Stores the Animator component used to control the door animation.
    /// </summary>
    private Animator _doorAnim;

    /// <summary>
    /// Gets the Animator component attached to the police sliding door.
    /// </summary>
    void Start()
    {
        // Get the Animator component attached to this GameObject.
        _doorAnim = GetComponent<Animator>();
    }

    /// <summary>
    /// Opens the police sliding door by setting the animation state to open.
    /// </summary>
    public void Open()
    {
        // Set the door animation to the open state.
        _doorAnim.SetBool("IsPoliceSlidingOpen", true);
    }

    /// <summary>
    /// Closes the police sliding door by setting the animation state to closed.
    /// </summary>
    public void Close()
    {
        // Set the door animation to the closed state.
        _doorAnim.SetBool("IsPoliceSlidingOpen", false);
    }
}