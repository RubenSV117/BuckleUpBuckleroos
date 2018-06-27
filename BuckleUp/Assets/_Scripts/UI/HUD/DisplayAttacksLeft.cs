using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays attacks left on equipped weapon for local player
/// 
/// Ruben Sanchez
/// 6/7/18
/// </summary>

public class DisplayAttacksLeft : Photon.MonoBehaviour
{
    private Text attackText;
    private WeaponManager weaponManager;

	void Start ()
	{
	    attackText = GetComponent<Text>();
	    weaponManager = GetComponent<WeaponManager>();
	}
	
	void Update ()
	{
	    //if (!photonView.isMine)
	    //    return;

	    //if (!attackText)
	    //    return;

	    //if (weaponManager.equippedWeapon is ProjectileWeapon)
	    //    attackText.text = "" + weaponManager.equippedWeapon.currentImmediateAttacks + "/" +
	    //                    weaponManager.equippedWeapon.currentSupplementaryAttacks;

	    //else
	    //    attackText.text = "" + weaponManager.equippedWeapon.currentImmediateAttacks;

	}
}
