using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Reads the rotation value for the left controller.
/// </summary>
public class ControllerRotationReader : MonoBehaviour
{
    [SerializeField] private InputActionProperty rotationAction;

    /// <summary>
    ///     The velocity of the input controller.
    /// </summary>
    public Quaternion Rotation { get; private set; } = Quaternion.identity;

    private void Update()
    {
        Rotation = rotationAction.action.ReadValue<Quaternion>();
        //Debug.Log(name + ": " + Rotation);
    }
}