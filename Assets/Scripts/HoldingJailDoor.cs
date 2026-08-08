using UnityEngine;

public class HoldingJailDoor : MonoBehaviour
{
    private Animator _doorAnim;
    private bool isOpen = true;

    public void Open()
    {
        var animatorComponent = GetComponent<Animator>();
        animatorComponent.SetBool("IsHoldingJailOpen", isOpen);
        isOpen = !isOpen;
    }

}