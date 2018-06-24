using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Holds events for UI canvas
/// 
/// Ruben Sanchez
/// 6/22/18
/// </summary>

public class CanvasEvents : MonoBehaviour
{
    [SerializeField] private GameObject interactButton;
    [SerializeField] private Image interactIcon;

    [SerializeField] private GameObject cycleBar;
    [SerializeField] private Image leftWeapon;
    [SerializeField] private Image rightWeapon;

    public delegate void Interact();
    public event Interact OnInteract;

    private CanvasAnimationController canvasAnim;

    void Awake()
    {
        GameManager.Instance.CanvasEvents = this;
        canvasAnim = GetComponent<CanvasAnimationController>();
    }
 
    public void SetInteractButtonActive(bool activated)
    {
        interactButton.SetActive(activated);
    }

    public void SendInteract()
    {
        if(OnInteract != null)
            OnInteract.Invoke();
    }

    public void SetInteractButtonIcon(Sprite icon)
    {
        interactIcon.sprite = icon;
    }

    // update wepaon images when equipped weapon is dropped for a new weapon 
    public void UpdateWeaponCycleBar(Sprite newWeapon)
    {
        if (canvasAnim.isOnleft)
            leftWeapon.sprite = newWeapon;

        else
            rightWeapon.sprite = newWeapon;
    }

    // initialize weapon images when the player first picks up a second weapon that can be cycled to
    public void InitializeWeaponCycleBar(Sprite left, Sprite right)
    {
        leftWeapon.sprite = left;
        rightWeapon.sprite = right;
    }

    public void SetWeaponCycleBarActive(bool activated)
    {
        cycleBar.SetActive(activated);
    }
}
