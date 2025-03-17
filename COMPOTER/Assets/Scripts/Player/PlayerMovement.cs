using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    private float currentSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // ===== Added Head Bobbing Variables =====
    [Header("Head Bobbing")]
    public Transform cameraHolder; // Assign the player's camera transform
    public float bobbingSpeed = 14f; // Speed of the bobbing
    public float bobbingAmount = 0.05f; // How much the camera moves up and down
    private float defaultYPos; // Original Y position of the camera
    private float timer; // Timer for sine wave function
    public float bobLerpSpeed = 8f;
    private float bobbingIntensity = 0f;
    // ========================================

    [Header("Footstep Sounds")]
    public AudioSource footstepSource; // Assign in the Inspector
    public AudioClip[] footstepClips;  // Assign footstep sounds in Inspector
    public float walkStepInterval = 0.5f; // Time between steps when walking
    public float runStepInterval = 0.3f;  // Time between steps when running
    private float footstepTimer = 0f; // Timer for footstep sounds

    [Header("Jump & Landing Sounds")]
    public AudioSource audioSource; // Assign the same AudioSource as footstepSource or a separate one
    public AudioClip jumpSound;  // Assign a jump sound in Inspector
    public AudioClip landSound;  // Assign a landing sound in Inspector

    private bool wasGrounded; // Track previous grounded state for landing sound



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        // Set the default speed to moveSpeed (walking)
        currentSpeed = moveSpeed;

        // ===== Store the camera's default position =====
        if (cameraHolder != null)
        {
            defaultYPos = cameraHolder.localPosition.y;
        }
        // ===============================================
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        // Handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        // Toggle sprinting
        if (Input.GetKey(sprintKey))
        {
            currentSpeed = runSpeed; // Sprint speed
        }
        else
        {
            currentSpeed = moveSpeed; // Walk speed
        }

        // ===== Apply head bobbing effect =====
        HandleHeadBobbing();
        // =====================================

        HandleFootsteps();

        // Play landing sound when hitting the ground after being in the air
        if (!wasGrounded && grounded)
        {
            if (audioSource != null && landSound != null)
                audioSource.PlayOneShot(landSound);
        }

        // Update grounded state for next frame
        wasGrounded = grounded;


    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on slope
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
        }

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * currentSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        // Play jump sound
        if (audioSource != null && jumpSound != null)
            audioSource.PlayOneShot(jumpSound);

    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
 
    // ===== Head Bobbing Function =====
    private void HandleHeadBobbing()
    {
        if (cameraHolder == null) return;

        // Check if the player is moving
        if (grounded && (horizontalInput != 0 || verticalInput != 0))
        {
            float bobSpeed = (currentSpeed == runSpeed) ? bobbingSpeed * 1.5f : bobbingSpeed;

            timer += Time.deltaTime * bobSpeed;

            // Smoothly increase bobbing intensity when moving
            bobbingIntensity = Mathf.Lerp(bobbingIntensity, 1f, Time.deltaTime * bobLerpSpeed);

            float newY = defaultYPos + Mathf.Sin(timer) * bobbingAmount * bobbingIntensity;
            cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, newY, cameraHolder.localPosition.z);
        }
        else
        {
            // Smoothly reduce bobbing when stopping
            bobbingIntensity = Mathf.Lerp(bobbingIntensity, 0f, Time.deltaTime * bobLerpSpeed);

            cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, Mathf.Lerp(cameraHolder.localPosition.y, defaultYPos, Time.deltaTime * bobLerpSpeed), cameraHolder.localPosition.z);
        }
    }

    private void HandleFootsteps()
    {
        if (!grounded || (horizontalInput == 0 && verticalInput == 0))
        {
            footstepTimer = 0; // Reset timer when not moving
            return;
        }

        // Determine step interval based on movement speed
        float stepInterval = (currentSpeed == runSpeed) ? runStepInterval : walkStepInterval;

        // Increment timer and play footstep sound if interval is met
        footstepTimer += Time.deltaTime;
        if (footstepTimer >= stepInterval)
        {
            footstepTimer = 0; // Reset timer

            if (footstepSource != null && footstepClips.Length > 0)
            {
                footstepSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
            }
        }
    }
}
