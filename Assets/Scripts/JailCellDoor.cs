using UnityEngine;
using UnityEngine.UIElements;

public class JailCellDoor : MonoBehaviour
{
    public Vector3 rotateAmount = new Vector3(0, 90, 0);
    bool isOpen = false;

    public void Interact()
    {
        var animator = GetComponent<Animator>();
        animator.SetBool("isOpen", !isOpen);
        isOpen = !isOpen;
        print("Door Interacted");

    }
}
