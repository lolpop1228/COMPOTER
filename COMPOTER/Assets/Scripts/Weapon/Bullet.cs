using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactEffect;
    public float damage = 10f;

    private void Start()
    {
        Destroy(gameObject ,5f);

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
        Target target = collision.gameObject.GetComponent<Target>();
        ProjectileEnemy enemy = collision.gameObject.GetComponent<ProjectileEnemy>();
        MeleeEnemy meleeEnemy = collision.gameObject.GetComponent<MeleeEnemy>();

        if (target != null )
        {
            target.TakeDamage(damage);
        }

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        if (meleeEnemy != null)
        {
            meleeEnemy.TakeDamage(damage);
        }

        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }

        Destroy(gameObject);
    }
}
