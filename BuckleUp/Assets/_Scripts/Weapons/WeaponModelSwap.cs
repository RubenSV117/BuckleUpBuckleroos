using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Swaps parenting between equipped and unequipped weapons for the weapon swap animation
/// Fixes position and rotation of the weapons afterwards
/// 
/// Ruben Sanchez
/// 6/5/18
/// </summary>
public class WeaponModelSwap : MonoBehaviour
{
    public List<Transform> weapons; // weapon models currently on the player

    [SerializeField] private Transform equippedWeaponPivot; // attachment point for equipped weapon
    [SerializeField] private Transform unequippedWeaponPivot; // attachment point for unequipped weapon

    [SerializeField] private Transform rightHand; // right hand bone on the humanoid
    [SerializeField] private Transform leftHand;  // left hand bone on the humanoid

    private Transform equippedModel; // reference of which model is currently equipped

    private void Awake()
    {
        equippedModel = weapons[0];
    }

    // parent unequipped weapon to the right hand to bring it forward in the animation
    public void ParentUnequipped()
    {
        foreach (Transform w in weapons)
        {
            if (w != equippedModel)
                w.SetParent(rightHand);                         
        }
    }

    // parent equipped weapon to the left hand to bring it up and over to the back in the animation
    public void ParentEquipped()
    {
        foreach (Transform w in weapons)
        {
            if (w == equippedModel)
                w.SetParent(leftHand);
        }
    }

    // iterate and change reference to currently equppied weapon
    public void SetEquipped()
    {
        equippedModel = weapons[(weapons.IndexOf(equippedModel) + 1) % weapons.Count];
    }

    // fix the weapon models' local position and rotation
    public void FixWeaponsPosAndRot()
    {
        SetEquipped();

        foreach (Transform w in weapons)
        {            
            w.SetParent(w == equippedModel ? equippedWeaponPivot : unequippedWeaponPivot); // parent to appropriate attachment point

            w.localPosition = Vector3.zero;
            w.localEulerAngles = Vector3.zero;

            //if (w == equippedModel && w.GetComponent<ProjectileWeapon>())
            //    aimIk.solver.transform = w.GetComponent<ProjectileWeapon>().firePoint;
        }
    }
}
