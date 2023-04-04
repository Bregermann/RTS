using UnityEngine;

public class Train : MonoBehaviour
{
    public float speed = 10f;
    public float carryingCapacity = 10f;

    private Transform destination;
    private float distanceToDestination;
    private float currentCarryingCapacity;

    private void Start()
    {
        destination = transform;
    }

    private void Update()
    {
        // Move towards the current destination
        Vector3 direction = (destination.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Check if reached destination
        distanceToDestination = Vector3.Distance(transform.position, destination.position);
        if (distanceToDestination <= 0.1f)
        {
            // If reached destination, stop and unload units or resources
            if (destination.tag == "UnitDestination")
            {
                UnloadUnits();
            }
            else if (destination.tag == "ResourceDestination")
            {
                UnloadResources();
            }
            destination = transform;
        }
    }

    public void SetDestination(Transform dest)
    {
        destination = dest;
    }

    public void LoadUnits(GameObject unit)
    {
        // Check if train has capacity to carry more units
        if (currentCarryingCapacity < carryingCapacity)
        {
            // Add unit to list of carried units
            unit.transform.parent = transform;
            unit.GetComponent<UnitController>().Stop();
            currentCarryingCapacity++;
        }
    }

    private void UnloadUnits()
    {
        // Unload all carried units
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Unit"))
            {
                child.parent = null;
                child.GetComponent<UnitController>().MoveTo(destination.position);
            }
        }
        currentCarryingCapacity = 0;
    }

    public void LoadResources(float amount)
    {
        // Check if train has capacity to carry more resources
        if (currentCarryingCapacity < carryingCapacity)
        {
            // Add resources to carried amount
            currentCarryingCapacity += amount;
        }
    }

    private void UnloadResources()
    {
        // Send resources to resource manager
        ResourceManager.Instance.ReceiveResources(currentCarryingCapacity);
        currentCarryingCapacity = 0;
    }
}

public class TransportTrain : Train
{
    // No additional functionality needed for transport train
}

public class ResourceTrain : Train
{
    // No additional functionality needed for resource train
}

public class TrainTrackBuilder : MonoBehaviour
{
    public float buildSpeed = 5f;

    private bool building;
    private Transform buildDestination;
    private float buildProgress;

    private void Start()
    {
        building = false;
    }

    private void Update()
    {
        if (building)
        {
            // Build track towards destination
            Vector3 direction = (buildDestination.position - transform.position).normalized;
            transform.position += direction * buildSpeed * Time.deltaTime;

            // Check if reached destination
            float distanceToDestination = Vector3.Distance(transform.position, buildDestination.position);
            if (distanceToDestination <= 0.1f)
            {
                buildProgress += buildSpeed * Time.deltaTime;
                if (buildProgress >= 100f)
                {
                    // Finish building track
                    buildDestination.gameObject.AddComponent<BoxCollider>();
                    building = false;
                    buildProgress = 0f;
                    buildDestination = null;
                }
            }
        }
    }

    public void BuildTrack(Transform destination)
    {
        if (!building)
        {
            // Start building track towards destination
            building = true;
            buildDestination = destination;
        }
    }
}