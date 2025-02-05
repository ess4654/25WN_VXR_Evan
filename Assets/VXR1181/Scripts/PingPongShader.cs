using UnityEngine;

[RequireComponent (typeof(MeshRenderer))]
public class PingPongShader : MonoBehaviour
{
    [SerializeField] private string pingPongFloat;
    [SerializeField, Range(-1f, 1f)] private float ping = 1f;
    [SerializeField, Range(-1f, 1f)] private float pong = 0f;
    [SerializeField] private float speed = 1f;

    private MeshRenderer render;
    private MaterialPropertyBlock shader;
    private float target;
    private float current;
    private float t;

    private void Awake()
    {
        shader = new MaterialPropertyBlock();
        render = GetComponent<MeshRenderer>();
        render.GetPropertyBlock(shader);
        target = ping;
        current = pong;
        t = 0;
    }

    private void Update()
    {
        if(shader != null)
        {
            shader.SetFloat(pingPongFloat, Mathf.Lerp(current, target, t));
            render.SetPropertyBlock(shader);
            t += Time.smoothDeltaTime * speed;

            if(t >= 1)
            {
                (current, target) = (target, current);
                t = 0;
            }
        }
    }
}
