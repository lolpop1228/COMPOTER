using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactEffect;
    public float damage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet collided with: " + collision.gameObject.name);
        Target target = collision.gameObject.GetComponent<Target>();
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (target != null )
        {
            target.TakeDamage(damage);
        }

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }

        Destroy(gameObject);
    }
}
