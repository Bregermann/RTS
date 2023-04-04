using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance
    public int startingResources = 1000; // Starting amount of resources for each player
    public int maxResources = 10000; // Maximum amount of resources that a player can have
    public float resourceGenerationInterval = 5f; // Interval at which resources are generated
    public int resourceGenerationAmount = 50; // Amount of resources generated per interval
    public List<PlayerController> players = new List<PlayerController>(); // List of players in the game
    public GameObject[] playerBases; // Array of player bases in the game
    public List<GameObject> resources = new List<GameObject>(); // List of resources in the game

    private float resourceGenerationTimer = 0f; // Timer for resource generation

    private void Awake()
    {
        // Set up singleton instance
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Find all player bases in the game
        playerBases = GameObject.FindGameObjectsWithTag("PlayerBase");

        // Set up players
        for (int i = 0; i < playerBases.Length; i++)
        {
            PlayerController player = new PlayerController(i, startingResources, playerBases[i]);
            players.Add(player);
        }
    }

    private void Update()
    {
        // Generate resources every resourceGenerationInterval seconds
        resourceGenerationTimer += Time.deltaTime;
        if (resourceGenerationTimer >= resourceGenerationInterval)
        {
            GenerateResources();
            resourceGenerationTimer = 0f;
        }
    }

    private void GenerateResources()
    {
        // Generate resources at random positions on the map
        for (int i = 0; i < players.Count; i++)
        {
            GameObject resource = ResourceSpawner.SpawnResource(players[i].playerBase.transform.position, players[i].playerNumber);
            resources.Add(resource);
        }
    }
}