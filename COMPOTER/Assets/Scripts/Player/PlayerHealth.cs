using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public TextMeshProUGUI healthText;
    public Transform cameraTransform;
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;
    public HealthBar healthBar;

    private Vector3 originalCamPostion;

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
        Vector3 originalPosition = cameraTransform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f,1f) * shakeMagnitude;
            float y = Random.Range(-1f,1f) * shakeMagnitude;

            cameraTransform.localPosition = originalPosition + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraTransform.localPosition = originalPosition;
    }
}
