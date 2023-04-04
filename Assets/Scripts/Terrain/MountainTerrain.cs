using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MountainTerrain : MonoBehaviour
{
    public float gridSize = 1f; // Size of grid squares for pathfinding
    public LayerMask obstacleMask; // Layers to consider as obstacles for pathfinding
    public MeshCollider terrainCollider; // Collider to use for collision detection
    public float maxSlopeAngle = 45f; // Maximum slope angle that units can climb

    private NavMesh navMesh; // Reference to NavMesh component

    // Start is called before the first frame update
    private void Start()
    {
        // Get reference to NavMesh component
        navMesh = GetComponent<NavMesh>();

        // Build NavMesh
        NavMeshData navMeshData = NavMeshBuilder.BuildNavMeshData(navMesh.GetSettingsByID(0), GetSources(), new Bounds(transform.position, terrainCollider.bounds.size), transform.position, Quaternion.identity);
        navMesh.UpdateNavMesh(navMeshData);
    }

    // Get sources for NavMeshBuilder
    private List<NavMeshBuildSource> GetSources()
    {
        List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();

        // Add terrain mesh as source
        NavMeshBuildSource terrainSource = new NavMeshBuildSource();
        terrainSource.sourceObject = terrainCollider.sharedMesh;
        terrainSource.transform = terrainCollider.transform.localToWorldMatrix;
        terrainSource.area = 0; // Set area type to walkable
        sources.Add(terrainSource);

        return sources;
    }

    // Check if a position is within the terrain bounds and not blocked by an obstacle
    public bool IsPositionValid(Vector3 position)
    {
        // Check if position is within terrain bounds
        if (!terrainCollider.bounds.Contains(position))
        {
            return false;
        }

        // Check if position is blocked by an obstacle
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, Mathf.Infinity, obstacleMask))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle > maxSlopeAngle)
            {
                return false;
            }
        }

        return true;
    }

    // Get closest valid position to target position
    public Vector3 GetClosestValidPosition(Vector3 targetPosition)
    {
        // Start at target position and move up until a valid position is found
        Vector3 currentPosition = targetPosition;
        while (!IsPositionValid(currentPosition))
        {
            currentPosition.y += gridSize;
            if (currentPosition.y > terrainCollider.bounds.max.y)
            {
                return targetPosition; // Return target position if no valid position is found
            }
        }

        // Move down until the closest valid position is found
        while (true)
        {
            Vector3 nextPosition = currentPosition;
            nextPosition.y -= gridSize;
            if (IsPositionValid(nextPosition))
            {
                return currentPosition;
            }
            else
            {
                currentPosition = nextPosition;
            }
        }
    }

    // Draw gizmos for debugging
    private void OnDrawGizmosSelected()
    {
        // Draw grid
        Gizmos.color = Color.white;
        for (float x = terrainCollider.bounds.min.x; x < terrainCollider.bounds.max.x; x += gridSize)
        {
            for (float z = terrainCollider.bounds.min.z; z < terrainCollider.bounds.max.z; z += gridSize)
            {
                Vector3 position = new Vector3(x, terrainCollider.bounds.min.y, z);
                if (IsPositionValid(position))
                {
                    Gizmos.DrawWireCube(position + new Vector3(gridSize / 2f, 0f, gridSize / 2f), new Vector3(gridSize, 0.1f, gridSize));
                }
            }
        }

        // Draw slope angle
        Gizmos.color = Color.red;
        for (float x = terrainCollider.bounds.min.x; x < terrainCollider.bounds.max.x; x += gridSize)
        {
            for (float z = terrainCollider.bounds.min.z; z < terrainCollider.bounds.max.z; z += gridSize)
            {
                Vector3 position = new Vector3(x, terrainCollider.bounds.min.y, z);
                if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, Mathf.Infinity, obstacleMask))
                {
                    float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                    if (slopeAngle > maxSlopeAngle)
                    {
                        Gizmos.DrawCube(position + new Vector3(gridSize / 2f, 0f, gridSize / 2f), new Vector3(gridSize, 0.1f, gridSize));
                    }
                }
            }
        }
    }
}