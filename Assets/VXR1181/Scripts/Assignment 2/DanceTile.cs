using System.Collections;
using UnityEngine;

namespace Assignment2
{
    /// <summary>
    ///     Changes the color of an individual dance tile.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class DanceTile : MonoBehaviour
    {
        private Renderer renderer;
        private MaterialPropertyBlock props;

        private void Start()
        {
            renderer = GetComponent<Renderer>();
            props = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(props);

            StartCoroutine(Loop());
        }

        private IEnumerator Loop()
        {
            props.SetColor("_BaseColor", new Color(Random.value, Random.value, Random.value, 1f));
            renderer.SetPropertyBlock(props);
            yield return new WaitForSeconds(.25f);

            StartCoroutine(Loop());
        }
    }
}