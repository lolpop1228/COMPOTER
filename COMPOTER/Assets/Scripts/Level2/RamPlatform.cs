using UnityEngine;

public class RamPlatform : MonoBehaviour
{
    public Transform player;
    public GameObject otherObject;
    public float triggerDistance = 5f;
    private bool isActive = false;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        bool shouldActivate = distanceToPlayer <= triggerDistance;

        if (shouldActivate != isActive)
        {
            SetOtherObjectActive(shouldActivate);
        }
    }

    private void SetOtherObjectActive(bool state)
    {
        if (state != isActive)
        {
            otherObject.SetActive(state);
            isActive = state;
        }
    }
}
