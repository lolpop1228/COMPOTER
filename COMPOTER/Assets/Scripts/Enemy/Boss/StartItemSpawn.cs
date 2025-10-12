using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartItemSpawn : MonoBehaviour
{
    public ItemSpawner itemSpawner;

    void OnDestroy()
    {
        if (itemSpawner != null)
        {
            itemSpawner.StartSpawning();
        }
    }
}
