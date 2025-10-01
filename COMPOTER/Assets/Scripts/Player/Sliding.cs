using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    private PlayerMovement pm;
    private CharacterController controller;

    [Header("Sliding")]
    public float maxSlideTime = 1f;
    public float slideSpeed = 12f;
    private float slideTimer;

    public float slideHeight = 1f; // CharacterController height when sliding
    private float standHeight;
    private Vector3 standCenter;

    [Header("Cooldown")]
    public float slideCooldown = 1f; 
    private float cooldownTimer;
    private bool isOnCooldown;

    [Header("Camera Effects")]
    public Camera playerCamera;
    public float slideFOV = 90f;
    private float normalFOV;
    public float fovChangeSpeed = 8f;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip slideSound;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();

        standHeight = controller.height;
        standCenter = controller.center;

        if (playerCamera != null)
            normalFOV = playerCamera.fieldOfView;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Update cooldown
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f) isOnCooldown = false;
        }

        if (Input.GetKeyDown(slideKey))
        {
            if (!pm.sliding && !isOnCooldown && (horizontalInput != 0 || verticalInput != 0))
                StartSlide();
        }

        if (Input.GetKeyUp(slideKey) && pm.sliding)
            StopSlide();

        // Smooth FOV
        if (playerCamera != null)
        {
            float targetFOV = pm.sliding ? slideFOV : normalFOV;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovChangeSpeed);
        }

        // Auto-stop if timer runs out
        if (pm.sliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0) StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if (pm.sliding) SlidingMovement();
    }

    private void StartSlide()
    {
        pm.sliding = true;

        if (audioSource && slideSound)
            audioSource.PlayOneShot(slideSound);

        // Adjust CharacterController for slide
        controller.height = slideHeight;
        controller.center = new Vector3(standCenter.x, slideHeight / 2f, standCenter.z);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (inputDir.magnitude < 0.1f) inputDir = orientation.forward; // force slide forward if no input

        // Apply CharacterController movement
        Vector3 slideMove = inputDir.normalized * slideSpeed * Time.fixedDeltaTime;
        controller.Move(slideMove + Vector3.up * pm.verticalVelocity * Time.fixedDeltaTime); // integrate vertical movement
    }

    private void StopSlide()
    {
        pm.sliding = false;

        // Reset CharacterController height
        controller.height = standHeight;
        controller.center = standCenter;

        // Start cooldown
        isOnCooldown = true;
        cooldownTimer = slideCooldown;
    }
}
