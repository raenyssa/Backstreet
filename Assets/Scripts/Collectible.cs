using UnityEngine;

public class Collectible : MonoBehaviour
{

    public void Collect()
    {
        //Destroy the coin after the sound has played
        Destroy(gameObject);

    }
}
