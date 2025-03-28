using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameObject enemySpawner;
    public GameObject healthBar;
    public Animator doorAnim;
    public string animToPlay;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner.SetActive(false);
        healthBar.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemySpawner.SetActive(true);
            healthBar.SetActive(true);
            doorAnim.Play(animToPlay);
            Destroy(gameObject);
        }
    }
}
