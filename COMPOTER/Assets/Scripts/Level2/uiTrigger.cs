using UnityEngine;
using UnityEngine.Playables;

public class uiTrigger : MonoBehaviour
{
    public GameObject uiBinary;
    public float triggerDistance = 10f;
    private Transform player;

    void Start()
    {
        player = Camera.main.transform;
    }

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <= triggerDistance)
        {
            if (!uiBinary.activeSelf)
                uiBinary.SetActive(true);
        }
        else
        {
            if (uiBinary.activeSelf)
                uiBinary.SetActive(false);
        }
    }
}
