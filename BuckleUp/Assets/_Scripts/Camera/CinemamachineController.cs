using Cinemachine;
using UnityEngine;

/// <summary>
/// Enables Cinemachine cams
/// Subscribes methods to InputManager's events
/// 
/// Ruben Sanchez
/// 5/28/18
/// </summary>


public class CinemamachineController : MonoBehaviour
{
    [SerializeField] private GameObject[] vCams;
    [SerializeField] private GameObject defaultCam;
    [SerializeField] private GameObject aimCam;
    [SerializeField] private GameObject sprintCam;

    private InputManager input;

    private void Awake()
    {
        GameManager.Instance.OnLocalPlayerJoined += TargetLocalPlayer;

        input = GameManager.Instance.Input;

        // subscribe all camera enabling methods to Input manager events
        input.OnAimChange += AimCamChange;
        input.OnSprintChange += SprintCamChange;
    }

    void Start ()
    {
        ActivateCam(defaultCam);
	}

    // Activate target camera, deactive the rest
    public void ActivateCam(GameObject targetCam)
    {
        targetCam.SetActive(true);

        foreach (var cam in vCams)
        {
            if (cam != targetCam)
                cam.SetActive(false);
        }
    }

    public void ActivateDefaultCam()
    {
        ActivateCam(defaultCam);
    }

    public void AimCamChange(bool isAiming)
    {
       ActivateCam(isAiming ? aimCam : defaultCam);
    }

    public void SprintCamChange(bool isSprinting)
    { 
        if(!input.IsAiming) // from sprinting to default
            ActivateCam(isSprinting ? sprintCam : defaultCam);

        else // from sprinting to aiming
            ActivateCam(aimCam);
    }

    public void TargetLocalPlayer(Player p)
    {
        foreach (var cam in vCams)
        {
            cam.GetComponent<CinemachineVirtualCamera>().Follow = p.FollowTransform;
            cam.GetComponent<CinemachineVirtualCamera>().m_LookAt = p.AimTransform;
        }
    }
}
