using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swayAmount = 0.1f; // How much the weapon sways
    public float swaySpeed = 5f;    // Speed of the sway
    public float returnSpeed = 10f; // Speed at which the weapon returns to its default position

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        // Store the initial position and rotation of the weapon
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        // Get player's movement input (horizontal and vertical)
        float moveX = Input.GetAxis("Mouse X");
        float moveY = Input.GetAxis("Mouse Y");

        // Calculate sway based on input, use Mathf.Sin for smooth oscillation
        float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount * moveX;
        float swayY = Mathf.Sin(Time.time * swaySpeed) * swayAmount * moveY;

        // Apply sway and return to the original position smoothly
        Vector3 targetPosition = initialPosition + new Vector3(swayX, swayY, 0f);
        Quaternion targetRotation = initialRotation * Quaternion.Euler(new Vector3(-swayY * 5f, swayX * 5f, 0));

        // Smoothly interpolate towards the target position and rotation
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * returnSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * returnSpeed);
    }
}
