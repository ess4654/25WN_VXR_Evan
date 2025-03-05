using UnityEngine;

namespace MegaMall
{
    /// <summary>
    ///     Controls the open/close position of the security doors.
    /// </summary>
    public class SecurityDoor : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float doorPosition;

        private void OnValidate() => SetDoorPosition();

        private void SetDoorPosition()
        {

        }
    }
}