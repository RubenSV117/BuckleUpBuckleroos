using UnityEngine;

/// <summary>
/// Moves a transfrom around its parent according to input, used to smoothly offset the look at target for cinemachine
/// 
/// Ruben Sanchez
/// 6/17/18
/// </summary>

public class TransformOffset : MonoBehaviour
{
    [SerializeField] private float damping;

    [Tooltip("Offsets from the parent transform")] [SerializeField]
    private float lookAhead = 4;

    [SerializeField] private Transform transformToMove;

    private InputManager input;
    private Vector2 currentPosition;
    private Vector2 targetPosition;


    private void Awake()
    {
        input = GameManager.Instance.Input;
        targetPosition = transformToMove.position;
    }

    void Update()
    {
        // Move the look at target if moving or aiming, prioritize aiming
        if (input.AimDirection.magnitude != 0)
            targetPosition = (Vector2) transform.position + input.AimDirection * lookAhead;

        else if (input.MoveDirection.magnitude != 0)
            targetPosition = (Vector2) transform.position + input.MoveDirection * lookAhead;

        // lerp between current and target, prioritize aim direction, set appropriate look ahead length
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime / damping);

        transformToMove.position = currentPosition;

    }
}
