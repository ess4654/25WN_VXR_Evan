using UnityEngine;

namespace MegaMall
{
    /// <summary>
    ///     Controls the open/close position of the security doors.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer), typeof(BoxCollider))]
    public class SecurityDoor : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float doorPosition;

        private MeshRenderer renderer;
        private MaterialPropertyBlock props;
        private BoxCollider collider;

        private const string property = "_Offset";
        private const float threshold = -.125f;
        private const float doorHeight = 3f;
        private readonly Vector3 colliderCenter = new Vector3(0.0625f, 1.5f, -1.5f);

        private void OnValidate() => SetDoorPosition();

        private void SetDoorPosition()
        {
            if(collider == null)
                collider = GetComponent<BoxCollider>();
            if(renderer == null)
                renderer = GetComponent<MeshRenderer>();
            if(props == null)
            {
                props = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(props);
            }

            props.SetVector(property, new Vector2(0, doorPosition * threshold));
            renderer.SetPropertyBlock(props);

            collider.center = colliderCenter + (doorHeight * doorPosition * Vector3.up);
        }
    }
}