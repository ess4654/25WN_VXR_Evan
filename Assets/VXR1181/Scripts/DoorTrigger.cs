using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        animator.SetTrigger("Open");
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetTrigger("Close");
    }
}
