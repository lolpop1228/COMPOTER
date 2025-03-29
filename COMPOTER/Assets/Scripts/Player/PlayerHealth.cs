using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Transform cameraTransform;
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;
    public HealthBar healthBar;

    private Vector3 originalCamPosition;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        // Store the initial camera position
        if (cameraTransform != null)
        {
            originalCamPosition = cameraTransform.localPosition;
        }
    }

    public void PlayerTakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthBar.SetHealth(currentHealth);

        if (cameraTransform != null)
        {
            StartCoroutine(ShakeCamera());
        }

        if (currentHealth <= 0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Dead");
        }
    }

    IEnumerator ShakeCamera()
    {
        if (cameraTransform == null) yield break;

        float elapsed = 0f;

        // Shake the camera
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            cameraTransform.localPosition = originalCamPosition + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;

            yield return null;
        }

        // Ensure the camera returns exactly to its original position
        cameraTransform.localPosition = originalCamPosition;
    }

    public void HealPlayer(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthBar.SetHealth(currentHealth);
    }
}
