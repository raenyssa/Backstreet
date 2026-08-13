using UnityEngine;

public class MRTDoorRight : MonoBehaviour
{
    private Animator _doorAnim;

    void Start()
    {
        _doorAnim = GetComponent<Animator>();
    }

    public void Open()
    {
        _doorAnim.SetBool("IsOpen", true);
    }

    public void Close()
    {
        _doorAnim.SetBool("IsOpen", false);
    }
}
