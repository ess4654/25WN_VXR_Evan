using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationSwapper : MonoBehaviour
{
    [SerializeField] private int animationClip;
    
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetInteger("Animation Clip", animationClip);
    }
}
