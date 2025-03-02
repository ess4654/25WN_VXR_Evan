using UnityEngine;

/// <summary>
///     Animates the treads of the escalator.
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class Escalator : MonoBehaviour
{
    [SerializeField, Min(0)] private int materialIndex = 1;
    [SerializeField, Min(.01f)] private float speed = 1;
    [SerializeField] private bool reverse;

    private MeshRenderer renderer;
    private MaterialPropertyBlock props;
    private float offset;
    private const float threshold = .125f;
    private const string property = "_Offset";

    private void Awake()
    {
        props = new MaterialPropertyBlock();
        renderer = GetComponent<MeshRenderer>();
        renderer.GetPropertyBlock(props, materialIndex);
    }

    private void Update()
    {
        props.SetFloat(property, Mathf.Abs(offset));
        renderer.SetPropertyBlock(props, materialIndex);

        offset += Time.smoothDeltaTime * speed * (reverse ? -1 : 1);
        if (offset >= threshold || offset < 0)
            offset = reverse ? threshold : 0;
    }
}