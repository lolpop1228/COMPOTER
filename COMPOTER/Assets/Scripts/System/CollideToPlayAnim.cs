using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideToPlayAnim : MonoBehaviour
{
    public Animator fistAnimator;
    public string animToPlay;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (fistAnimator != null)
        {
            fistAnimator.Play(animToPlay);
        }
    }
}
