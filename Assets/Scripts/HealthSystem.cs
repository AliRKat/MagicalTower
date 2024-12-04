using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    protected float currentHealth;

    [Header("UI")]
    public Slider healthBar; // Optional health bar for the object

    public delegate void HealthDepleted(GameObject obj);
    public event HealthDepleted OnHealthDepleted;

    protected virtual void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Initialize health bar if available
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    /// <summary>
    /// Apply damage to the object.
    /// </summary>
    /// <param name="damage">Amount of damage to apply.</param>
    public void Damage(float damage)
    {
        currentHealth -= damage;

        // Clamp health to prevent over-damage
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update health bar if available
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
            
        // Trigger event if health is depleted
        if (currentHealth <= 0)
        {
            HandleHealthDepleted();
        }
    }

    /// <summary>
    /// Heal the object by a specified amount.
    /// </summary>
    /// <param name="amount">Amount of health to restore.</param>
    public void Heal(float amount)
    {
        currentHealth += amount;

        // Clamp health to prevent over-healing
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update health bar if available
        if (healthBar != null) 
        { 
            healthBar.value = currentHealth; 
        }
    }

    /// <summary>
    /// Handles logic when health reaches zero.
    /// Override this in child classes to add custom behavior.
    /// </summary>
    protected virtual void HandleHealthDepleted()
    {
        OnHealthDepleted?.Invoke(gameObject);
    }
}