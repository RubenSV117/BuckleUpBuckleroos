using UnityEngine;

/// <summary>
/// Controls player animations
/// Gets Input info from InputManager
/// 
/// Ruben Sanchez
/// 5/28/19
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMove;


    private InputManager input;
    private WeaponManager weaponManager;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        weaponManager = GetComponent<WeaponManager>();

        input = GameManager.Instance.Input;

        // subscribe event methods to Input events
        input.OnSprintChange += SetSprint;
        input.OnRoll += Roll;
        input.OnAimChange += Aim;
        input.OnWeaponCycle += WeaponCycle;

    }

    public void SetMoving()
    {
        // movement
        anim.SetBool("Run", input.MoveDirection.magnitude != 0);

        //// attacking
        //anim.SetBool("attacking", input.AimDirection.magnitude != 0);
        //TurnToDirectionOfAttack(input.MoveDirection, input.AimDirection);
    }

    public void SetMovementDirection()
    {
        // move direction, do not update if currently rolling
        if (!playerMove.isRolling)
        {
            anim.SetFloat("horizontalMove", input.MoveDirection.x);
            anim.SetFloat("verticalMove", input.MoveDirection.y);
        }
    }

    private void SetSprint(bool isSprinting)
    {
        anim.SetBool("sprint", isSprinting);
    }

    public void Roll()
    {
        anim.Play("Roll");
    }

    public void WeaponCycle()
    {
        if(weaponManager.weapons.Count > 1)
            anim.Play("WeaponCycle", anim.GetLayerIndex("Both Arms"));
    }

    public void Aim(bool isAiming)
    {
        anim.SetBool("aiming", isAiming);
    }

    public void TurnToDirectionOfAttack(Vector3 moveDirection, Vector3 aimDirection)
    {
        //not aiming
        if (aimDirection.magnitude == 0)
            return;

        else
        {
            anim.SetBool("attacking", true);

            // aiming without moving
            if (moveDirection.magnitude == 0)
            {
                anim.SetFloat("horizontalAim", 0);
                anim.SetFloat("verticalAim", 0);
            }

            // moving and aiming
            else
            {
                // aiming in the direction of movement
                if (Vector3.Dot(moveDirection, aimDirection) >= .5f)
                    SetAnimAim(0, 1);

                // aiming in opposite direction of movement
                else if (Vector3.Dot(moveDirection, aimDirection) <= -.5f)
                    SetAnimAim(0, -1);

                // aiming while strafing right
                else if (Vector3.Dot(transform.right, moveDirection) >= .5f)
                    SetAnimAim(1, 0);

                // aiming while strafing left
                else if (Vector3.Dot(transform.right, moveDirection) <= -.5f)
                    SetAnimAim(-1, 0);
            }
        }
    }

    public void SetAnimAim(float x, float y)
    {
        anim.SetFloat("horizontalAim", x);
        anim.SetFloat("verticalAim", y);
    }
}
