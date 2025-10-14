using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossTimer : MonoBehaviour
{
    public BossFightController bossFightController;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bossFightController.StartTimer();
        }
    }
}
