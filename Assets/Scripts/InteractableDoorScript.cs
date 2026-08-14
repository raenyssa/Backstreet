/*
 * Author: Marilyn Tan
 * Date: 8th August 2026
 * File: InteractableDoor
 * Description: Controls the opening and closing of an interactable door.
 
 */
using UnityEngine;

/// <summary>
/// Controls the opening and closing of an interactable door.
/// </summary>
public class InteractableDoorScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    /// <summary>
    /// Audio source used for the door interaction sound.
    /// </summary>
    /*Audio source for my door interaction*/
    public AudioSource audioSource;

    /// <summary>
    /// Stores the ID assigned to this door.
    /// </summary>
    [SerializeField]
    public int DoorID;

    /// <summary>
    /// Keeps track of whether the door is currently open.
    /// </summary>
    private bool isOpen = true;

    /// <summary>
    /// Toggles the door between its open and closed states and plays the door audio.
    /// </summary>
    public void open()
    {
        //activate the animator for the door
        var animatorComponent = GetComponent<Animator>();

        // Update the door animation based on its current state.
        animatorComponent.SetBool("IsOpen", isOpen);

        // Toggle the door state for the next interaction.
        isOpen = !isOpen;

        // Play the door audio if an AudioSource has been assigned.
        if (audioSource != null)//play the door audio
        {
            audioSource.Play();
        }
        
    }
}