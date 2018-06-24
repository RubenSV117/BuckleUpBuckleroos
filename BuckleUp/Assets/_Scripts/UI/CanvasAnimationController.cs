using UnityEngine;

/// <summary>
/// Controller for all UI Animations
/// 
/// Ruben Sanchez
/// 
/// </summary>

public class CanvasAnimationController : MonoBehaviour
{
    [SerializeField] private Animator weaponCycleAnim;
    public bool isOnleft = true;

    public void PlayCycleKnobAnim()
    {
        if(isOnleft)
            weaponCycleAnim.Play("CycleRight");

        else
            weaponCycleAnim.Play("CycleLeft");

        isOnleft = !isOnleft;
    }
}
