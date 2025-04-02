using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAttackScript : MonoBehaviour
{
    public GameObject[] attackPrefabs;
    public Transform[] attackPoints;
    public float spawnInterval = 3f;

    void OnEnable()
    {
        InvokeRepeating("SpawnPrefabsAtRandomPoints", 0f, spawnInterval);
    }

    void SpawnPrefabsAtRandomPoints()
    {
        if (attackPrefabs.Length == 0 || attackPoints.Length < 2) return; // Ensure there are at least two spawn points

        // Select two different random prefabs
        GameObject randomPrefab1 = attackPrefabs[Random.Range(0, attackPrefabs.Length)];
        GameObject randomPrefab2 = attackPrefabs[Random.Range(0, attackPrefabs.Length)];

        // Ensure the two spawn points are different
        Transform randomPoint1 = attackPoints[Random.Range(0, attackPoints.Length)];
        Transform randomPoint2;
        do
        {
            randomPoint2 = attackPoints[Random.Range(0, attackPoints.Length)];
        } while (randomPoint2 == randomPoint1);  // Ensure the points are different

        // Instantiate both prefabs at the random spawn points
        GameObject mainAttack1 = Instantiate(randomPrefab1, randomPoint1.position, randomPoint1.rotation);
        GameObject mainAttack2 = Instantiate(randomPrefab2, randomPoint2.position, randomPoint2.rotation);

        // Destroy the instantiated prefabs after 4 seconds
        Destroy(mainAttack1, 4f);
        Destroy(mainAttack2, 4f);
    }

    void OnDisable()
    {
        CancelInvoke("SpawnPrefabsAtRandomPoints");
    }
}
