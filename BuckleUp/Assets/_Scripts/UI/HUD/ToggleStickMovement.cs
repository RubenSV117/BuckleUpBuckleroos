using UnityEngine;

/// <summary>
/// Rotates a transform around a parent according to input direction
/// 
/// Ruben Sanchez
/// 6/21/18
/// </summary>

public class ToggleStickMovement : MonoBehaviour 
{
    [SerializeField] private float offsetDistance = 35f;
    [SerializeField] private RectTransform moveMidPoint;
    [SerializeField] private RectTransform aimMidPoint;
    [SerializeField] private float damping;

    private InputManager input;

    // current position of the movable centers of the sticks
    private Vector2 currentMovePoint; 
    private Vector2 currentAimPoint;

    private Vector2 originalMovePosition;
    private Vector2 originalAimPosition;

    private void Awake()
    {
        originalMovePosition = moveMidPoint.localPosition;
        originalAimPosition = aimMidPoint.localPosition;
    }

    void Start()
    {
        input = Player.localPlayer.input;
    }

    void Update()
    {
        // lerp to the direction being held

        // move stick
        currentMovePoint = Vector2.Lerp(currentMovePoint,
            originalMovePosition + offsetDistance * input.MoveDirection,
            Time.deltaTime / damping);

        moveMidPoint.localPosition = currentMovePoint;

        // aim stick
        currentAimPoint = Vector2.Lerp(currentAimPoint,
            originalAimPosition + offsetDistance * input.AimDirection,
            Time.deltaTime / damping);

        aimMidPoint.localPosition = currentAimPoint;
    }
}
