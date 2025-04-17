using UnityEngine;

public class uiTriggerOnce : MonoBehaviour
{
    public GameObject uiBinary;
    public float triggerDistance = 10f;
    private Transform player;
    private bool hasTriggered = false;

    void Start()
    {
        player = Camera.main.transform;
    }

    void Update()
    {
        if (!hasTriggered && Vector3.Distance(player.position, transform.position) <= triggerDistance)
        {
            uiBinary.SetActive(true);
            hasTriggered = true;
        }
    }
}
