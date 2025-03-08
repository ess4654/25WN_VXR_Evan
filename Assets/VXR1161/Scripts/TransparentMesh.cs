using UnityEngine;

namespace MegaMall
{
    /// <summary>
    ///     Used to update the transparency of wall & roof tiles.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class TransparentMesh : MonoBehaviour
    {
        [SerializeField, Range(minAlpha, 1f)] private float alpha = 1;

        private MeshRenderer renderer;
        private MaterialPropertyBlock props;
        private const string property = "_Alpha";
        private const float minAlpha = 0.1f;

        private void OnValidate() => UpdateMeshTransparency();

        private void UpdateMeshTransparency()
        {
            if(renderer == null)
                renderer = GetComponent<MeshRenderer>();
            if (props == null)
            {
                props = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(props);
            }

            props.SetFloat(property, alpha);
            renderer.SetPropertyBlock(props);
        }

        /// <summary>
        ///     Sets the alpha value for the mesh.
        /// </summary>
        /// <param name="alpha">Range between [0.1f - 1f]</param>
        public void SetMeshAlpha(float alpha)
        {
            this.alpha = Mathf.Clamp(alpha, minAlpha, 1);
            UpdateMeshTransparency();
        }
    }
}