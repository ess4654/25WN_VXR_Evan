using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Handles Jump interaction for the XR Origin.
/// </summary>
public class SimpleJumpProvider : MonoBehaviour
{
    [SerializeField] private InputActionReference jumpInput;
    [SerializeField] private float jumpHeight = 2f;

    private CharacterController controller;
    private Vector3 velocity;

    #region SETUP

    private void Start()
    {
        controller = GetComponentInParent<CharacterController> ();
    }

    private void OnEnable()
    {
        jumpInput.action.performed += Jump;
    }

    private void OnDisable()
    {
        jumpInput.action.performed -= Jump;
    }

    #endregion

    #region JUMPING

    private void Jump(InputAction.CallbackContext context)
    {
        if (controller.isGrounded)
            ApplyJumpForce();
    }

    private void ApplyJumpForce()
    {
        velocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y) * Vector2.up;
    }

    #endregion

    private void Update()
    {
        velocity += Physics.gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}