using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro
using System.Collections;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public GameObject bulletPrefab;
    public Transform muzzlePoint;
    public float bulletSpeed = 500f;
    public float fireRate = 0.5f;
    public bool isAutomatic;

    [Header("Ammo Settings")]
    public int maxAmmo = 10;
    public int currentAmmo;
    public int reserveAmmo = 30;
    public float reloadTime = 2f;
    public bool isReloading = false;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    private Animator animator;

    [Header("Camera Shake Settings")]
    public Transform cameraTransform;
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;

    [Header("UI Elements")]
    public TextMeshProUGUI ammoText; // Reference for the UI text
    public GameObject ammmoTextObject;

    private float nextTimeToFire;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    private void Update()
    {
        if (isReloading) return;

        if (isAutomatic)
        {
            if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
            {
                if (currentAmmo > 0)
                {
                    nextTimeToFire = Time.time + fireRate;
                    Shoot();
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
            {
                if (currentAmmo > 0)
                {
                    nextTimeToFire = Time.time + fireRate;
                    Shoot();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        if (currentAmmo <= 0) return;

        currentAmmo--;
        UpdateAmmoUI(); // Update UI

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
        StartCoroutine(CameraShake());
    }

    IEnumerator Reload()
    {
        isReloading = true;

        if (animator != null)
        {
            animator.SetTrigger("Reload");
        }

        if (audioSource != null && reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        isReloading = false;
        UpdateAmmoUI(); // Update UI after reloading
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

        cameraTransform.localPosition = originalPosition;
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentAmmo} / {reserveAmmo}";
        }
    }

    void OnEnable()
    {
        ammmoTextObject.SetActive(true);
    }

    void OnDisable()
    {
        ammmoTextObject.SetActive(false);
    }
}
