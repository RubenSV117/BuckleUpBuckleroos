using Cinemachine;
using UnityEngine;

/// <summary>
/// Adjust current Vcam look at height to match aiming Input
/// 
/// Ruben Sanchez
/// 5/28/18
/// </summary>


public class VerticalAim : MonoBehaviour
{
    public CinemachineVirtualCamera activeCam;
    [Tooltip("The 'Look At' Transform for VCams")]
    public Transform targetTransform;
    [Tooltip("The 'Follow' Transform for VCams")]
    public Transform followTransform;

    [SerializeField] private float maxAimHeight;
    [SerializeField] private float minAimHeight;

    [SerializeField] private float maxFollowHeight;
    [SerializeField] private float minFollowHeight;

    private InputManager input;
    private float verticalValue;
    private float aimToFollowDistanceRatio; // distance ratio between the follow and aim transforms

    private void Awake()
    {
        aimToFollowDistanceRatio = (maxFollowHeight - minFollowHeight) / (maxAimHeight - minAimHeight);
        //input = GameManager.Instance.Input;
    }

    public void VerticalTurn()
    {
        // adjust target transform vertically based on Input
        //targetTransform.localPosition += targetTransform.up * input.AimDirection.y * input.AimSensitivity.y;
        //followTransform.localPosition -= targetTransform.up * input.AimDirection.y * (aimToFollowDistanceRatio) * input.AimSensitivity.y;

        // readjust target transform if out of bounds
        if (targetTransform.localPosition.y > maxAimHeight)
            targetTransform.localPosition = new Vector3(targetTransform.localPosition.x, maxAimHeight, targetTransform.localPosition.z);

        else if (targetTransform.localPosition.y < minAimHeight)
            targetTransform.localPosition = new Vector3(targetTransform.localPosition.x, minAimHeight, targetTransform.localPosition.z);

        // readjust follow transform if out of bounds
        if (followTransform.localPosition.y > maxFollowHeight)
            followTransform.localPosition = new Vector3(followTransform.localPosition.x, maxFollowHeight, followTransform.localPosition.z);

        else if (followTransform.localPosition.y < minFollowHeight)
            followTransform.localPosition = new Vector3(followTransform.localPosition.x, minFollowHeight, followTransform.localPosition.z);
    }
}
