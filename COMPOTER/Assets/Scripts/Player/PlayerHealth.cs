using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void PlayerTakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            Debug.Log("Dead");
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth.ToString("F0");
        }
    }
}
