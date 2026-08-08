using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject LeftDoor;
    [SerializeField] private GameObject RightDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger");
            LeftDoor.GetComponent<PoliceSlidingDoor>().Open();
            RightDoor.GetComponent<PoliceSlidingDoorRight>().Open();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LeftDoor.GetComponent<PoliceSlidingDoor>().Close();
            RightDoor.GetComponent<PoliceSlidingDoorRight>().Close();
        }
    }
}