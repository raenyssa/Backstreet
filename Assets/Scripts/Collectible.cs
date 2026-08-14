/*
 * Author: Gladis Koh
 * Date: 9 th August 2026
 * File:Collectible
 * Description: Handles the behaviour of a collectible object that can be collected by the player.
 
 */
using UnityEngine;

/// <summary>
/// Handles the behaviour of a collectible object that can be collected by the player.
/// </summary>
public class Collectible : MonoBehaviour
{
    /// <summary>
    /// Collects the object and removes it from the scene.
    /// </summary>
    public void Collect()
    {
        // Destroy the collectible after it has been collected.
        Destroy(gameObject);
    }
}
