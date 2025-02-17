using UnityEngine;

/// <summary>
///     Animates the rotation of the claw machine handle.
/// </summary>
public class ClawMachineHandle : MonoBehaviour
{
    [SerializeField] private ControllerRotationReader controllerRotation;
    [SerializeField, Min(1f)] private float rotationRadius = 10f;

    /// <summary>
    ///     The axis of input for the handle.
    /// </summary>
    public Vector2 Axis { get; private set; }

    float ClampAngle(float angle, float from, float to)
    {
        // accepts e.g. -80, 80
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }

    private void Update()
    {
        if(controllerRotation != null)
        {
            var rotation = Quaternion.Inverse(controllerRotation.Rotation).eulerAngles;
            rotation.x = ClampAngle(rotation.x, -rotationRadius, rotationRadius);
            rotation.z = ClampAngle(rotation.z, -rotationRadius, rotationRadius);
            transform.localEulerAngles = rotation;

            float x = rotation.z > 180 ? rotation.z - 360 : rotation.z;
            float y = rotation.x > 180 ? rotation.x - 360 : rotation.x;
            Axis = new Vector2 (x / rotationRadius, -y / rotationRadius);
        }
        else
            Axis = Vector2.zero;

        //Debug.Log("Axis: " + Axis);
    }
}
