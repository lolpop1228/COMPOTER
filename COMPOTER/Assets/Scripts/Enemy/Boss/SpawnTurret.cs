using UnityEngine;

public class SpawnTurret : MonoBehaviour
{
    public GameObject prefab;         // Prefab to spawn
    public Transform spawnCenter;     // Center of the sphere cast
    public float sphereRadius = 5f;   // Radius of the sphere cast
    public int spawnCount = 10;       // Number of prefabs to spawn

    void Start()
    {
        SpawnPrefabs();
    }

    void SpawnPrefabs()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPosition = GetRandomPositionInsideSphere();

            // Check if the position is occupied before spawning
            if (!Physics.CheckSphere(randomPosition, 0.5f)) 
            {
                Instantiate(prefab, randomPosition, Quaternion.identity);
            }
        }
    }

    Vector3 GetRandomPositionInsideSphere()
    {
        // Generate a random position inside a sphere
        Vector3 randomOffset = Random.insideUnitSphere * sphereRadius;
        Vector3 spawnPosition = spawnCenter.position + randomOffset;
        
        // Cast a sphere downward to find a valid ground position
        if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, sphereRadius))
        {
            return hit.point; // Ensure objects spawn on the surface
        }

        return spawnPosition; // Default if no ground is found
    }
}
