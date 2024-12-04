using UnityEngine;

public class Fireball : MonoBehaviour, IDamageDealer
{
    [HideInInspector] public int damage;
    [HideInInspector] public float projectileSpeed;
    [HideInInspector] public float areaEffectRadius;
    public int DamageAmount => damage; 
    private Vector3 direction;

    public void Initialize(int damage, Vector3 direction, float speed, float areaEffectRadius)
    {
        this.damage = damage;
        this.direction = direction;
        this.projectileSpeed = speed;
        this.areaEffectRadius = areaEffectRadius;
    }
    private void Start()
    {
        Invoke("DestroyObject", 5);
    }

    private void Update()
    {
        transform.Translate(direction * projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Enemy"))
        {
            Explode(transform.position);
        }
    }

    private void Explode(Vector3 explosionCenter)
    {
        Collider[] colliders = Physics.OverlapSphere(explosionCenter, areaEffectRadius);
        foreach (Collider collider in colliders)
        {
            // Check if the projectile hits an enemy
            if (collider.CompareTag("Enemy"))
            {
                // Get the PlayerHealth script on the player and apply damage
                HealthSystem health = collider.gameObject.GetComponentInParent<HealthSystem>();
                if (health != null)
                {
                    DealDamage(health);
                }
            }
        }
    }

    public void DealDamage(HealthSystem target)
    {
        target.Damage(DamageAmount);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}