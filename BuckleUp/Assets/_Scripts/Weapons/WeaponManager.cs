using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages weapon inventory and cycling
/// Subscribes methods to Input attack and cycle events
/// Sends new weapon info to CanvasEvents to update UI
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

    private InputManager input;
    public Weapon equippedWeapon { get; private set; }

    private CanvasEvents canvasEvents; // used to update the weapons in the UI cycle bar

    private void Awake()
    {
        equippedWeapon = weapons[0];

        foreach (var sr in equippedWeapon.SpriteRenderers)
            sr.sortingOrder = equippedWeaponSortOrder;
    }

    private void Start()
    {
        canvasEvents = GameManager.Instance.CanvasEvents;
        input = Player.localPlayer.input;
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
            equippedWeapon.transform.localScale = Vector2.one;
            equippedWeapon.transform.position = w.transform.position;
            equippedWeapon.transform.eulerAngles = w.transform.eulerAngles;

            // add Weapon script to this list 
            weapons.Add(w); 

            // parent new weapon to equipepd attach point 
            w.transform.SetParent(equippedAttachPoint);
            w.transform.localPosition = Vector3.zero;
            w.transform.localEulerAngles = Vector3.zero;

            w.transform.localScale = new Vector3(w.transform.localScale.x * (GameManager.Instance.LocalPlayer.isFacingRight ? 1 : -1), w.transform.localScale.y, w.transform.localScale.z);

            // render  in front of the player
            foreach (var sr in w.SpriteRenderers)
                sr.sortingOrder = equippedWeaponSortOrder;

            // make new weapon the equipped weapon
            equippedWeapon = w;
            w.OnEquip();

            // update UI cycle bar with the new weapon
            canvasEvents.UpdateWeaponCycleBar(w.SpriteRenderers[0].sprite);
        }

        // else pick up the weapon and move it to unequipped point
        else if (weapons.Count < maxWeapons)
        {
            // add Weapon script to this list 
            weapons.Add(w); 

            //parent new weapon to unequipepd attach point 
            w.transform.SetParent(unequippedAttachPoint);
            w.transform.localPosition = Vector3.zero;
            w.transform.localEulerAngles = Vector3.zero;
            w.transform.localScale =  new Vector3(w.transform.localScale.x * (GameManager.Instance.LocalPlayer.isFacingRight ? 1 : -1), w.transform.localScale.y, w.transform.localScale.z);

            // render behind the player
            foreach (var sr in w.SpriteRenderers)
                sr.sortingOrder = unequippedWeaponSortOrder;

            // activate UI weapon cycle bar and initialize the weapon images
            canvasEvents.InitializeWeaponCycleBar(equippedWeapon.SpriteRenderers[0].sprite, w.SpriteRenderers[0].sprite);
            canvasEvents.SetWeaponCycleBarActive(true);
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
