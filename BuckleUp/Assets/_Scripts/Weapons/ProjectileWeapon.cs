using UnityEngine;
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
    [SerializeField] private float shootSpeed;
    [SerializeField] private int damage;
    [Tooltip("Total ammo not in the clip")]
    [SerializeField] private int maxReloadAmmo;
    [SerializeField] private int maxClipAmmo;
    [SerializeField] private GameObject projectile;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform firePoint;
    [Tooltip("Minimum Offset")]
    [SerializeField] private float minRecoil;
    [Tooltip("Maximum Offset")]
    [SerializeField] private float maxRecoil;


    [Header("Auto Mode")]
    [Tooltip("Shot per second on auto")]
    [SerializeField]
    [Range(1, 20)]
    private float autoFireRate = 5f;

    [Header("Burst Mode")]
    [SerializeField]
    private bool burst;

    [Tooltip("Number of shots in one burst")]
    [SerializeField]
    [Range(1, 5)]
    private int fireBurstCount = 3;

    [Tooltip("Shots per second in a burst")]
    [SerializeField]
    [Range(1, 20)]
    private float fireRate = 3;

    [Tooltip("Delay in between bursts")]
    [SerializeField]
    [Range(0, 3)]
    private float burstCooldown = .5f;

    [SerializeField] private UnityEvent onShoot;

    private bool onCooldown;
    private Coroutine burstFireCoroutine;

    private float currentReloadAmmo;
    private float currentClipAmmo;

    private AudioSource audioS;

    private void OnDisable()
    {
        onCooldown = false;
        StopAllCoroutines();
        burstFireCoroutine = null;
    }

    private void Awake()
    {
        currentReloadAmmo = maxReloadAmmo;
        currentClipAmmo = maxClipAmmo;

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

    private void FireSingleShot()
    {
        // if weapon still has ammo in the clip
        if (currentClipAmmo > 0) 
        {
            muzzleFlash.Play();

            currentClipAmmo--;

            // instantiate projectile
            Projectile proj = Instantiate(projectile, firePoint.position, firePoint.rotation).GetComponent<Projectile>();

            // apply recoil
            float offset = Random.Range(minRecoil, maxRecoil) * Random.Range(-1, 2); 

            Vector2 velocity = (Vector2) (offset * firePoint.up) +
                               (Vector2) Vector3.Normalize(firePoint.right * transform.root.localScale.x) * shootSpeed;

            // shoot projectile with offset from the recoil
            proj.Shoot(velocity);

            audioS.Play();
            muzzleFlash.Play();

            // if you're out of ammo in the clip but have enough to reload
            if (currentClipAmmo == 0 && currentReloadAmmo > 0) 
                Reload();
        }
    }

    public void Reload()
    {
        // if you still have enough ammo for a full clip
        if (currentReloadAmmo > maxClipAmmo)
        {
            currentReloadAmmo -= (maxClipAmmo - currentClipAmmo); // subtract only the ammo missing from the clip
            currentClipAmmo = maxClipAmmo;
        }
        
        // else reload the rest of the ammo
        else
        {
            currentClipAmmo = currentReloadAmmo;
            currentReloadAmmo = 0;
        }
    }

    public void AddAmo(int amount)
    {
        // if amount would fill up your max
        if (currentReloadAmmo + amount >= maxReloadAmmo)
            currentReloadAmmo = maxReloadAmmo;

        // else just add the amount
        else
            currentReloadAmmo += amount;
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
