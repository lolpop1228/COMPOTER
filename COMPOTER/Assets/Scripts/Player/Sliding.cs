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
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Camera Effects")]
    public Camera playerCamera;
    public float slideFOV = 90f; // FOV when sliding
    private float normalFOV; // Store the original FOV
    public float fovChangeSpeed = 8f; // Speed of FOV transition

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    // Audio
    public AudioSource audioSource;
    public AudioClip slideSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        startYScale = playerObj.localScale.y;

        // Store the original camera FOV
        if (playerCamera != null)
            normalFOV = playerCamera.fieldOfView;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
            StartSlide();

        if (Input.GetKeyUp(slideKey) && pm.sliding)
            StopSlide();

        // Smoothly transition FOV
        if (playerCamera != null)
        {
            float targetFOV = pm.sliding ? slideFOV : normalFOV;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovChangeSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        pm.sliding = true;

        audioSource.PlayOneShot(slideSound);
        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Sliding normal
        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }
        // Sliding down a slope
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        pm.sliding = false;
        audioSource.Stop();

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }
}
