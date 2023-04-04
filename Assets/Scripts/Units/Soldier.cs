using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public float moveSpeed = 5f; // The speed at which the soldier moves
    public float maxHealth = 100f; // The maximum health of the soldier
    public float currentHealth; // The current health of the soldier
    public float attackDamage = 10f; // The amount of damage the soldier can deal
    public float attackRange = 2f; // The range at which the soldier can attack
    public float attackCooldown = 1f; // The time between each attack

    private Transform target; // The target the soldier is currently attacking
    private float lastAttackTime; // The time at which the soldier last attacked

    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth; // Set the soldier's current health to their maximum health
    }

    // Update is called once per frame
    private void Update()
    {
        if (target == null) // If the soldier doesn't have a target, look for one
        {
            FindTarget();
        }
        else // If the soldier has a target, move towards it and attack if in range
        {
            MoveTowardsTarget();
            AttackTarget();
        }
    }

    // Find a new target to attack
    private void FindTarget()
    {
        // Here you could implement a system to find a nearby enemy unit or structure to attack, based on your game design
        // For the purposes of this example, we'll just set the target to the first enemy unit or structure we find
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            target = enemies[0].transform;
        }
    }

    // Move the soldier towards their target
    private void MoveTowardsTarget()
    {
        transform.LookAt(target); // Look at the target
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime); // Move towards the target
    }

    // Attack the soldier's target if in range
    private void AttackTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            target.GetComponent<Health>().TakeDamage(attackDamage); // Deal damage to the target
            lastAttackTime = Time.time; // Record the time at which the soldier last attacked
        }
    }

    // Take damage and check if the soldier is dead
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Destroy the soldier and remove it from the game
    private void Die()
    {
        Destroy(gameObject);
    }
}