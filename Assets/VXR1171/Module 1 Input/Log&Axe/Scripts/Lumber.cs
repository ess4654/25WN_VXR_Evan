using UnityEngine;

/// <summary>
///     Controls the splitting of the lumber.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Lumber : MonoBehaviour
{
    [SerializeField] private Rigidbody logOne;
    [SerializeField] private Rigidbody logTwo;
    [SerializeField] private float splinterForce = 10;

    private Collider collider;
    private const float minSplitSpeed = 5;
    private const float minStickSpeed = 3;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
            SplitLog();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Blade") && collision.TryGetComponent(out Blade blade)) //the blade hit this lumber
        {
            if(blade.ControllerDataReader != null)
                TrySplitLog(blade);
        }
    }

    /// <summary>
    ///     Attempts to split the log with a provided velocity of the axe.
    /// </summary>
    /// <param name="velocity">Velocity the axe hit the log.</param>
    private void TrySplitLog(Blade blade)
    {
        var velocity = blade.ControllerDataReader.Velocity.magnitude;
        if (velocity >= minSplitSpeed)
        {
            SplitLog();
        }
        else if(velocity >= minStickSpeed) //did not hit the log with enough force
        {
            Debug.Log("Drop Blade");
            blade.Drop();
            blade.DisablePhysics();
        }
    }

    private void SplitLog()
    {
        Debug.Log("Split Log");
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
            log.AddForce(splinterForce * log.transform.right, ForceMode.Impulse);
        }
    }
}