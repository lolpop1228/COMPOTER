 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMOn : MonoBehaviour
{
    public GameObject BGM;
    public GameObject ambient;

    void Start()
    {
        if (BGM != null)
            {
                BGM.SetActive(false);
            }
        if (ambient != null)
        {
            ambient.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (BGM != null)
            {
                BGM.SetActive(true);
            }if (ambient != null)
            {
                ambient.SetActive(false);
            }
        }
    }
}
