using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossResult : MonoBehaviour
{
    public MonoBehaviour[] scriptToDisable;

    void OnEnable()
    {
        //Disable all script
        foreach (MonoBehaviour script in scriptToDisable)
        {
            script.enabled = false;
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
