using UnityEngine;

public class HoldingJailDoor : MonoBehaviour
{
    private Animator _doorAnim;

    void Start()
    {
        _doorAnim = GetComponent<Animator>();
    }

    public void Open()
    {
        _doorAnim.SetBool("IsHoldingJailOpen", true);
    }

    public void Close()
    {
        _doorAnim.SetBool("IsHoldingJailOpen", false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Open();
        }
    }
}