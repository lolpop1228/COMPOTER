using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public HealthBar healthBar;

    [Header("Camera Shake")]
    public Transform cameraTransform;
    public float shakeDuration = 0.1f;
    public float baseShakeMagnitude = 0.1f;
    public float highDamageShakeMagnitude = 0.2f; // Increased shake for 20+ damage

    [Header("UI & Effects")]
    public GameObject hurtPanel; // The red UI panel
    public float hurtScreenDuration = 0.3f; // How long red screen is visible
    public float hurtFadeSpeed = 5f;        // Speed of fade effect
    private Image hurtImage;                // Cached Image component

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip healsound;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        if (hurtPanel != null)
        {
            hurtImage = hurtPanel.GetComponent<Image>();
            Color c = hurtImage.color;
            c.a = 0f; // Start fully transparent
            hurtImage.color = c;
        }
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

        // 🔹 Flash the hurt screen
        if (hurtPanel != null)
            StartCoroutine(FlashHurtPanel());

        if (currentHealth <= 0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Dead");
        }
    }

    IEnumerator FlashHurtPanel()
    {
        float elapsed = 0f;

        // Fade in quickly
        while (elapsed < 0.1f)
        {
            elapsed += Time.deltaTime * hurtFadeSpeed;
            Color c = hurtImage.color;
            c.a = Mathf.Lerp(0f, 0.6f, elapsed / 0.1f); // Up to 60% opacity
            hurtImage.color = c;
            yield return null;
        }

        // Hold the red flash
        yield return new WaitForSeconds(hurtScreenDuration);

        // Fade out smoothly
        elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime * hurtFadeSpeed;
            Color c = hurtImage.color;
            c.a = Mathf.Lerp(0.6f, 0f, elapsed / 0.5f);
            hurtImage.color = c;
            yield return null;
        }
    }

    IEnumerator ShakeCamera(float magnitude)
    {
        Vector3 originalPosition = cameraTransform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cameraTransform.localPosition = originalPosition + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraTransform.localPosition = originalPosition;
    }

    public void HealPlayer(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthBar.SetHealth(currentHealth);

        if (audioSource != null && healsound != null)
            audioSource.PlayOneShot(healsound);
    }
}
