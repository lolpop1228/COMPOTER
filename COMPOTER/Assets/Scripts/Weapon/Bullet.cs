using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactEffect;
    public float damage = 10f;

    private void Start()
    {
        Destroy(gameObject, 5f);

        // Ignore collision with the player
        GameObject player = GameObject.Find("PlayerController");
        if (player != null)
        {
            Collider playerCollider = player.GetComponent<Collider>();
            Collider bulletCollider = GetComponent<Collider>();

            if (playerCollider != null && bulletCollider != null)
            {
                Physics.IgnoreCollision(bulletCollider, playerCollider);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet collided with: " + collision.gameObject.name);
        
        // Find the second-highest parent in hierarchy
        Transform secondHighestParent = FindSecondHighestParent(collision.transform);
        
        // Check for all possible target types
        CheckForTargets(collision.gameObject, secondHighestParent.gameObject);

        // Spawn impact effect
        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }

        Destroy(gameObject);
    }

    private Transform FindSecondHighestParent(Transform childTransform)
    {
        Transform current = childTransform;
        Transform parent = current.parent;
        
        // If no parent or only one parent exists, return the original transform
        if (parent == null || parent.parent == null)
            return current;
        
        // Walk up the hierarchy until we find the second-highest parent
        while (parent.parent != null)
        {
            current = parent;
            parent = parent.parent;
        }
        
        return current;
    }

    private void CheckForTargets(GameObject immediateCollision, GameObject secondHighestParent)
    {
        // Check immediate collision first
        Target target = immediateCollision.GetComponent<Target>();
        ProjectileEnemy enemy = immediateCollision.GetComponent<ProjectileEnemy>();
        TeleportEnemy teleportEnemy = immediateCollision.GetComponent<TeleportEnemy>();
        PSUBoss pSUBoss = immediateCollision.GetComponent<PSUBoss>();
        GPUBoss gPUBoss= immediateCollision.GetComponent<GPUBoss>();
        HomingTurret homingTurret = immediateCollision.GetComponent<HomingTurret>();

        // If nothing found on immediate collision, check second-highest parent
        if (target == null && enemy == null && teleportEnemy == null && pSUBoss == null && gPUBoss == null && homingTurret == null)
        {
            target = secondHighestParent.GetComponent<Target>();
            enemy = secondHighestParent.GetComponent<ProjectileEnemy>();
            teleportEnemy = secondHighestParent.GetComponent<TeleportEnemy>();
            pSUBoss = secondHighestParent.GetComponent<PSUBoss>();
            gPUBoss = secondHighestParent.GetComponent<GPUBoss>();
            homingTurret = secondHighestParent.GetComponent<HomingTurret>();
        }

        // Apply damage to whatever we found
        if (target != null) target.TakeDamage(damage);
        if (enemy != null) enemy.TakeDamage(damage);
        if (teleportEnemy != null) teleportEnemy.TakeDamage(damage);
        if (pSUBoss != null) pSUBoss.TakeDamage(damage);
        if (gPUBoss != null) gPUBoss.TakeDamage(damage);
        if (homingTurret != null) homingTurret.TakeDamage(damage);
    }
}