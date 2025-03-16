using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDetect : MonoBehaviour
{
    public float rayRange = 2f;
    public Camera fpsCam;

    // Update is called once per frame
    void Update()
    {
        ShootRaycast();
    }

    void ShootRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, rayRange))
        {
            Debug.Log("Raycast Hit:" + hit.collider.gameObject.name);
        }
    }
}
