using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator.Play("Appear");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
