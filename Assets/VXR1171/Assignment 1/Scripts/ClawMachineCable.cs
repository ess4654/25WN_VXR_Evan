using UnityEngine;

namespace ArcadeGame.Views.Machines
{
    /// <summary>
    ///     Controls the rendering of the cable for the claw machine.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class ClawMachineCable : MonoBehaviour
    {
        #region VARIABLE DECLARATIONS

        [SerializeField] private float cableThickness = 1f;
        [SerializeField] private Transform blockAnchor;
        [SerializeField] private Vector3 offset;

        private LineRenderer cableRenderer;

        #endregion

        #region SETUP

        private void Awake() =>
            cableRenderer = GetComponent<LineRenderer>();

        private void OnValidate()
        {
            if(cableRenderer == null)
                cableRenderer = GetComponent<LineRenderer>();
            
            cableRenderer.startWidth = cableThickness;
            if(blockAnchor)
                cableRenderer.SetPosition(1, blockAnchor.position + offset);
        }

        #endregion

        /// <summary>
        ///     Update the render for the cable of the claw machine as it moves/hovers above the plushies.
        /// </summary>
        private void Update()
        {
            var position1 = transform.position;
            var position2 = blockAnchor.position + offset;
            cableRenderer.SetPositions(new Vector3[] { position1, position2 });
        }
    }
}