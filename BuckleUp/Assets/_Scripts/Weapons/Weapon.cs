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
    public abstract void Attack();
}
