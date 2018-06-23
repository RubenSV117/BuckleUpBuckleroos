using System.Collections;
using UnityEngine;

/// <summary>
/// Manages behavior for the laser rifle
/// 
/// Ruben Sanchez
/// 6/8/18
/// </summary>

public class LaserWeapon : Weapon
{
    [Tooltip("Time between pressing the trigger and firing")]
    [SerializeField] private float chargeTime;
    [SerializeField] private float range;
    [SerializeField] private LineRenderer laser;
    [SerializeField] private float chargeStartLaserWidth = .2f;
    [SerializeField] private float chargeEndLaserWidth = .5f;
    [SerializeField] Transform firePoint;


    private Coroutine chargeCo;

    private void Awake()
    {
        laser.enabled = false;
        GameManager.Instance.Input.OnAttackEnd += CancelCharge;
    }

    void LateUpdate()
    {
        laser.SetPosition(0, firePoint.position);
    }

    public override void Attack()
    {
        if (chargeCo == null)
            chargeCo = StartCoroutine(Charge());
    }

    public IEnumerator Charge()
    {
        laser.endWidth = chargeStartLaserWidth;
        laser.startWidth = chargeStartLaserWidth;
        laser.enabled = true;

        float timeToShoot = Time.time + chargeTime; // time at which to shoot

        while (Time.time < timeToShoot)
        {

            yield return null;
        }

        chargeCo = null;
        Shoot();

        laser.endWidth = chargeEndLaserWidth;
        laser.startWidth = chargeEndLaserWidth;
        yield return new WaitForSeconds(.5f);
        laser.enabled = false;
    }

    public void Shoot()
    {

    }

    public void CancelCharge()
    {
        if (chargeCo != null)
        {
            StopCoroutine(chargeCo);
            chargeCo = null;
            laser.enabled = false;
        }
    }
}
