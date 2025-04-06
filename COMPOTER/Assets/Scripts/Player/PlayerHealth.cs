using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Transform cameraTransform;
    public float shakeDuration = 0.1f;
    public float baseShakeMagnitude = 0.1f;
    public float highDamageShakeMagnitude = 0.2f; // Increased shake for 20+ damage
    public HealthBar healthBar;
    private AudioSource audioSource;
    public AudioClip healsound;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void PlayerTakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthBar.SetHealth(currentHealth);

        if (cameraTransform != null)
        {
            float shakeIntensity = (damage >= 20) ? highDamageShakeMagnitude : baseShakeMagnitude;
            StartCoroutine(ShakeCamera(shakeIntensity));
        }

        if (currentHealth <= 0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Dead");
        }
    }

    IEnumerator ShakeCamera(float magnitude)
    {
        Vector3 originalPosition = cameraTransform.localPosition; // Save the initial position before shaking
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cameraTransform.localPosition = originalPosition + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;

            yield return null;
        }

        // Ensure the camera returns exactly to its original position
        cameraTransform.localPosition = originalPosition;
    }

    public void HealPlayer(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthBar.SetHealth(currentHealth);
    }
}
