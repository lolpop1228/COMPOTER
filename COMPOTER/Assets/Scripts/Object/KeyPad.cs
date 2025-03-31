using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyPad : MonoBehaviour
{
    public Animator doorAnim;
    public string animToPlay;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            if (doorAnim != null)
            {
                doorAnim.Play(animToPlay);
            }
        }
    }
}
