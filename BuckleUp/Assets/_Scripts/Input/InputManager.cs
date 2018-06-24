using UnityEditor;
using UnityEngine;

/// <summary>
/// Manages controller and touch Input
/// 
/// Ruben Sanchez
/// 5/16/18
/// </summary>

public class InputManager : MonoBehaviour
{
    public Vector2 AimSensitivity = new Vector2(6, .6f);
    public Vector2 AimDamping = new Vector2(.05f, .1f);

    public bool IsAiming;

    [HideInInspector] public Vector2 MoveDirection;
    [HideInInspector] public Vector2 AimDirection;

    public delegate void Attack();
    public event Attack OnAttack;
    public event Attack OnAttackEnd;

    public delegate void Roll();
    public event Roll OnRoll;

    public delegate void SlowMo();
    public event SlowMo OnSlowMo;

    public delegate void Interact();
    public event Interact OnInteract;

    public delegate void Aim(bool isAiming);
    public event Aim OnAimChange;

    public delegate void WeaponCycle();
    public event WeaponCycle OnWeaponCycle;

    public delegate void Sprint(bool isSprinting);
    public event Sprint OnSprintChange;

    [Tooltip("Amount of normal sensitivity to use when zoomed aiming")]
    [SerializeField] private float aimSensitivityMultiplier = .2f;

    private bool isMoving;
    private bool isAttacking;

    private Vector2 initialMoveTouchPosition;
    private Vector2 initialShootTouchPosition;

    private float minDeltaThreshold = .3f;

    void Update()
    {
        // Touch Input
#if UNITY_ANDROID || UNITY_IOS
        {
            // Manage current touches
            foreach (Touch touch in Input.touches)
            {
                // initial left side touch for movement
                if (touch.phase == TouchPhase.Began && 
                    Camera.main.ScreenToViewportPoint(touch.position).x < .25f &&
                    Camera.main.ScreenToViewportPoint(touch.position).y < .35f)
                {
                    initialMoveTouchPosition = touch.position;
                    isMoving = true;
                }

                if (isMoving)
                {
                    // movement touch dragged passed threshold
                    if ((touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) &&
                        Camera.main.ScreenToViewportPoint(touch.position).x < .5f)
                    {
                        Vector2 delta = touch.position - initialMoveTouchPosition;

                        MoveDirection = delta.magnitude > minDeltaThreshold
                            ? Vector3.Normalize(delta)
                            : Vector3.zero;
                    }

                    // release of move touch
                    else if (touch.phase == TouchPhase.Ended &&
                             Camera.main.ScreenToViewportPoint(touch.position).x < .5f)
                    {
                        isMoving = false;
                        MoveDirection = Vector3.zero;
                    }
                        
                }
              
                // initial right side touch for aiming
                if (touch.phase == TouchPhase.Began && 
                         Camera.main.ScreenToViewportPoint(touch.position).x > .8f &&
                         Camera.main.ScreenToViewportPoint(touch.position).y < .35f)
                {
                    initialShootTouchPosition = touch.position;
                    isAttacking = true;
                    OnAttack.Invoke();
                }

                if (isAttacking)
                {
                    // aim touch dragged passed threshold
                    if ((touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) && Camera.main.ScreenToViewportPoint(touch.position).x > .5f)
                    {
                        Vector2 delta = touch.position - initialShootTouchPosition;

                        AimDirection = delta.magnitude > minDeltaThreshold
                            ? Vector3.Normalize(delta)
                            : Vector3.zero;

                        OnAttack.Invoke();

                    }

                    // release of aim touch
                    else if (touch.phase == TouchPhase.Ended &&
                             Camera.main.ScreenToViewportPoint(touch.position).x > .5f)
                    {
                        isAttacking = false;
                        AimDirection = Vector3.zero;
                    }
                }
            
            }

        }
        // Contoller/Keyboard Input
#else
        {
        MoveDirection = new Vector3(Input.GetAxis("HorizontalMove"), 0, Input.GetAxis("VerticalMove"));
        AimDirection = new Vector3(Input.GetAxis("HorizontalAim"), 0, Input.GetAxis("VerticalAim"));

        //attack
        if (Input.GetAxis("Attack") != 0 && OnAttack != null)
        {
            if (!isAttacking)
                isAttacking = true;

            OnAttack.Invoke();
            OnSprintChange.Invoke(false); // cancel sprint on attack
        }

        // attack end
        if (Input.GetAxis("Attack") == 0 && OnAttackEnd != null && isAttacking)
        {
            isAttacking = false;

            OnAttackEnd.Invoke();
        }


        //aim begin
        if (Input.GetAxis("Aim") != 0 && OnAimChange != null && !IsAiming)
        {
            IsAiming = true;
            OnAimChange.Invoke(true);
            AimSensitivity *= aimSensitivityMultiplier;
        }

        //aim end
        if (Input.GetAxis("Aim") == 0 && OnAimChange != null && IsAiming)
        {
            IsAiming = false;
            OnAimChange.Invoke(false);
            AimSensitivity /= aimSensitivityMultiplier;
        }

        //roll
        if (Input.GetButtonDown("Roll") && OnRoll != null)
            OnRoll.Invoke();

        //slowMo
        if (Input.GetButtonDown("SlowMo") && OnSlowMo != null)
            OnSlowMo.Invoke();

        //cycle weapon
        if (Input.GetButtonDown("CycleWeapon") && OnWeaponCycle != null)
            OnWeaponCycle.Invoke();

        //sprint begin
        if (Input.GetButtonDown("Sprint") && OnSprintChange != null)
            OnSprintChange.Invoke(true);
        
        //sprint end
        if (Input.GetButtonUp("Sprint") && OnSprintChange != null)
            OnSprintChange.Invoke(false);

        //interact
        if (Input.GetButtonDown("Interact") && OnInteract != null)
            OnInteract.Invoke();
    }
    
#endif
    }
}
