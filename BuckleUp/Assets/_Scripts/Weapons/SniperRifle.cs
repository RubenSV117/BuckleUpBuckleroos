using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Defines behavior for sniper rifles
/// 
/// Ruben Sanchez
/// 6/23/18
/// </summary>

public class SniperRifle : ProjectileWeapon
{
    [Header("Camera Mods")]
    [Tooltip("Amount to increase the camera field of view by")]
    [Range(1, 10)]
    [SerializeField] private float orthographicSizeIncrease;

    [Tooltip("Amount to increase the camera offset by")]
    [Range(1, 10)]
    [SerializeField] private float cameraOffsetIncrease;

    [Tooltip("Speed multiplier for orthographic size switching")]
    [Range(1, 20)]
    [SerializeField] private float orthographicSwitchSpeed = 1;

    private bool raycastOnCooldown;


    // Raycast for a hit detection b/c for the distance and speed a sniper shot travels, OnCollisionEnter is not fully reliable
    public override void Attack()
    {
        if (raycastOnCooldown)
            return;

        // use base attack, the projectile on this weapon however will only be used as a bullet tracer with the trail renderer
        base.Attack();

        // Find Collider hit
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right * (GameManager.Instance.LocalPlayer.isFacingRight ? 1 : -1));

        // impact particle
        if(hit)
            Instantiate(projectile.GetComponent<Projectile>().impactParticle, hit.point, Quaternion.LookRotation(hit.normal));

        if(hit && hit.transform.GetComponent<Health>())
            hit.transform.GetComponent<Health>().TakeDamage(damage);

        raycastOnCooldown = true;
        StartCoroutine(RaycastCoolDown());

    }

    private IEnumerator RaycastCoolDown()
    {
        yield return new WaitForSeconds(1 / autoFireRate);
        raycastOnCooldown = false;
    }

    public override void OnEquip()
    {
        StartCoroutine(IncreaseOrthographicSize(orthographicSizeIncrease));
        GameManager.Instance.CameraLookAtOffset += cameraOffsetIncrease;
    }

    public override void OnUnequip()
    {
        StartCoroutine(DecreaseOrthographicSize(-orthographicSizeIncrease));
        GameManager.Instance.CameraLookAtOffset -= cameraOffsetIncrease;
    }

    private IEnumerator IncreaseOrthographicSize(float amount)
    {
        float initialSize = GameManager.Instance.ActiveCam.m_Lens.OrthographicSize;

        while (GameManager.Instance.ActiveCam.m_Lens.OrthographicSize < initialSize + amount)
        {
            GameManager.Instance.ActiveCam.m_Lens.OrthographicSize += Time.deltaTime * orthographicSwitchSpeed;
            yield return null;
        }
    }

    private IEnumerator DecreaseOrthographicSize(float amount)
    {
        float initialSize = GameManager.Instance.ActiveCam.m_Lens.OrthographicSize;

        while (GameManager.Instance.ActiveCam.m_Lens.OrthographicSize > initialSize + amount)
        {
            GameManager.Instance.ActiveCam.m_Lens.OrthographicSize -= Time.deltaTime * orthographicSwitchSpeed;
            yield return null;
        }
    }
}
