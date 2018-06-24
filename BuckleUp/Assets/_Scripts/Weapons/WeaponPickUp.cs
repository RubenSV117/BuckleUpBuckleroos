using UnityEngine;

/// <summary>
/// Weapon pickup interaction
/// 
/// Ruben Sanchez
/// 
/// </summary>

public class WeaponPickUp : MonoBehaviour, IInteractable
{
    private Weapon weapon;

    void Awake()
    {
        weapon = GetComponentInParent<Weapon>();
    }

    public void InteractableBegin()
    {
        // set interact button icon to this weapon's main sprite
        GameManager.Instance.CanvasEvents.SetInteractButtonIcon(weapon.SpriteRenderers[0].sprite);
    }

    public void Interact()
    {
        // equip this weapon to the player
        WeaponManager wm = GameManager.Instance.LocalPlayer.WeaponManager;

        if (!wm.weapons.Contains(weapon))
            GameManager.Instance.LocalPlayer.WeaponManager.Equip(weapon);

        InteractableEnd();
    }

    public void InteractableEnd()
    {
        // disable interact button
        GameManager.Instance.CanvasEvents.SetInteractButtonActive(false);
    }
}
