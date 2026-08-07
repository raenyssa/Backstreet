using UnityEngine;

public class PoliceSlidingDoorRight : MonoBehaviour
{
    private Animator _doorAnim;

    void Start()
    {
        _doorAnim = GetComponent<Animator>();
    }

    public void Open()
    {
        _doorAnim.SetBool("IsPoliceRightOpen", true);
    }

    public void Close()
    {
        _doorAnim.SetBool("IsPoliceRightOpen", false);
    }
}