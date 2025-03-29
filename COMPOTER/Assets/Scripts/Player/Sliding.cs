using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Sliding")]
    public float maxSlideTime = 2f; // Max time the player can slide
    public float slideForce = 5f; // Force applied during sliding
    private float slideTimer;

    public float slideYScale = 0.5f; // Y scale for sliding to make the player smaller
    private float startYScale;

    [Header("Cooldown")]
    public float slideCooldown = 1.5f; // Cooldown duration before sliding again
    private bool canSlide = true;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    [Header("Sounds")]
    public AudioSource slidingAudioSource; // AudioSource reference for sliding sound
    public AudioClip slidingSound; // Sliding sound clip

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        startYScale = playerObj.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0) && canSlide)
        {
            StartSlide();
        }

        if (pm.sliding)
        {
            slideTimer -= Time.deltaTime; // Reduce slide timer

            if (slideTimer <= 0)
            {
                StopSlide(); // Stop slide when max time is reached
            }
        }

        if (Input.GetKeyUp(slideKey) && pm.sliding)
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
        {
            SlidingMovement();
        }
    }

    private void StartSlide()
    {
        pm.sliding = true;
        canSlide = false; // Prevent sliding again immediately

        // Shrink player for the sliding effect
        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;

        // Play sliding sound
        if (slidingAudioSource && slidingSound)
        {
            slidingAudioSource.clip = slidingSound;
            slidingAudioSource.loop = true; // Loop the sliding sound
            slidingAudioSource.Play();
        }
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Normal sliding when not on a slope or not falling
        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
        }
        else
        {
            // Sliding down a slope
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }
    }

    private void StopSlide()
    {
        pm.sliding = false;

        // Reset player scale to normal
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);

        // Stop sliding sound
        if (slidingAudioSource)
        {
            slidingAudioSource.Stop();
        }

        StartCoroutine(SlideCooldownTimer()); // Start cooldown timer
    }

    private IEnumerator SlideCooldownTimer()
    {
        yield return new WaitForSeconds(slideCooldown);
        canSlide = true; // Allow sliding again after cooldown
    }
}
