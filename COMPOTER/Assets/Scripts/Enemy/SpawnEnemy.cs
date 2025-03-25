using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemySpawner;
    public Animator doorAnim;

    void Start()
    {
        enemySpawner.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemySpawner.SetActive(true);
            doorAnim.Play("Close");
            Destroy(gameObject);
        }
    }
}
