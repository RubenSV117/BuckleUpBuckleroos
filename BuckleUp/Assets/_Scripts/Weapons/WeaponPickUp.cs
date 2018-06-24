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
        GameManager.Instance.CanvasEvents.SetInteractButtonIcon(weapon.SpriteRenderers[0].sprite);
    }

    public void Interact()
    {
        WeaponManager wm = GameManager.Instance.LocalPlayer.WeaponManager;

        if (!wm.weapons.Contains(weapon))
            GameManager.Instance.LocalPlayer.WeaponManager.Equip(weapon);

        InteractableEnd();
    }

    public void InteractableEnd()
    {
        GameManager.Instance.CanvasEvents.SetInteractButton(false);
    }
}
