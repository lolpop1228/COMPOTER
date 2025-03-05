using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlayerMovement : MonoBehaviour
{
    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
