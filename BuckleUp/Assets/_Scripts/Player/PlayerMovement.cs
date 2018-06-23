using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls player horizontal movement
/// 
/// Ruben Sanchez
/// 5/27/28
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveSpeedWhileFiring;
    [SerializeField] private float sprintSpeedMultiplier;

    [Tooltip("Added Speed")]
    [SerializeField] private float rollSpeedBoost;
    [Tooltip("In Seconds")]
    [SerializeField] private float rollLength;
    [SerializeField] private Transform alignedArm;

    [SerializeField] private UnityEvent OnRollBegin;
    [SerializeField] private UnityEvent OnRollEnd;

    [SerializeField] private UnityEvent OnSprintBegin;
    [SerializeField] private UnityEvent OnSprintEnd;
     
    public bool isSprinting;
    public bool isRolling { get; private set; }
    private bool canControlMove = true;
    private Vector3 originalScale;
    private InputManager input;
    private Rigidbody2D rigidB;
    private Coroutine rollCoroutine;
    private Transform mTransform;

    private void Awake()
    {
        rigidB = GetComponent<Rigidbody2D>();

        input = GameManager.Instance.Input;

        // subscribe movement methods to Input events
        input.OnSprintChange += SetSprint;
        input.OnRoll += Roll;
        input.OnAttack += CancelSprint;

        mTransform = transform;
        originalScale = mTransform.localScale;
    }

    public void Move()
    {
        if (canControlMove) // control deactivated during roll
        {
            float currentSpeed = input.AimDirection.magnitude == 0 ? moveSpeed : moveSpeedWhileFiring; // set speed according to firing state
            Vector3 moveVelocity = input.MoveDirection * (isSprinting ? currentSpeed * sprintSpeedMultiplier : currentSpeed); // apply speed boost if sprinting
            rigidB.velocity = new Vector2(moveVelocity.x, moveVelocity.y);
        }
    }

    public void RotateCharacter()
    {
        if (input.AimDirection.magnitude != 0)
        {
            mTransform.localScale = new Vector3(input.AimDirection.x > 0 ? originalScale.x : -originalScale.x, originalScale.y, originalScale.z);

            alignedArm.right = input.AimDirection * transform.localScale.x;
        }
   
        else if (input.MoveDirection.magnitude != 0)
        {
            mTransform.localScale = new Vector3(input.MoveDirection.x > 0 ? originalScale.x : -originalScale.x, originalScale.y, originalScale.z);

            alignedArm.right = input.MoveDirection * transform.localScale.x;
        }
      
    }


    public void SetSprint(bool isSprinting)
    {
        if(isSprinting)
            OnSprintBegin.Invoke();

        else
            OnSprintEnd.Invoke();

        this.isSprinting = isSprinting;
    }

    public void CancelSprint()
    {
        isSprinting = false;
    }

    public void Roll()
    {
        if (rollCoroutine == null)
            rollCoroutine = StartCoroutine(RollCo());
    }

    public IEnumerator RollCo()
    {
        isRolling = true;
        OnRollBegin.Invoke();
        canControlMove = false; // deactivate control
        rigidB.velocity += (Vector2)Vector3.Normalize(rigidB.velocity) * rollSpeedBoost; // apply speed boost in direction of roll

        yield return new WaitForSeconds(rollLength);

        canControlMove = true;
        rollCoroutine = null;
        OnRollEnd.Invoke();
        isRolling = false;
    }

}
