using UnityEngine;

public class BinaryCubeInput : MonoBehaviour
{
    public string binaryValue;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Button"))
        {
            gameManager.AddBinaryDigit(binaryValue);
        }
    }
}
