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

    [Header("Camera Shake Settings")]
    public Transform cameraTransform;  // Assign your main camera
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;

    //Auto
    public bool isAutomatic;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isAutomatic)
        {
            if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }
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

        StartCoroutine(CameraShake());  // Apply recoil shake
    }

    IEnumerator CameraShake()
    {
        if (cameraTransform == null) yield break;

        Vector3 originalPosition = cameraTransform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            cameraTransform.localPosition = originalPosition + new Vector3(x, y, 0);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        cameraTransform.localPosition = originalPosition;  // Reset camera position
    }
}
