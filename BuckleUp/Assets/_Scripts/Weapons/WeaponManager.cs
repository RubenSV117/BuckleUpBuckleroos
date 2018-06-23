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
        // add weapon to the list if not full and disable it
        if (weapons.Count < maxWeapons)
        {
            modelSwap.weapons.Add(w.transform); // add to the list for WeaponModelSwap on the animator object
            weapons.Add(w); // add Weapon script to this list 

            //parent new weapon to unequipepd attach point 
            w.transform.SetParent(unequippedAttachPoint);
            w.transform.localPosition = Vector3.zero;
            w.transform.localEulerAngles = Vector3.zero;
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
        

        // cycle equipped weapon to the next index
        equippedWeapon = weapons[(weapons.IndexOf(equippedWeapon) + 1) % weapons.Count];

        // attach new equipped weapon to equippedWeaponPoint and reset its transform
        equippedWeapon.transform.SetParent(equippedAttachPoint);
        equippedWeapon.transform.localPosition = Vector2.zero;
        equippedWeapon.transform.localEulerAngles = Vector3.zero;

        foreach (var sr in equippedWeapon.SpriteRenderers)
            sr.sortingOrder = equippedWeaponSortOrder;
        
    }

    public void Attack()
    {
        equippedWeapon.Attack();
    }

}
