/*
 * Author: Marilyn Tan
 * Date: 11th August 2026
 * File: MRTDoorLeft
 * Description: Controls the opening and closing animation of the left MRT door.
 
 */
using UnityEngine;

/// <summary>
/// Controls the opening and closing animation of the left MRT door.
/// </summary>
public class MRTDoorLeft : MonoBehaviour
{
    /// <summary>
    /// Stores the Animator component used to control the door animation.
    /// </summary>
    private Animator _doorAnim;

    /// <summary>
    /// Gets the Animator component attached to the MRT door.
    /// </summary>
    void Start()
    {
        // Get the Animator component attached to this GameObject.
        _doorAnim = GetComponent<Animator>();
    }

    /// <summary>
    /// Opens the left MRT door by setting the animation state to open.
    /// </summary>
    public void Open()
    {
        // Set the door animation to the open state.
        _doorAnim.SetBool("IsOpen", true);
    }

    /// <summary>
    /// Closes the left MRT door by setting the animation state to closed.
    /// </summary>
    public void Close()
    {
        // Set the door animation to the closed state.
        _doorAnim.SetBool("IsOpen", false);
    }
}