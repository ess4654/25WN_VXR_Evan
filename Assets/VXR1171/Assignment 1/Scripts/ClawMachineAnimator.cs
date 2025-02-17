using Shared.Helpers;
using System.Drawing;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ArcadeGame.Views.Machines
{
    /// <summary>
    ///     Controls the animations for the claw machine game.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class ClawMachineAnimator : MonoBehaviour
    {
        public const string Filename = "Claw Machine Animator";

        #region VARIABLE DECLARATIONS

        [Header("Joystick Settings")]
        [SerializeField] private ClawMachineHandle joystick;
        [SerializeField] private float joystickDeadZone = .1f;

        [Header("Rails Settings")]
        [SerializeField] private Transform rails;
        [SerializeField] private float forwardMovementSpeed = 1f;
        [SerializeField] private Vector2 railBoundaries;

        [Header("Block Settings")]
        [SerializeField] private Transform clawBlock;
        [SerializeField] private float horizontalMovementSpeed = 1f;
        [SerializeField] private Vector2 blockBoundaries;
        [SerializeField] private Vector2 dropZone;

        [Header("Claw Settings")]
        [SerializeField] private CharacterJoint claw;
        [SerializeField] private Collider clawCollider;
        [SerializeField] private Vector2 clawExtensionRange;
        [SerializeField] private float clawExtensionTime = 1.5f;
        [SerializeField] private float bladeClosingTime = 1f;
        [SerializeField] private InputActionProperty dropAction;

        [Header("Audio")]
        [SerializeField] private AudioSource movementSounds;
        [SerializeField] private AudioSource clawSounds;
        [SerializeField] private AudioSource uiSounds;
        [SerializeField] private AudioClip dropClawSound;
        [SerializeField] private AudioClip clawClose;
        [SerializeField] private AudioClip clawOpen;

        private bool dropping;
        private bool clawExtensionCompete;
        private Animator controller;

        private const float threshold = 0.01f;
        private const float autoAxis = 0.75f;

        #endregion

        private void Awake()
        {
            controller = GetComponent<Animator>();
        }

        private void Update()
        {
            if (joystick != null)
                ReadAxis(joystick.Axis);
            if (dropAction != null && dropAction.action.ReadValue<float>() == 1)
                AnimateClawDrop();
        }

        #region METHODS

        /// <summary>
        ///     Reads the axis input from the joystick on the claw machine.
        /// </summary>
        /// <param name="axis">Axis of the input.</param>
        private void ReadAxis(in Vector2 axis)
        {
            var clampedAxis = axis;
            clampedAxis.x = Mathf.Abs(axis.x) < joystickDeadZone ? 0 : axis.x;
            clampedAxis.y = Mathf.Abs(axis.y) < joystickDeadZone ? 0 : axis.y;
            bool positionChanged = AnimateRails(in clampedAxis);

            if (movementSounds)
                movementSounds.mute = dropping || clampedAxis == Vector2.zero || !positionChanged;
        }

        /// <summary>
        ///     Animates the movement of the claw machine rails and claw block.
        /// </summary>
        /// <param name="axis">Axis of the input.</param>
        private bool AnimateRails(in Vector2 axis)
        {
            if(dropping) return false;

            Vector3 railStartPosition = rails.localPosition;
            Vector3 clawStartPosition = clawBlock.localPosition;

            //move railing
            var railPosition = rails.localPosition + (axis.y * forwardMovementSpeed * Time.smoothDeltaTime * rails.forward);
            railPosition.z = Mathf.Clamp(railPosition.z, railBoundaries.x, railBoundaries.y);
            rails.localPosition = railPosition;

            //move claw block
            var blockPosition = clawBlock.localPosition + (axis.x * horizontalMovementSpeed * Time.smoothDeltaTime * clawBlock.right);
            blockPosition.x = Mathf.Clamp(blockPosition.x, blockBoundaries.x, blockBoundaries.y);
            clawBlock.localPosition = blockPosition;

            return railStartPosition != rails.localPosition || clawStartPosition != clawBlock.localPosition;
        }

        /// <summary>
        ///     Animates the dropping of the claw.
        /// </summary>
        /// <returns>Completed dropping animation</returns>
        public async Task AnimateClawDrop()
        {
            if (dropping) return;

            dropping = true;
            if (movementSounds)
                movementSounds.mute = true;

            ExtendAndRetractClaw(); //start the claw extension and retraction method asynchronously

            await Timer.WaitForSeconds(1f); //wait 1 second to animate the button back up
            if (this == null) return;

            await Timer.WaitUntil(() => this == null || clawExtensionCompete);

            await AnimateClawBlades(true);

            //await AnimatePrizeDrop();
            //await AnimateClawReturn();

            dropping = false;
        }

        /// <summary>
        ///     Controls the animation of the claw extension downwards, then closing the blades, and retracting back up.
        /// </summary>
        /// <returns>Completed claw extension method.</returns>
        private async Task ExtendAndRetractClaw()
        {
            if (uiSounds) //play audio in 2D space
                uiSounds.PlayOneShot(dropClawSound);

            clawExtensionCompete = false;
            var startY = clawExtensionRange.x;
            var endY = clawExtensionRange.y;

            if (clawCollider)
                clawCollider.enabled = true;

            claw.GetComponent<Rigidbody>().AddForce(.01f * Vector3.down, ForceMode.Impulse);

            //animate claw dropping down
            LeanTween
                .value(startY, endY, clawExtensionTime)
                .setOnUpdate(UpdateClawAnchor)
                .setEaseLinear();

            await Timer.WaitForSeconds(clawExtensionTime);
            if (this == null) return;

            //animate the claw blades closing
            await AnimateClawBlades(false);
            if (this == null) return;

            if (clawCollider)
                clawCollider.enabled = false;

            //animate claw pulling up
            LeanTween
                .value(endY, startY, clawExtensionTime)
                .setOnUpdate(UpdateClawAnchor)
                .setEaseLinear();

            await Timer.WaitForSeconds(clawExtensionTime);
            if (this == null) return;

            clawExtensionCompete = true;
        }

        /// <summary>
        ///     Updates the value of the claw anchor.
        /// </summary>
        /// <param name="yValue">Y Value of the claw anchor point.</param>
        private void UpdateClawAnchor(float yValue)
        {
            if (claw != null)
                claw.anchor = yValue * Vector3.up;
        }

        /// <summary>
        ///     Animates the opening and closing of the claw blades.
        /// </summary>
        /// <param name="open">Whether the blades are to be opened or not.</param>
        /// <returns>Completed animation routine</returns>
        private async Task AnimateClawBlades(bool open)
        {
            //animate the claw blades opening or closing
            var start = open ? 1 : 0;
            var end = open ? 0 : 1;
            LeanTween
                .value(start, end, bladeClosingTime)
                .setOnUpdate((float v) => controller.SetFloat("Claw Blade Rotation", v))
                .setEaseLinear();

            if (clawSounds) //play audio in 3D space
                clawSounds.PlayOneShot(open ? clawOpen : clawClose);

            await Timer.WaitForSeconds(bladeClosingTime / 2f);
            if (this == null) return;

            await Timer.WaitForSeconds(bladeClosingTime / 2f);
        }

        /// <summary>
        ///     Animates the claw moving to the drop zone of the machine.
        /// </summary>
        /// <returns>Completed drop animation</returns>
        public async Task AnimatePrizeDrop()
        {
            await AnimateTowards(dropZone);
            if (this == null) return;

            await Timer.WaitForSeconds(.5f);
            if (this == null) return;

            //animate the claw blades opening
            await AnimateClawBlades(true);
            await Timer.WaitForSeconds(1f);
        }

        /// <summary>
        ///     Animates the claw returning to the center point of the machine.
        /// </summary>
        /// <returns>Completed return animation</returns>
        public Task AnimateClawReturn() => AnimateTowards(Vector2.zero);

        /// <summary>
        ///     Animates the movement mechanism towards the desired position.
        /// </summary>
        /// <remarks>
        ///     x = block position
        ///     y = rails position
        /// </remarks>
        /// <param name="position">Position of the movement mechanism to move towards.</param>
        /// <returns>Completed movement animation</returns>
        private async Task AnimateTowards(Vector2 position)
        {
            float xInput;
            float yInput;

            if (movementSounds)
                movementSounds.mute = false;

            while (this && (Mathf.Abs(clawBlock.localPosition.x - position.x) > threshold) || (Mathf.Abs(rails.localPosition.z - position.y) > threshold))
            {
                //Log($"Moving Towards: {position}");
                //Log($"Distance Remaining: {new Vector2(Mathf.Abs(clawBlock.localPosition.x - position.x), Mathf.Abs(rails.localPosition.z - position.y)).magnitude}");

                xInput = Mathf.Abs(clawBlock.localPosition.x - position.x) <= threshold ? 0 : clawBlock.localPosition.x < position.x ? -autoAxis : autoAxis;
                yInput = Mathf.Abs(rails.localPosition.z - position.y) <= threshold ? 0 : rails.localPosition.z < position.y ? -autoAxis : autoAxis;
                AnimateRails(new Vector2(xInput, yInput));
                await Timer.WaitForFrame();
            }

            if (movementSounds)
                movementSounds.mute = true;

            //Log($"Finished Moving Claw");
        }

        #endregion
    }
}