using UnityEngine;

/// <summary>
/// Manages collision for firearm projectiles
/// 
/// Ruben Sanchez
/// 
/// </summary>

public class Projectile : MonoBehaviour 
{
    [HideInInspector]
    public int damage;

    [SerializeField] private ParticleSystem impactParticle;

    private Rigidbody2D rigidB;

    private void Awake()
    {
        rigidB = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(impactParticle, transform.position, impactParticle.transform.rotation);

        if(collision.gameObject.GetComponent<Health>() != null)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    public void Shoot(Vector2 velocity)
    {
        rigidB.velocity = velocity; 
    }
}
