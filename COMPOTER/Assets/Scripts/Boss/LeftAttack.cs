using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftAttack : MonoBehaviour
{
    public GameObject[] attackPrefabs;  // Array of prefabs to spawn
    public Transform[] attackPoints;    // Array of spawn points
    public float delayBetweenSpawns = 1f;  // Delay between spawning each prefab

    private bool isSpawning = true; // Flag to control the spawning loop

    void OnEnable()
    {
        // Start the coroutine to spawn prefabs sequentially with delays
        StartCoroutine(SpawnPrefabsWithDelay());
    }

    IEnumerator SpawnPrefabsWithDelay()
    {
        if (attackPrefabs.Length == 0 || attackPoints.Length == 0) yield break; // Ensure there are prefabs and spawn points

        // Loop indefinitely while the script is enabled
        while (isSpawning)
        {
            // Loop through each spawn point and spawn a prefab at that point
            for (int i = 0; i < attackPoints.Length; i++)
            {
                // Select the prefab in a cyclic manner (looping over the prefabs array)
                GameObject prefabToSpawn = attackPrefabs[i % attackPrefabs.Length];

                // Instantiate the prefab at the current spawn point
                Instantiate(prefabToSpawn, attackPoints[i].position, attackPoints[i].rotation);

                // Wait for the specified delay before continuing to the next spawn point
                yield return new WaitForSeconds(delayBetweenSpawns);
            }
        }
    }

    void OnDisable()
    {
        // Stop the coroutine and prevent further spawns
        isSpawning = false;
        StopCoroutine(SpawnPrefabsWithDelay());
    }
}
