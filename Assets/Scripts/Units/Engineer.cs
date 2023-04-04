using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engineer : MonoBehaviour
{
    public float moveSpeed = 5f; // The speed at which the engineer moves
    public float maxHealth = 100f; // The maximum health of the engineer
    public float currentHealth; // The current health of the engineer
    public float repairRate = 10f; // The rate at which the engineer can repair structures
    public float repairRange = 2f; // The range at which the engineer can repair structures

    private Transform target; // The target the engineer is currently repairing

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Set the engineer's current health to their maximum health
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) // If the engineer doesn't have a target, look for one
        {
            FindTarget();
        }
        else // If the engineer has a target, move towards it and repair if in range
        {
            MoveTowardsTarget();
            RepairTarget();
        }
    }

    // Find a new target to repair
    void FindTarget()
    {
        // Here you could implement a system to find a damaged structure or building to repair, based on your game design
        // For the purposes of this example, we'll just set the target to the first damaged structure or building we find
        GameObject[] damagedObjects = GameObject.FindGameObjectsWithTag("Damaged");
        if (damagedObjects.Length > 0)
        {
            target = damagedObjects[0].transform;
        }
    }

    // Move the engineer towards their target
    void MoveTowardsTarget()
    {
        transform.LookAt(target); // Look at the target
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime); // Move towards the target
    }

    // Repair the engineer's target if in range
    void RepairTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= repairRange)
        {
            target.GetComponent<Repairable>().Repair(repairRate * Time.deltaTime); // Repair the target
        }
    }

    // Take damage and check if the engineer is dead
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Destroy the engineer and remove it from the game
    void Die()
    {
        Destroy(gameObject);
    }
}
