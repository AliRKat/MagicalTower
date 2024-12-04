using System;
using UnityEngine;

public class Barrage : MonoBehaviour, IDamageDealer
{
    private Transform target;
    [HideInInspector] public float speed;
    [HideInInspector] public int damage;
    public int DamageAmount => damage;


    private void Start()
    {
        Invoke("DestroyObject", 5);
    }

    internal void Initialize(float speed, int damage, Transform target)
    {
        this.speed = speed;
        this.damage = damage;
        this.target = target;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
            transform.LookAt(target);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the projectile hits an enemy
        if (other.CompareTag("Enemy"))
        {
            // Get the PlayerHealth script on the player and apply damage
            HealthSystem health = other.gameObject.GetComponentInParent<HealthSystem>();
            if (health != null)
            {
                DealDamage(health);
            }
        }

        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
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