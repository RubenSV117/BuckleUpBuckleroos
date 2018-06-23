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


    // Raycast for a hit detection b/c for the distance and speed a sniper shot travels, OnCollisionEnter is not fully reliable
    public override void Attack()
    {
        // use base attack, the projectile on this weapon however will only be used as a bullet tracer with the trail renderer
        base.Attack();

        // Find Collider hit
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right * transform.root.localScale.x);

        if(hit && hit.transform.GetComponent<Health>())
            hit.transform.GetComponent<Health>().TakeDamage(damage);

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
