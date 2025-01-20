using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Blade : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable grabInteractable;
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
        interactor = e.interactableObject as XRBaseInteractor;
    }

    private void ResetControllerDataReader(SelectExitEventArgs arg0)
    {

    }
}
