/*
 * Author: Raenyssa Lim
 * Date: 5th August 2026
 * File: PoliceSlidingDoorRight
 * Description: Controls the opening and closing animation of the right police sliding door.
 
 */
using UnityEngine;

/// <summary>
/// Controls the opening and closing animation of the right police sliding door.
/// </summary>
public class PoliceSlidingDoorRight : MonoBehaviour
{
    /// <summary>
    /// Stores the Animator component used to control the door animation.
    /// </summary>
    private Animator _doorAnim;

    /// <summary>
    /// Gets the Animator component attached to the right police sliding door.
    /// </summary>
    void Start()
    {
        // Get the Animator component attached to this GameObject.
        _doorAnim = GetComponent<Animator>();
    }

    /// <summary>
    /// Opens the right police sliding door by setting the animation state to open.
    /// </summary>
    public void Open()
    {
        // Set the right door animation to the open state.
        _doorAnim.SetBool("IsPoliceRightOpen", true);
    }

    /// <summary>
    /// Closes the right police sliding door by setting the animation state to closed.
    /// </summary>
    public void Close()
    {
        // Set the right door animation to the closed state.
        _doorAnim.SetBool("IsPoliceRightOpen", false);
    }
}