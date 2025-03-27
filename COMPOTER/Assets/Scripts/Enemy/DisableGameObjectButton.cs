using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObjectButton : MonoBehaviour
{
    public MonoBehaviour[] scriptsToDisable; // Array to hold scripts to disable

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box")) // Fix collision reference
        {
            foreach (MonoBehaviour script in scriptsToDisable)
            {
                if (script != null)
                {
                    script.enabled = false; // Disable script instead of GameObject
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
            {
                script.enabled = true; // Re-enable script when the box exits
            }
        }
    }
}
