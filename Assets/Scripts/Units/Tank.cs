using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public float moveSpeed = 5f; // The speed at which the tank moves
    public float maxHealth = 100f; // The maximum health of the tank
    public float currentHealth; // The current health of the tank
    public float attackDamage = 20f; // The amount of damage the tank does per attack
    public float attackRange = 5f; // The range at which the tank can attack
    public float attackRate = 2f; // The rate at which the tank can attack
    private float timeSinceLastAttack = 0f; // The time since the tank's last attack

    private Transform target; // The target the tank is currently attacking

    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth; // Set the tank's current health to its maximum health
    }

    // Update is called once per frame
    private void Update()
    {
        if (target == null) // If the tank doesn't have a target, look for one
        {
            FindTarget();
        }
        else // If the tank has a target, move towards it and attack if in range
        {
            MoveTowardsTarget();
            AttackTarget();
        }
    }

    // Find a new target to attack
    private void FindTarget()
    {
        // Here you could implement a system to find an enemy unit or structure to attack, based on your game design
        // For the purposes of this example, we'll just set the target to the first enemy unit we find
        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
        if (enemyUnits.Length > 0)
        {
            target = enemyUnits[0].transform;
        }
    }

    // Move the tank towards its target
    private void MoveTowardsTarget()
    {
        transform.LookAt(target); // Look at the target
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime); // Move towards the target
    }

    // Attack the tank's target if in range
    private void AttackTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= attackRange)
        {
            if (timeSinceLastAttack >= 1f / attackRate) // If the tank has waited long enough since its last attack, attack again
            {
                target.GetComponent<Unit>().TakeDamage(attackDamage); // Deal damage to the target
                timeSinceLastAttack = 0f; // Reset the time since the tank's last attack
            }
            else
            {
                timeSinceLastAttack += Time.deltaTime; // Add the time since the last frame to the time since the tank's last attack
            }
        }
    }

    // Take damage and check if the tank is dead
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Destroy the tank and remove it from the game
    private void Die()
    {
        Destroy(gameObject);
    }
}