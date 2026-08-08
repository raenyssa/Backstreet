using UnityEngine;

public class PoliceSlidingDoor : MonoBehaviour
{
    private Animator _doorAnim;

    void Start()
    {
        _doorAnim = GetComponent<Animator>();
    }

    public void Open()
    {
        _doorAnim.SetBool("IsPoliceSlidingOpen", true);
    }

    public void Close()
    {
        _doorAnim.SetBool("IsPoliceSlidingOpen", false);
    }
}