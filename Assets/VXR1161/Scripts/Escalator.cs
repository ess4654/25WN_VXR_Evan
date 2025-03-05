using UnityEngine;

namespace MegaMall
{
    /// <summary>
    ///     Animates the treads of the escalator.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class Escalator : MonoBehaviour
    {
        [SerializeField, Min(0)] private int treadIndex = 1;
        [SerializeField, Min(0)] private int railingIndex = 3;
        [SerializeField, Min(.01f)] private float speed = 1;
        [SerializeField] private bool reverse;

        private MeshRenderer renderer;
        private MaterialPropertyBlock treadProps;
        private MaterialPropertyBlock railingProps;
        private float offset;
        private const float threshold = .125f;
        private const string property = "_Offset";

        private void Awake()
        {
            treadProps = new MaterialPropertyBlock();
            railingProps = new MaterialPropertyBlock();
            renderer = GetComponent<MeshRenderer>();
            renderer.GetPropertyBlock(treadProps, treadIndex);
            renderer.GetPropertyBlock(railingProps, railingIndex);
        }

        private void Update()
        {
            //update the treads
            treadProps.SetVector(property, new Vector2(Mathf.Abs(offset), 0));
            renderer.SetPropertyBlock(treadProps, treadIndex);

            //increase offset
            var delta = Time.smoothDeltaTime * speed * (reverse ? -1 : 1);
            offset += delta;

            //update the railing
            railingProps.SetVector(property, new Vector2(Mathf.Abs(offset - (delta / 2.0f)), 0));
            renderer.SetPropertyBlock(railingProps, railingIndex);

            if (offset >= threshold || offset < 0)
                offset = reverse ? threshold : 0;
        }
    }
}