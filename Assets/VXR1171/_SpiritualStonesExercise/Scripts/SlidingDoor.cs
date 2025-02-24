using System.Collections;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    #region VARIABLE DECLARATIONS

    [SerializeField] private Transform openDooraPosition;
    [SerializeField] private Transform closeDoorPosition;
    [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private bool open;

    [Tooltip("Slide movement duration in seconds")]
    [SerializeField] private float slideDuration = 5.0f;

    private Coroutine doorSlideCoroutine;

    #endregion

    #region SETUP

    private void Start()
    {
        Close();
    }

    private void OnEnable()
    {
        StoneSocket.OnStonesChanged += HandleStonesChanged;
    }

    private void OnDisable()
    {
        StoneSocket.OnStonesChanged -= HandleStonesChanged;
    }

    private void HandleStonesChanged(int numStones)
    {
        if (numStones == 3)
            Open();
        else
            Close();
    }

    #endregion

    #region METHODS

    private IEnumerator SlideDoor(Vector3 endPosition)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / slideDuration;

            transform.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(percentageComplete));
            yield return null;
        }
        transform.position = endPosition; // Ensure the exact target position is reached
    }

    [ContextMenu("Open")] // This allows running the function from the Editor to test it (dotStack Menu next to Component Name). Only works for functions with no parameters.
    public void Open()
    {
        if (open) return;
        
        StopDoorSlideCoroutine(); // Stop any existing coroutine to avoid conflicts
        doorSlideCoroutine = StartCoroutine(SlideDoor(openDooraPosition.position));
        open = true;
    }

    [ContextMenu("Close")]
    public void Close()
    {
        if (!open) return;

        StopDoorSlideCoroutine();
        doorSlideCoroutine = StartCoroutine(SlideDoor(closeDoorPosition.position));
        open = false;
    }

    private void StopDoorSlideCoroutine()
    {
        if (doorSlideCoroutine != null)
        {
            StopCoroutine(doorSlideCoroutine);
            doorSlideCoroutine = null;
        }
    }

    #endregion
}