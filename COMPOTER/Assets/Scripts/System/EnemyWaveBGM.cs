using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveBGM : MonoBehaviour
{
    public GameObject BGM;
    public GameObject ambient;
    private bool hasSwitched = false;

    void Update()
    {
        if (hasSwitched) return;

        bool allCleared = true;

        foreach (Transform child in transform)
        {
            if (child.childCount > 0)
            {
                allCleared = false;
                break;
            }
        }

        if (allCleared)
        {
            if (BGM != null)
            {
                BGM.SetActive(false);
            }

            if (ambient != null)
            {
                ambient.SetActive(true);
            }

            hasSwitched = true;
        }
    }
}
