using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Base class for all weapons
/// 
/// Ruben Sanchez
/// 6/3/18
/// </summary>

public abstract class Weapon : MonoBehaviour
{
    public List<SpriteRenderer> SpriteRenderers;
    public int maxImmediateAttacks; // can be clip ammo or melee strikes
    public int maxSupplementaryAttacks; // reload ammo
    public int currentImmediateAttacks { get; protected set; }
    public int currentSupplementaryAttacks { get; protected set; }

    public abstract void Attack();
    public abstract void OnEquip();
    public abstract void OnUnequip();

}
