/*
 * Author: Marilyn Tan
 * Date: 11th August 2026
 * File: MoveCamera
 * Description: Controls the movement of the camera rotation
 
 */
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCamera : MonoBehaviour
{
    /// <summary>
    /// Gameobject component of the player
    /// </summary>
    [SerializeField] GameObject playerCamera;

    /// <summary>
    /// Mouse movement on the X axis
    /// </summary>
    float MouseX;

    /// <summary>
    /// Mouse movement on the Y axis
    /// </summary>
    float MouseY;

    // Update is called once per frame
    void Update()
    {
        MoveCam();
    }

    void MoveCam()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        MouseX = mouseDelta.x;
        MouseY = mouseDelta.y;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + MouseX / 10, 0);
    }
}
