using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays attacks left on equipped weapon for local player
/// 
/// Ruben Sanchez
/// 6/7/18
/// </summary>

public class DisplayAttacksLeft : MonoBehaviour
{
    private Text attackText;
    private WeaponManager weaponManager;

	void Start ()
	{
	    attackText = GetComponent<Text>();
	    weaponManager = GameManager.Instance.LocalPlayer.WeaponManager;
	}
	
	void Update ()
	{
	    if (weaponManager.equippedWeapon is ProjectileWeapon)
	        attackText.text = "" + weaponManager.equippedWeapon.currentImmediateAttacks + "/" +
	                        weaponManager.equippedWeapon.currentSupplementaryAttacks;

	    else
	        attackText.text = "" + weaponManager.equippedWeapon.currentImmediateAttacks;

	}
}
