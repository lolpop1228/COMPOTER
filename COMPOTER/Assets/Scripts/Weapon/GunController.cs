using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 500f;
    public float fireRate = 0.5f;
    public Transform muzzlePoint;
    public ParticleSystem muzzleFlash;
    private AudioSource audioSource;
    public AudioClip fireSound;
    private Animator animator;

    private float nextTimeToFire;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

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

        if (animator != null)
        {
            animator.SetTrigger("Fire");
        }

        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }

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
