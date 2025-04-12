using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public GameObject player;
    public PlayerHealth playerHealth;
    public float damage = 100f;
    public float knockBackForce = 70f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = other.transform.position - transform.position;
                direction.y = 1f;

                rb.AddForce(direction.normalized * knockBackForce, ForceMode.Impulse);
            }

            playerHealth.PlayerTakeDamage(damage);
            Debug.Log(other.gameObject.name);
        }
    }

    private void KnockBack(Vector3 direction)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Normalize direction to avoid variable force based on distance
            Vector3 knockback = direction.normalized * knockBackForce;

            // Apply the force as an impulse
            rb.AddForce(knockback, ForceMode.Impulse);
        }
    }
}
