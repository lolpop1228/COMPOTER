using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustSpawnEnemy : MonoBehaviour
{
    public GameObject enemySpawner;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemySpawner.SetActive(true);
            Destroy(gameObject);
        }
    }
}
