using UnityEngine;

public class CameraPivot : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed);
    }
}