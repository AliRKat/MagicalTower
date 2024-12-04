using UnityEditorInternal;
using UnityEngine;

public class Enemy : HealthSystem, IDamageDealer
{
    [Header("Enemy Settings")]
    public float moveSpeed = 2f;
    public int damagePerSecond = 10;

    public int DamageAmount => damagePerSecond;

    public Transform target;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on the enemy!");
        }
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameOver()) 
        {
            return;
        }
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        if (target == null || rb == null) return;

        // Calculate direction towards the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Move the Rigidbody towards the target
        rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    public void DealDamage(HealthSystem target)
    {
        target.Damage(DamageAmount);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthSystem health = collision.gameObject.GetComponent<HealthSystem>();
            if (health != null)
            {
                DealDamage(health);
            }
        }
    }

    protected override void HandleHealthDepleted()
    {
        base.HandleHealthDepleted();
        gameObject.SetActive(false);
    }
}