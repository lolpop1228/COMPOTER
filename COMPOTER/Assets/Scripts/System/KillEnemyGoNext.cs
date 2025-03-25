using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemyGoNext : MonoBehaviour
{
    public GameObject objectToActive;

    // Start is called before the first frame update
    void Start()
    {
        objectToActive.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount <= 0)
        {
            if (objectToActive != null)
                objectToActive.SetActive(true);
        }
    }
}
