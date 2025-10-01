using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement;
    public Transform cameraHolder;

    [Header("Head Bob Settings")]
    public float walkBobSpeed = 8f;
    public float walkBobAmount = 0.05f;

    public float crouchBobSpeed = 4f;
    public float crouchBobAmount = 0.025f;

    private float defaultYPos;
    private float timer;

    void Start()
    {
        if (cameraHolder == null)
            cameraHolder = transform;

        defaultYPos = cameraHolder.localPosition.y;
    }

    void Update()
    {
        if (playerMovement == null || cameraHolder == null)
            return;

        // Moving only when input + grounded
        bool isMoving = playerMovement.moveDirection.magnitude > 0.1f && playerMovement.grounded;

        if (isMoving)
        {
            float bobSpeed = walkBobSpeed;
            float bobAmount = walkBobAmount;

            if (playerMovement.state == PlayerMovement.MovementState.crouching)
            {
                bobSpeed = crouchBobSpeed;
                bobAmount = crouchBobAmount;
            }

            timer += Time.deltaTime * bobSpeed;
            float bobOffset = Mathf.Sin(timer) * bobAmount;
            Vector3 newPos = new Vector3(
                cameraHolder.localPosition.x,
                defaultYPos + bobOffset,
                cameraHolder.localPosition.z
            );
            cameraHolder.localPosition = newPos;
        }
        else
        {
            // Reset smoothly to default position
            Vector3 targetPosition = new Vector3(
                cameraHolder.localPosition.x,
                defaultYPos,
                cameraHolder.localPosition.z
            );
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, targetPosition, Time.deltaTime * 6f);
            timer = 0f;
        }
    }
}
