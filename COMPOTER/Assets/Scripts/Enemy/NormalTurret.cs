using System.Collections;
using UnityEngine;

public class NormalTurret : MonoBehaviour
{
    [Header("Turret Settings")]
    public float detectionRange = 15f;
    public float rotationSpeed = 5f;
    public float fireRate = 1f;
    private float nextFireTime;

    [Header("References")]
    public Transform target;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

    private void Update()
    {
        FindTarget();

        if (target != null)
        {
            RotateTowardsTarget();

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    void FindTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= detectionRange)
            {
                target = player.transform;
            }
            else
            {
                target = null;
            }
        }
    }

    void RotateTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * bulletSpeed;
            }
        }
    }
}
