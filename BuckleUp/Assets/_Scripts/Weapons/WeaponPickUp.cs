using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages weapon pick ups
/// 
/// Ruben Sanchez
/// 6/3/18
/// </summary>
public class WeaponPickUp : MonoBehaviour
{
    [SerializeField] private CanvasEvents canvasEvents;
    [SerializeField] private UnityEvent onPickUp;

    private WeaponManager weaponManger; // WeaponManager of player that picks up this weapon
    private Weapon weaponToPickup;

    private void Awake()
    {
        weaponManger = GetComponentInParent<WeaponManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Entered weapon trigger 
        if (other.transform.root.GetComponent<Weapon>() != null && 
            !weaponManger.weapons.Contains(other.transform.root.GetComponent<Weapon>()))
        {
            weaponToPickup = other.transform.root.GetComponent<Weapon>();

            // subscribe the interacting player's OnInteract event to Equip
            canvasEvents.OnInteract += Equip;

            // activate interact button
            canvasEvents.SetInteractButton(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.root.GetComponent<Weapon>() != null)
        {
            weaponToPickup = null;

            // unsubscribe the interacting player's OnInteract event to Equip
            canvasEvents.OnInteract -= Equip;

            // deactivate interact button
            canvasEvents.SetInteractButton(false);
        }
    }

    public void Equip()
    {
        if(weaponToPickup)
            weaponManger.Equip(weaponToPickup); // equip weapon on the players WeaponManager

        onPickUp.Invoke();
    }
}

