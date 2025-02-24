using System;
using UnityEngine;

public class StoneSocket : MonoBehaviour
{
    public static event Action<int> OnStonesChanged;

    [Tooltip("Drag one of the three spiritual stones in here")] //This allows us to display some text when hovering over the variable name in the editor.
    [SerializeField] private GameObject stoneReference;
    [SerializeField] private SlidingDoor door;
    [SerializeField] private AudioSource audioSource;
    
    private static int NumberActiveStones;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.gameObject.name} gameobject entered the trigger");
        if(stoneReference.Equals(other.gameObject))
        {
            NumberActiveStones++;
            OnStonesChanged?.Invoke(NumberActiveStones);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"{other.gameObject.name} gameobject exited the trigger");
        if (stoneReference.Equals(other.gameObject))
        {
            NumberActiveStones--;
            OnStonesChanged?.Invoke(NumberActiveStones);
        }
    }
}