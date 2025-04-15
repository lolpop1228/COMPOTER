using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEnemyBGMOff : MonoBehaviour
{
    public GameObject BGM;
    private bool canControlBGM = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canControlBGM && transform.childCount == 0)
        {
            if (BGM != null)
            {
                BGM.SetActive(false);
            }
        }
    }
}
