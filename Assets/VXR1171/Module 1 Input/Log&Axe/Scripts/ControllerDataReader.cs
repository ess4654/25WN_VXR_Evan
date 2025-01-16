using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Reads data from the touch controller.
/// </summary>
public class ControllerDataReader : MonoBehaviour
{
    [SerializeField] private InputActionProperty velocityAction;

    /// <summary>
    ///     The velocity of the input controller.
    /// </summary>
    public Vector3 Velocity { get ; private set; } = Vector3.zero;

    private void Update()
    {
        Velocity = velocityAction.action.ReadValue<Vector3>();
        Debug.Log(name + ": " + Velocity);
    }
}