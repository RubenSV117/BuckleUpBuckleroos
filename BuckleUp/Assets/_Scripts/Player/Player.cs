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
    public WeaponManager WeaponManager;
    public Health Health;

    [HideInInspector]
    public bool isFacingRight;

    void Awake()
    {
        WeaponManager = GetComponent<WeaponManager>();
        Health = GetComponent<Health>();
    }

    void Start()
    {
        GameManager.Instance.LocalPlayer = this;
    }
}
