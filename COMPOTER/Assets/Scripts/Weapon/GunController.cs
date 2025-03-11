using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 500f;
    public float fireRate = 0.5f;
    public Transform muzzlePoint;
    public ParticleSystem muzzleFlash;

    private float nextTimeToFire;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }    
    }

    void Shoot()
    {
        muzzleFlash.Play();

        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = muzzlePoint.forward * bulletSpeed;
        }

        Destroy(bullet, 2f);
    }
}
