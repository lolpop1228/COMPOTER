using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pistol : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 500f;
    public float fireRate = 0.3f;

    private AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip reloadSound;

    private Animator gunAnimator;

    public int magazineSize = 9;
    public int totalAmmo = 27;
    private int currentBulletAmount;
    private bool isReloading;

    public TextMeshProUGUI bulletText;
    public TextMeshProUGUI totalAmmoText;

    bool readyToShoot = true;
    public float reloadTime = 2.0f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gunAnimator = GetComponent<Animator>();
        currentBulletAmount = magazineSize;

        UpdateBulletUI();
    }

    private void Update()
    {
        if (isReloading) return;

        if (Input.GetMouseButtonDown(0) && readyToShoot && currentBulletAmount > 0)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentBulletAmount < magazineSize && totalAmmo > 0)
        {
            StartCoroutine(Reload());
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        currentBulletAmount--;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(shootPoint.forward * bulletSpeed, ForceMode.Impulse);
        }

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("Fire");
        }

        UpdateBulletUI();
        Invoke(nameof(ResetShot), fireRate);
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        readyToShoot = false;

        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("Reload");
        }

        if (audioSource != null && reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadTime);

        int bulletsNeeded = magazineSize - currentBulletAmount;
        int bulletsToReload = Mathf.Min(bulletsNeeded, totalAmmo);

        currentBulletAmount += bulletsToReload;
        totalAmmo -= bulletsToReload;

        isReloading = false;
        readyToShoot = true;

        UpdateBulletUI();
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void UpdateBulletUI()
    {
        if (bulletText != null)
        {
            bulletText.text = currentBulletAmount.ToString();
        }

        if (totalAmmoText != null)
        {
            totalAmmoText.text = totalAmmo.ToString();
        }
    }

    private void OnEnable()
    {
        if (gunAnimator != null)
        {
            gunAnimator.ResetTrigger("Fire");
            gunAnimator.ResetTrigger("Reload");

            gunAnimator.Play("Idle", 0, 0f);
            gunAnimator.Update(0.1f); // Force the animator to refresh the state
        }

        isReloading = false;
        readyToShoot = true;
    }
}
