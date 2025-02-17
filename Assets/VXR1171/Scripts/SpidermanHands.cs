using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Gestures;

/// <summary>
///     Detects the spiderman hand pose.
/// </summary>
public class SpidermanHands : MonoBehaviour
{
    [SerializeField] private XRHandTrackingEvents m_LeftHandEvents;
    [SerializeField] private XRHandTrackingEvents m_RightHandEvents;
    [SerializeField] private XRHandPose spidermanPose;
    [SerializeField] private UnityEvent onPoseStart;
    [SerializeField] private UnityEvent onPoseEnd;

    private bool wasDetetctedL;
    private bool wasDetetctedR;

    void Start()
    {
        m_LeftHandEvents.jointsUpdated.AddListener(OnJointsUpdatedL);
        m_RightHandEvents.jointsUpdated.AddListener(OnJointsUpdatedR);
    }

    private void OnJointsUpdatedL(XRHandJointsUpdatedEventArgs e)
    {
        if (spidermanPose != null)
        {
            bool detected = spidermanPose.CheckConditions(e);
            if (!wasDetetctedL && detected)
            {
                wasDetetctedL = true;
                OnPoseStart();
            }
            else if (wasDetetctedL && !detected)
            {
                OnPoseEnd();
                wasDetetctedL = false;
            }
        }
    }

    private void OnJointsUpdatedR(XRHandJointsUpdatedEventArgs e)
    {
        if (spidermanPose != null)
        {
            bool detected = spidermanPose.CheckConditions(e);
            if (!wasDetetctedR && detected)
            {
                wasDetetctedR = true;
                OnPoseStart();
            }
            else if (wasDetetctedR && !detected)
            {
                OnPoseEnd();
                wasDetetctedR = false;
            }
        }
    }

    void OnPoseStart()
    {
        onPoseStart?.Invoke();
    }

    void OnPoseEnd()
    {
        onPoseEnd?.Invoke();
    }
}
