 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPass : MonoBehaviour
{
    public Animator animator;
    public string animToPlay;
    bool hasStarted = false;

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            hasStarted = true;
            StartCoroutine(PlayAnim(25f));
        }
    }

    IEnumerator PlayAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.Play(animToPlay);
    }
}
