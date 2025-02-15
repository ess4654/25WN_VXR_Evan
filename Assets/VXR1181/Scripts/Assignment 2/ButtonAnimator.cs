using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Animator))]
public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,  IPointerDownHandler, IPointerUpHandler
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("Hover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Hover", false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetBool("Click", true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        animator.SetBool("Click", false);
    }
}