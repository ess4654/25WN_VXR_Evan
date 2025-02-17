using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
///     Controls the interaction with the blade of the axe.
/// </summary>
[RequireComponent(typeof(XRGrabInteractable))]
public class Blade : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable grabInteractable;
    
    public ControllerDataReader ControllerDataReader => controllerDataReader;
    [SerializeField] private ControllerDataReader controllerDataReader;
    
    [SerializeField] private XRBaseInteractor interactor;

    #region SETUP

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        if (grabInteractable == null)
            return;

        grabInteractable.selectEntered.AddListener(OnSelectEnter);
        grabInteractable.selectExited.AddListener(ResetControllerDataReader);
    }

    private void OnDisable()
    {
        if (grabInteractable == null)
            return;

        grabInteractable.selectEntered.RemoveListener(OnSelectEnter);
        grabInteractable.selectExited.RemoveListener(ResetControllerDataReader);
    }

    #endregion

    private void OnSelectEnter(SelectEnterEventArgs e)
    {
        //Set the interactor that is grabbing the axe
        interactor = e.interactableObject as XRBaseInteractor;
        Debug.Log(e.interactorObject);

        //Set the controller data reader
        controllerDataReader = interactor.gameObject.GetComponentInParent<ControllerDataReader>();
    }

    private void ResetControllerDataReader(SelectExitEventArgs e)
    {
        //Reset the reader
        controllerDataReader = null;
    }

    /// <summary>
    ///     Drops the blade/axe.
    /// </summary>
    public void Drop()
    {
        interactor.interactionManager
            .CancelInteractableSelection(grabInteractable as IXRSelectInteractable);
    }

    /// <summary>
    ///     Enables physics on the blade.
    /// </summary>
    public void EnablePhysics()
    {
        if (TryGetComponent(out Rigidbody axe))
            axe.constraints = RigidbodyConstraints.None;
    }

    /// <summary>
    ///     Disables the physics on the blade.
    /// </summary>
    public void DisablePhysics()
    {
        if (TryGetComponent(out Rigidbody axe))
            axe.constraints = RigidbodyConstraints.FreezeAll;
    }
}