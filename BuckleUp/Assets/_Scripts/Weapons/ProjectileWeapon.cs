﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;

/// <summary>
/// Defines behavior for projectile weapons, can auto or burst fire
/// 
/// Ruben Sanchez
/// 6/3/18
/// </summary>
public class ProjectileWeapon : Weapon
{ 
    [SerializeField] protected float shootSpeed;
    [SerializeField] protected int damage;
    [Tooltip("Total ammo not in the clip")]

    [SerializeField] protected GameObject projectile;
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected Transform firePoint;
    [Tooltip("Minimum Offset")]
    [SerializeField] protected float minRecoil;
    [Tooltip("Maximum Offset")]
    [SerializeField] protected float maxRecoil;


    [Header("Auto Mode")]
    [Tooltip("Shot per second on auto")]
    [SerializeField]
    [Range(1, 20)]
    protected float autoFireRate = 5f;

    [Header("Burst Mode")]
    [SerializeField]
    protected bool burst;

    [Tooltip("Number of shots in one burst")]
    [SerializeField]
    [Range(1, 5)]
    protected int fireBurstCount = 3;

    [Tooltip("Shots per second in a burst")]
    [SerializeField]
    [Range(1, 20)]
    protected float fireRate = 3;

    [Tooltip("Delay in between bursts")]
    [SerializeField]
    [Range(0, 3)]
    protected float burstCooldown = .5f;

    [SerializeField] private UnityEvent onShoot;

    protected bool onCooldown;
    protected Coroutine burstFireCoroutine;

    protected AudioSource audioS;

    private void OnDisable()
    {
        onCooldown = false;
        StopAllCoroutines();
        burstFireCoroutine = null;
    }

    private void Awake()
    {
        currentSupplementaryAttacks = maxSupplementaryAttacks;
        currentImmediateAttacks = maxImmediateAttacks;

        audioS = GetComponent<AudioSource>();
    }

    public override void Attack()
    {
        if (onCooldown)
            return;

        onShoot.Invoke();

        // if automatic, fire single shot and begin cooldown coroutine
        if (!burst)
        {
            onCooldown = true;
            FireSingleShot();
            StartCoroutine(AutoFireCoolDown());
        }

        // if burst, begin the burst fire coroutine
        else
        {
            onCooldown = true;

            if (burstFireCoroutine == null)
                burstFireCoroutine = StartCoroutine(BurstFire());
        }
    }

    public override void OnEquip() { }
    public override void OnUnequip() { }

    private void FireSingleShot()
    {
        // if weapon still has ammo in the clip
        if (currentImmediateAttacks > 0) 
        {
            currentImmediateAttacks--;

            // instantiate projectile
            Projectile proj = Instantiate(projectile, firePoint.position, firePoint.rotation).GetComponent<Projectile>();
            proj.damage = damage;

            // apply recoil
            float offset = Random.Range(minRecoil, maxRecoil) * Random.Range(-1, 2); 

            Vector2 velocity = (Vector2) (offset * firePoint.up) +
                               (Vector2) Vector3.Normalize(firePoint.right * (GameManager.Instance.LocalPlayer.isFacingRight ? 1 : -1)) * shootSpeed;

            // shoot projectile with offset from the recoil
            proj.Shoot(velocity);

            audioS.Play();
            muzzleFlash.Play();

            // if you're out of ammo in the clip but have enough to reload
            if (currentImmediateAttacks == 0 && currentSupplementaryAttacks > 0) 
                Reload();
        }
    }

    public void Reload()
    {
        // if you still have enough ammo for a full clip
        if (currentSupplementaryAttacks > maxImmediateAttacks)
        {
            currentSupplementaryAttacks -= (maxImmediateAttacks - currentImmediateAttacks); // subtract only the ammo missing from the clip
            currentImmediateAttacks = maxImmediateAttacks;
        }
        
        // else reload the rest of the ammo
        else
        {
            currentImmediateAttacks = currentSupplementaryAttacks;
            currentSupplementaryAttacks = 0;
        }
    }

    public void AddAmo(int amount)
    {
        // if amount would fill up your max
        if (currentSupplementaryAttacks + amount >= maxSupplementaryAttacks)
            currentSupplementaryAttacks = maxSupplementaryAttacks;

        // else just add the amount
        else
            currentSupplementaryAttacks += amount;
    }


    public IEnumerator BurstFire()
    {
        // fire burst count with set delay in between
        for (int i = 0; i < fireBurstCount; i++)
        {
            FireSingleShot();
            yield return new WaitForSeconds(1 / fireRate);
        }

        // wait for the next burst to be available
        yield return new WaitForSeconds(burstCooldown);
        onCooldown = false;
        burstFireCoroutine = null;
    }

    public IEnumerator AutoFireCoolDown()
    {
        // wait for the next shot to be available
        yield return new WaitForSeconds(1f / autoFireRate);
        onCooldown = false;
    }
}
