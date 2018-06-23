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
    private bool cycleRight = true;


    public void PlayCycleKnobAnim()
    {
        if(cycleRight)
            weaponCycleAnim.Play("CycleRight");

        else
            weaponCycleAnim.Play("CycleLeft");

        cycleRight = !cycleRight;
    }
}
