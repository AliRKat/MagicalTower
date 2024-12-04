using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellcastingController : MonoBehaviour
{
    public Fireball fireballPrefab;
    public Barrage barragePrefab;
    public Transform tower;

    public float fireballCooldown = 3f;
    public float barrageCooldown = 1f;
    public float detectionRadius = 150f;

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
        spellCooldowns["Fireball"] = false;
        spellCooldowns["Barrage"] = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !spellCooldowns["Fireball"])
        {
            CastFireball();
        }
        if (Input.GetKeyDown(KeyCode.B) && !spellCooldowns["Barrage"])
        {
            CastBarrage();
        }
    }

    private void CastFireball()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection.y = 0;

        Fireball fireball = Instantiate(fireballPrefab, tower.position, Quaternion.identity);
        fireball.Initialize(fireballDamage, randomDirection.normalized, fireballSpeed, fireballRadius);

        StartCoroutine(SpellCooldown("Fireball", fireballCooldown));
    }

    private void CastBarrage()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(tower.position, detectionRadius, LayerMask.GetMask("Enemy"));

        foreach (Collider enemyCollider in enemiesInRange)
        {
            Transform enemy = enemyCollider.transform;
            Barrage barrage = Instantiate(barragePrefab, tower.position, Quaternion.identity);
            barrage.Initialize(barrageSpeed, barrageDamage, enemy.parent.transform);
        }

        StartCoroutine(SpellCooldown("Barrage", barrageCooldown));
    }

    // General cooldown function for spells, uses the spell name to update the cooldown
    private IEnumerator SpellCooldown(string spellName, float cooldownTime)
    {
        spellCooldowns[spellName] = true; // Set the spell's cooldown flag to true
        yield return new WaitForSeconds(cooldownTime); // Wait for the cooldown duration
        spellCooldowns[spellName] = false; // Reset the spell's cooldown flag to false
    }
}