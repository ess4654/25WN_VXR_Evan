using UnityEngine;

public class SpinCube : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("Clockwise", true);
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("Clockwise", false);
    }
}
