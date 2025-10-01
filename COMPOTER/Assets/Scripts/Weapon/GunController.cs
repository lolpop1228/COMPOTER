using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    public int maxReserveAmmo = 90; // 🔹 Maximum reserve you can carry
    public float reloadTime = 2f;
    public bool isReloading = false;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    private Animator animator;

    [Header("Camera Recoil Settings")]
    public CameraRecoil cameraRecoil;

    // Expose all recoil variables from CameraRecoil
    public float recoilX = 2f;
    public float recoilY = 1f;
    public float recoilZ = 0.5f;

    public float recoilSpeed = 10f;
    public float returnSpeed = 5f;
    public float maxRecoil = 15f;

    public AnimationCurve recoilCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float recoilDecay = 3f;
    public float randomnessFactor = 0.3f;

    public float weaponRecoilMultiplier = 1f;

    [Header("UI Elements")]
    public TextMeshProUGUI ammoText;
    public GameObject ammmoTextObject;

    private float nextTimeToFire;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        // 🔹 Apply recoil settings to the CameraRecoil script
        if (cameraRecoil != null)
        {
            cameraRecoil.SetWeaponRecoil(recoilX, recoilY, recoilZ, weaponRecoilMultiplier);
            cameraRecoil.recoilSpeed = recoilSpeed;
            cameraRecoil.returnSpeed = returnSpeed;
            cameraRecoil.maxRecoil = maxRecoil;
            cameraRecoil.recoilCurve = recoilCurve;
            cameraRecoil.recoilDecay = recoilDecay;
            cameraRecoil.randomnessFactor = randomnessFactor;
        }
    }

    private void Update()
    {
        if (isReloading) return;

        if (currentAmmo <= 0 && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

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
        UpdateAmmoUI();

        if (animator != null)
            animator.SetTrigger("Fire");

        if (audioSource != null && fireSound != null)
            audioSource.PlayOneShot(fireSound);

        if (muzzleFlash != null)
            muzzleFlash.Play();

        Vector3 spawnPosition = muzzlePoint.position + muzzlePoint.forward * 0.5f;
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, muzzlePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
            rb.velocity = muzzlePoint.forward * bulletSpeed;

        Destroy(bullet, 2f);

        // 🔹 Apply recoil when shooting
        if (cameraRecoil != null)
            cameraRecoil.FireRecoil();
    }

    IEnumerator Reload()
    {
        isReloading = true;

        if (animator != null)
            animator.SetTrigger("Reload");

        if (audioSource != null && reloadSound != null)
            audioSource.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        // 🔹 Ensure reserve never exceeds cap
        reserveAmmo = Mathf.Clamp(reserveAmmo, 0, maxReserveAmmo);

        isReloading = false;
        UpdateAmmoUI();
    }

    public void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = $"{currentAmmo} / {reserveAmmo}";
    }

    void OnEnable()
    {
        if (ammmoTextObject != null)
            ammmoTextObject.SetActive(true);
    }

    void OnDisable()
    {
        if (ammmoTextObject != null)
            ammmoTextObject.SetActive(false);
    }

    // 🔹 Add ammo but clamp to max reserve
    public void AddReserveAmmo(int amount)
    {
        reserveAmmo = Mathf.Min(reserveAmmo + amount, maxReserveAmmo);
        UpdateAmmoUI();
    }
}
