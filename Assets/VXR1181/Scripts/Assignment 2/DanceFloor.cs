using UnityEngine;

namespace Assignment2
{
    /// <summary>
    ///     Triggers the dance animation for the particular floor.
    /// </summary>
    public class DanceFloor : MonoBehaviour
    {
        public int AnimationClip => animationClip;
        [SerializeField] private int animationClip;
    }
}