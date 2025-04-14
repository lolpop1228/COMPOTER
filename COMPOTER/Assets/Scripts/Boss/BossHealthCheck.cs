using UnityEngine;

public class BossHealthCheck : MonoBehaviour
{
    public GPUBoss gpuBoss; // Reference to the GPUBoss script
    private bool bossIsDead = false;

    void Update()
    {
        if (gpuBoss == null || bossIsDead) return;

        if (GetCurrentHealth() <= 0f)
        {
            bossIsDead = true;
            OnBossDefeated();
        }
    }

    float GetCurrentHealth()
    {
        // You could expose a public getter in GPUBoss instead, but here's a workaround:
        var healthField = typeof(GPUBoss).GetField("currentHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (float)healthField.GetValue(gpuBoss);
    }

    void OnBossDefeated()
    {
        Debug.Log("Boss has been defeated!");

        // TODO: Add your logic here (cutscene, transition, unlock door, etc.)
    }
}
