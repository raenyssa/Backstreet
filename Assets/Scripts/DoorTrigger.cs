/*
 * Author: Raenyssa Lim
 * Date: 11th August 2026
 * File: DoorTrigger
 * Description: Controls the opening and closing of doors when the player enters or exits a trigger.
 
 */
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the opening and closing of doors when the player enters or exits a trigger.
/// The type of door is determined by the active scene.
/// </summary>
public class DoorTrigger : MonoBehaviour
{
    /// <summary>
    /// Reference to the left door GameObject.
    /// </summary>
    [SerializeField] private GameObject LeftDoor;

    /// <summary>
    /// Reference to the right door GameObject.
    /// </summary>
    [SerializeField] private GameObject RightDoor;

    /// <summary>
    /// Audio source used to play the police sliding door sound.
    /// </summary>
    public AudioSource PoliceSlidingDoorAudio;

    /// <summary>
    /// Opens the appropriate doors when the player enters the trigger.
    /// The door type is selected based on the active scene.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Get the name of the currently active scene.
        string sceneName = SceneManager.GetActiveScene().name;

        // Check if the object entering the trigger is the player.
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger");

            // Open the MRT doors in Marilyn's scene.
            if (sceneName == "Marilyn's scene")
            {
                RightDoor.GetComponent<MRTDoorRight>().Open();
                LeftDoor.GetComponent<MRTDoorLeft>().Open();
            }

            // Open the police sliding doors in the Jaywalk scene.
            if (sceneName == "JaywalkScene")
            {
                LeftDoor.GetComponent<PoliceSlidingDoor>().Open();
                RightDoor.GetComponent<PoliceSlidingDoorRight>().Open();
            }
            else
            {
                return;
            }
        }
    }

    /// <summary>
    /// Closes the appropriate doors when the player exits the trigger.
    /// The door type is selected based on the active scene.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        // Get the name of the currently active scene.
        string sceneName = SceneManager.GetActiveScene().name;

        // Check if the object leaving the trigger is the player.
        if (other.CompareTag("Player"))
        {
            // Close the police sliding doors and play the door audio in the Jaywalk scene.
            if (sceneName == "JaywalkScene")
            {
                LeftDoor.GetComponent<PoliceSlidingDoor>().Close();
                RightDoor.GetComponent<PoliceSlidingDoorRight>().Close();

                // Play the sliding door sound if an audio source has been assigned.
                if (PoliceSlidingDoorAudio != null)
                {
                    PoliceSlidingDoorAudio.Play();
                }
            }

            // Close the MRT doors in Marilyn's scene.
            else if (sceneName == "Marilyn's scene")
            {
                LeftDoor.GetComponent<MRTDoorLeft>().Close();
                RightDoor.GetComponent<MRTDoorRight>().Close();
            }
            else
            {
                return;
            }
        }
    }
}

