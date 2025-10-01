using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float crouchSpeed = 3f;
    public float slideSpeed = 12f;

    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier = 2f;
    public float slopeIncreaseMultiplier = 2f;

    [Header("Jumping & Gravity")]
    public float jumpHeight = 2f;
    public float gravity = -20f;
    public float verticalVelocity;
    private bool readyToJump = true;
    public float jumpCooldown = 0.2f;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    public bool grounded;
    private bool wasGrounded;

    [Header("Crouching")]
    public float crouchYScale = 0.5f;
    private float startYScale;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("FOV")]
    public Camera fpsCam;
    private float normalFov;
    public float changeFovSpeed = 8f;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip walkSound;
    public AudioClip crouchSound;
    public AudioClip jumpSound;
    public AudioClip landSound;

    [Header("Crouching")]
    public float crouchHeight = 1f; // Height when crouching
    private float standHeight;
    private Vector3 standCenter;

    private float footstepTimer;
    public float footstepRate = 0.5f;

    private CharacterController controller;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    public Vector3 moveDirection;

    // 🔹 New smoothed velocity
    private Vector3 currentVelocity;
    public float smoothMoveSpeed = 10f; // Higher = snappier, lower = smoother

    public MovementState state;
    public enum MovementState
    {
        walking,
        crouching,
        sliding,
        air
    }

    public bool sliding;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        startYScale = transform.localScale.y;
        if (fpsCam != null) normalFov = fpsCam.fieldOfView;
        standHeight = controller.height;
        standCenter = controller.center;
    }

    private void Update()
    {
        GroundCheck();
        MyInput();
        StateHandler();
        HandleMovementSounds();
        HandleFOV();

        // Apply gravity
        if (grounded && verticalVelocity < 0)
            verticalVelocity = -2f; // stick to ground
        verticalVelocity += gravity * Time.deltaTime;

        // 🔹 Smooth velocity blending
        Vector3 targetVelocity = moveDirection * moveSpeed;
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.deltaTime * smoothMoveSpeed);

        // Apply final move
        Vector3 velocity = currentVelocity + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        if (!wasGrounded && grounded)
            PlaySound(landSound);

        wasGrounded = grounded;

        // Respawn if fall
        if (transform.position.y < -100f)
            ReloadScene();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;

        // Jump
        if (Input.GetKey(jumpKey) && grounded && readyToJump)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Crouch
        if (Input.GetKeyDown(crouchKey))
        {
            controller.height = crouchHeight;
            controller.center = new Vector3(standCenter.x, crouchHeight / 2f, standCenter.z);
            PlaySound(crouchSound);
        }
        if (Input.GetKeyUp(crouchKey))
        {
            controller.height = standHeight;
            controller.center = standCenter;
        }
    }

    private void StateHandler()
    {
        if (sliding)
        {
            state = MovementState.sliding;
            desiredMoveSpeed = slideSpeed;
        }
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }

        // Smooth transition for speed
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime * speedIncreaseMultiplier;
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
    }

    private void Jump()
    {
        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        PlaySound(jumpSound);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void HandleFOV()
    {
        // Only uses normal FOV now
        fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, normalFov, Time.deltaTime * changeFovSpeed);
    }

    private void HandleMovementSounds()
    {
        if (grounded && moveDirection.magnitude > 0.1f)
        {
            if (footstepTimer <= 0)
            {
                PlaySound(walkSound);
                footstepTimer = footstepRate;
            }
            else
            {
                footstepTimer -= Time.deltaTime;
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource)
            audioSource.PlayOneShot(clip);
    }

    private void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
