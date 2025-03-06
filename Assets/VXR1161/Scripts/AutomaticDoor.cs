using UnityEngine;

namespace MegaMall
{
    /// <summary>
    ///     Handles the door positions for the automatic doors.
    /// </summary>
    public class AutomaticDoor : MonoBehaviour
    {
        [SerializeField] private Transform doorL, doorR;
        [SerializeField] private float openPosition;
        [SerializeField, Range(0f, 1f)] private float doorPosition;

        private void OnValidate() => SetDoorPosition();

        private void SetDoorPosition()
        {
            if(doorL == null)
                doorL = transform.GetChild(0);
            if (doorR == null)
                doorR = transform.GetChild(1);

            var doorPos = openPosition * doorPosition * Vector3.forward;
            if (doorL != null)
                doorL.localPosition = doorPos;
            if (doorR != null)
                doorR.localPosition = -doorPos;
        }
    }
}