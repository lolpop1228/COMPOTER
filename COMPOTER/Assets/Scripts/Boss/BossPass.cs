using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPass : MonoBehaviour
{
    public Animator animator;
    public string animToPlay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            animator.Play(animToPlay);
        }
    }
}
