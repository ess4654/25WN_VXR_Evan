using UnityEngine;

namespace Assignment2
{
    /// <summary>
    ///     Manages the player for the dance scene.
    /// </summary>
    [RequireComponent (typeof (CharacterController))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform meshPivot;

        private CharacterController controller;

        private void Awake()
        {
            controller = GetComponent<CharacterController> ();
        }

        private void Update()
        {
            int xAxis = 0;
            int zAxis = 0;

            if(Input.GetKey(KeyCode.A))
                xAxis--;
            if (Input.GetKey(KeyCode.D))
                xAxis++;

            if (Input.GetKey(KeyCode.S))
                zAxis--;
            if (Input.GetKey(KeyCode.W))
                zAxis++;

            controller.Move(moveSpeed * Time.smoothDeltaTime * new Vector3 (xAxis, 0, zAxis));
        }

        #region COLLISIONS

        private void OnTriggerEnter(Collider collision)
        {
            if(collision.TryGetComponent(out DanceFloor danceFloor) && animator)
            {
                animator.SetInteger("Dance Animation", danceFloor.AnimationClip);
                animator.SetBool("Dancing", true);
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.TryGetComponent(out DanceFloor _) && animator)
            {
                animator.SetInteger("Dance Animation", 0);
                animator.SetBool("Dancing", false);
            }
        }

        #endregion
    }
}