using UnityEngine;

/// <summary>
/// Manages local Player instance
/// 
/// Ruben Sanchez
/// 6/7/18
/// </summary>

public class Player : MonoBehaviour
{
    // transforms on the player used for cinemachine vcams
    public Transform AimTransform;
    public Transform FollowTransform;

    void Start()
    {
        GameManager.Instance.LocalPlayer = this;
    }
}
