using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMOn : MonoBehaviour
{
    public GameObject BGM;

    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (BGM != null)
            {
                BGM.SetActive(true);
            }
        }
    }
}
