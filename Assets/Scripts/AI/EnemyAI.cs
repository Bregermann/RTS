using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public float scoutInterval = 10f;
    public float attackInterval = 30f;
    public float gatherInterval = 20f;

    private float scoutTimer = 0f;
    private float attackTimer = 0f;
    private float gatherTimer = 0f;

    private List<GameObject> enemyUnits;
    private List<GameObject> playerUnits;

    private GameObject playerBase;
    private GameObject enemyBase;

    private List<GameObject> resourceNodes;

    private void Start()
    {
        // Get references to player and enemy units, as well as player and enemy bases
        enemyUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("EnemyUnit"));
        playerUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlayerUnit"));
        playerBase = GameObject.FindGameObjectWithTag("PlayerBase");
        enemyBase = GameObject.FindGameObjectWithTag("EnemyBase");

        // Get references to resource nodes
        resourceNodes = new List<GameObject>(GameObject.FindGameObjectsWithTag("ResourceNode"));
    }

    private void Update()
    {
        // Scout for player units and base every scoutInterval seconds
        scoutTimer += Time.deltaTime;
        if (scoutTimer >= scoutInterval)
        {
            Scout();
            scoutTimer = 0f;
        }

        // Attack player units or base every attackInterval seconds
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            Attack();
            attackTimer = 0f;
        }

        // Gather resources from nodes every gatherInterval seconds
        gatherTimer += Time.deltaTime;
        if (gatherTimer >= gatherInterval)
        {
            GatherResources();
            gatherTimer = 0f;
        }
    }

    private void Scout()
    {
        // Move enemy units towards player units and base
        foreach (GameObject enemyUnit in enemyUnits)
        {
            if (playerUnits.Count > 0)
            {
                GameObject closestPlayerUnit = GetClosestUnit(enemyUnit.transform.position, playerUnits);
                enemyUnit.GetComponent<UnitController>().MoveTo(closestPlayerUnit.transform.position);
            }
            else
            {
                enemyUnit.GetComponent<UnitController>().MoveTo(playerBase.transform.position);
            }
        }
    }

    private void Attack()
    {
        // Attack player units or base with enemy units
        foreach (GameObject enemyUnit in enemyUnits)
        {
            if (playerUnits.Count > 0)
            {
                GameObject closestPlayerUnit = GetClosestUnit(enemyUnit.transform.position, playerUnits);
                enemyUnit.GetComponent<UnitController>().Attack(closestPlayerUnit);
            }
            else
            {
                enemyUnit.GetComponent<UnitController>().Attack(playerBase);
            }
        }
    }

    private void GatherResources()
    {
        // Find the closest resource node to each unit and gather from it
        foreach (GameObject enemyUnit in enemyUnits)
        {
            float closestDistance = Mathf.Infinity;
            GameObject closestResource = null;
            foreach (GameObject resource in resources)
            {
                if (resource.GetComponent<Resource>().type == ResourceType.Dung || resource.GetComponent<Resource>().type == ResourceType.Bananas || resource.GetComponent<Resource>().type == ResourceType.Elderberries)
                {
                    float distance = Vector3.Distance(enemyUnit.transform.position, resource.transform.position);
                    if (distance < closestDistance)
                    {
                        closestResource = resource;
                        closestDistance = distance;
                    }
                }
            }

            if (closestResource != null)
            {
                enemyUnit.GetComponent<UnitController>().Gather(closestResource);
            }
        }
    }

    private void FindResources()
    {
        // Find all resources on the map
        resources = new List<GameObject>(GameObject.FindGameObjectsWithTag("Resource"));
    }

    private void OnDestroy()
    {
        // Remove reference to this script from all enemy units when destroyed
        foreach (GameObject enemyUnit in enemyUnits)
        {
            enemyUnit.GetComponent<UnitController>().SetEnemyAI(null);
        }
    }
}