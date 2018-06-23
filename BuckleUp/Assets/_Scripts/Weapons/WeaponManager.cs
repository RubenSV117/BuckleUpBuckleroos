using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages weapon inventory and cycling
/// 
/// Ruben Sanchez
/// 6/3/18 
/// </summary>
public class WeaponManager : MonoBehaviour
{
    public List<Weapon> weapons;

    [SerializeField] private int maxWeapons;
    [SerializeField] private Transform equippedAttachPoint; // attach point for equipped weapon
    [SerializeField] private Transform unequippedAttachPoint; // attach point for unequipped weapon
    [Tooltip("Renderer Sorting layers")]
    [SerializeField] private int unequippedWeaponSortOrder;
    [SerializeField] private int equippedWeaponSortOrder;

    private WeaponModelSwap modelSwap; // on the animator object, used for animation events during weapon swap
    private InputManager input;
    private Weapon equippedWeapon;

    private void Awake()
    {
        modelSwap = GetComponentInChildren<WeaponModelSwap>();

        equippedWeapon = weapons[0];

        foreach (var sr in equippedWeapon.SpriteRenderers)
            sr.sortingOrder = equippedWeaponSortOrder;
        
        input = GameManager.Instance.Input;

        // subscribe events 
        input.OnAttack += Attack;
        input.OnWeaponCycle += CycleWeapon;
    }

    public void Equip(Weapon w)
    {
        
        // if at capacity, drop currently equipped weapon and equip the new weapon
        if (weapons.Count == maxWeapons)
        {
            // drop currently equipped
            equippedWeapon.OnUnequip();
            weapons.Remove(equippedWeapon);
            equippedWeapon.transform.SetParent(null);

            weapons.Add(w); // add Weapon script to this list 

            // parent new weapon to equipepd attach point 
            w.transform.SetParent(equippedAttachPoint);
            w.transform.localPosition = Vector3.zero;
            w.transform.localEulerAngles = Vector3.zero;

            // render  in front of the player
            foreach (var sr in w.SpriteRenderers)
                sr.sortingOrder = equippedWeaponSortOrder;

            w.OnEquip();
        }

        // else pick up the weapon and move it to unequipped point
        else if (weapons.Count < maxWeapons)
        {
            weapons.Add(w); // add Weapon script to this list 

            //parent new weapon to unequipepd attach point 
            w.transform.SetParent(unequippedAttachPoint);
            w.transform.localPosition = Vector3.zero;
            w.transform.localEulerAngles = Vector3.zero;

            // render behind the player
            foreach (var sr in w.SpriteRenderers)
                sr.sortingOrder = unequippedWeaponSortOrder;
        }

    }

    public void CycleWeapon()
    {
        if (weapons.Count <= 1)
            return;

        // attach equipped weapon to unequippedAttachPoint and reset its transform
        equippedWeapon.transform.SetParent(unequippedAttachPoint);
        equippedWeapon.transform.localPosition = Vector2.zero;
        equippedWeapon.transform.localEulerAngles = Vector3.zero;

        foreach (var sr in equippedWeapon.SpriteRenderers)
            sr.sortingOrder = unequippedWeaponSortOrder;

        equippedWeapon.OnUnequip();
        
        // cycle equipped weapon to the next index
        equippedWeapon = weapons[(weapons.IndexOf(equippedWeapon) + 1) % weapons.Count];

        // attach new equipped weapon to equippedWeaponPoint and reset its transform
        equippedWeapon.transform.SetParent(equippedAttachPoint);
        equippedWeapon.transform.localPosition = Vector2.zero;
        equippedWeapon.transform.localEulerAngles = Vector3.zero;

        // render in front of the player
        foreach (var sr in equippedWeapon.SpriteRenderers)
            sr.sortingOrder = equippedWeaponSortOrder;

        equippedWeapon.OnEquip();
    }

    public void Attack()
    {
        equippedWeapon.Attack();
    }

}
