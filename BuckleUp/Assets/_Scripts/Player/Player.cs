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
    public GameObject Camera;
    public InputManager input;

    [HideInInspector]
    public bool isFacingRight;

    public static Player localPlayer;

    void Awake()
    {
        if (localPlayer == null) 
            localPlayer = this;

        WeaponManager = GetComponent<WeaponManager>();
        Health = GetComponent<Health>();
        input = GetComponent<InputManager>();

        Camera.SetActive(true);
    }
}
