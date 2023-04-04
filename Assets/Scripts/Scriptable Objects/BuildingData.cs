using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building Data", menuName = "Building Data")]
public class BuildingData : ScriptableObject
{
    public string buildingName; // The name of the building
    public GameObject buildingPrefab; // The prefab of the building
    public float buildTime; // The amount of time it takes to build the building
    public float maxHealth = 100f; // The maximum health of the building
    public float currentHealth; // The current health of the building
    public int cost; // The cost to build the building
    public float energyProduction; // The amount of energy the building produces per second (if applicable)
    public float resourceProduction; // The amount of resources the building produces per second (if applicable)
    public bool isResourceDepot; // Whether the building is a resource depot
    public bool isEnergySource; // Whether the building is an energy source
    public bool isMilitaryBuilding; // Whether the building produces military units
    public bool isProductionBuilding; // Whether the building produces resources
    public bool isDefensiveBuilding; // Whether the building has defensive capabilities
    public float range; // The range of the building's attacks (if applicable)
    public float attackRate; // The rate at which the building can attack (if applicable)
    public float attackDamage; // The amount of damage the building does per attack (if applicable)

    // Initialize the building's current health to its maximum health
    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    // Take damage and check if the building is destroyed
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            DestroyBuilding();
        }
    }

    // Destroy the building and remove it from the game
    private void DestroyBuilding()
    {
        // Here you could add any other necessary logic for destroying the building, such as removing it from the player's available buildings list, etc.
        Destroy(gameObject);
    }
}