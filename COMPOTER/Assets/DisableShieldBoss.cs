using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableShieldBoss : MonoBehaviour
{
    public GameObject targetObject;

    void Update()
    {
        if (transform.childCount == 0)
        {
            targetObject.SetActive(false);
        }
    }
}
