using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClearWave : MonoBehaviour
{
    public Animator animator;
    public string animToPlay;
    private bool hasPlayed = false;

    void Update()
    {
        if (hasPlayed) return;

        bool allCleared = true;

        // Loop through each child of this GameObject
        foreach (Transform child in transform)
        {
            // If the child has any children, it's not cleared
            if (child.childCount > 0)
            {
                allCleared = false;
                break;
            }
        }

        if (allCleared && animator != null)
        {
            animator.Play(animToPlay);
            hasPlayed = true; // Ensure animation only plays once
        }
    }
}
