using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellcastingController : MonoBehaviour
{
    public Fireball fireballPrefab;
    public Barrage barragePrefab;
    public Transform tower;

    public float fireballCooldown = 3f; // Cooldown for Fireball in seconds
    public float barrageCooldown = 1f; // Cooldown for Barrage in seconds
    public float detectionRadius = 150f; // Radius for detecting enemies for both Fireball and Barrage

    [Header("Fireball Settings")]
    public int fireballDamage;
    public float fireballSpeed;
    public float fireballRadius;

    [Header("Barrage Settings")]
    public int barrageDamage;
    public float barrageSpeed;

    // Dictionary to track cooldowns for multiple spells
    private Dictionary<string, bool> spellCooldowns = new Dictionary<string, bool>();

    private void Start()
    {
        // Initialize spell cooldowns
        spellCooldowns["Fireball"] = false;
        spellCooldowns["Barrage"] = false;
    }

    private void Update()
    {
        // Fireball spell logic
        if (Input.GetKeyDown(KeyCode.F) && !spellCooldowns["Fireball"]) // Fireball Spell
        {
            CastFireball();
        }

        // Barrage spell logic
        if (Input.GetKeyDown(KeyCode.B) && !spellCooldowns["Barrage"]) // Barrage Spell
        {
            CastBarrage();
        }
    }

    private void CastFireball()
    {
        // Find a random direction within a specified radius (e.g., 10 units away)
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection.y = 0; // Keep the fireball on the same Y level (horizontal direction)

        // Instantiate the fireball and initialize its direction and properties
        Fireball fireball = Instantiate(fireballPrefab, tower.position, Quaternion.identity);
        fireball.Initialize(fireballDamage, randomDirection.normalized, fireballSpeed, fireballRadius); // Damage, direction, speed, area radius

        // Start the cooldown timer for Fireball
        StartCoroutine(SpellCooldown("Fireball", fireballCooldown));
    }

    private void CastBarrage()
    {
        // Perform a spherecast to detect enemies within the detection radius
        Collider[] enemiesInRange = Physics.OverlapSphere(tower.position, detectionRadius, LayerMask.GetMask("Enemy"));

        foreach (Collider enemyCollider in enemiesInRange)
        {
            Transform enemy = enemyCollider.transform;

            // For each enemy in range, instantiate a barrage projectile
            Barrage barrage = Instantiate(barragePrefab, tower.position, Quaternion.identity);
            barrage.Initialize(barrageSpeed, barrageDamage, enemy.parent.transform);
        }

        // Start the cooldown timer for Barrage
        StartCoroutine(SpellCooldown("Barrage", barrageCooldown));
    }

    // General cooldown function for both spells, uses the spell name to update the cooldown
    private IEnumerator SpellCooldown(string spellName, float cooldownTime)
    {
        spellCooldowns[spellName] = true; // Set the spell's cooldown flag to true
        yield return new WaitForSeconds(cooldownTime); // Wait for the cooldown duration
        spellCooldowns[spellName] = false; // Reset the spell's cooldown flag to false
    }
}