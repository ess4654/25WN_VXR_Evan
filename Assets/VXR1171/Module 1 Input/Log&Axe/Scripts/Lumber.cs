using UnityEngine;

/// <summary>
///     Controls the splitting of the lumber.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Lumber : MonoBehaviour
{
    [SerializeField] private Rigidbody logOne;
    [SerializeField] private Rigidbody logTwo;

    private Collider collider;
    private const float minSplitSpeed = 5;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Blade") && collision.TryGetComponent(out Blade blade)) //the blade hit this lumber
        {
            if(blade.ControllerDataReader != null)
            {
                TrySplitLog(blade.ControllerDataReader.Velocity.magnitude);
            }
        }
    }

    /// <summary>
    ///     Attempts to split the log with a provided velocity of the axe.
    /// </summary>
    /// <param name="velocity">Velocity the axe hit the log.</param>
    private void TrySplitLog(float velocity)
    {
        if (velocity < minSplitSpeed) return; //did not hit the log with enough force

        EnablePhysics(logOne);
        EnablePhysics(logTwo);
    }

    /// <summary>
    ///     Enables the physics on a specific log.
    /// </summary>
    /// <param name="log">The log/lumber to send flying after the split.</param>
    private void EnablePhysics(Rigidbody log)
    {
        if (log != null)
        {
            log.useGravity = true;
            log.isKinematic = false;
            //log.AddForce();
        }
    }
}