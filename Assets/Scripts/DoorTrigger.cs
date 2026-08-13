using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject LeftDoor;
    [SerializeField] private GameObject RightDoor;

    private void OnTriggerEnter(Collider other)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger");
                if (sceneName == "Marilyn's scene")
                {
                    RightDoor.GetComponent<MRTDoorRight>().Open();
                    LeftDoor.GetComponent<MRTDoorLeft>().Open();
                }
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

    private void OnTriggerExit(Collider other)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (other.CompareTag("Player"))
        {
            if (sceneName == "JaywalkScene")
                {
                    LeftDoor.GetComponent<PoliceSlidingDoor>().Close();
                    RightDoor.GetComponent<PoliceSlidingDoorRight>().Close();
                }
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