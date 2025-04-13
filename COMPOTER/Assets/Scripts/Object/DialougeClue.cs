using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeClue : MonoBehaviour, IInteractable
{
    public GameObject dialouge;

    void Start()
    {
        if (dialouge != null)
        {
            dialouge.SetActive(false);
        }
    }
    public void Interact()
    {
        if (dialouge != null)
        {
            dialouge.SetActive(true);
        }
    }
}
