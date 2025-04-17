using UnityEngine;

public class BinaryCubeInput : MonoBehaviour
{
    public string binaryValue;
    private GameManager gameManager;
    private AudioSource audioSource;
    public AudioClip audioClip;

    void Start()
    {

        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Button"))
        {
            gameManager.AddBinaryDigit(binaryValue);
            if (audioSource != null)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
    }
}
