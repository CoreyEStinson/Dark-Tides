using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location References")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destroy the casing object")][SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")][SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")][SerializeField] private float ejectPower = 150f;
    [Tooltip("Delay in seconds between shots")][SerializeField] private float shotDelay = 0.5f;

    [Header("Sound Settings")]
    [Tooltip("Sound effect for shooting the gun")]
    [SerializeField] private AudioClip shootSound;

    private XRNode controllerNode = XRNode.RightHand; // Use XRNode.LeftHand if you want to use the left controller
    private bool canShoot = true; // Track if the gun can shoot
    private AudioSource audioSource;

    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        audioSource = gameObject.AddComponent<AudioSource>();
        if (shootSound != null)
        {
            audioSource.clip = shootSound;
        }
    }

    void Update()
    {
        // If you want a different input, change it here
        if (canShoot && IsOculusButtonPressed())
        {
            // Calls animation on the gun that has the relevant animation events that will fire
            gunAnimator.SetTrigger("Fire");
            Shoot();
            CasingRelease();
            StartCoroutine(ShootCooldown());
        }
    }

    private bool IsOculusButtonPressed()
    {
        InputDevice controller = InputDevices.GetDeviceAtXRNode(controllerNode);
        bool isPressed = false;
        controller.TryGetFeatureValue(CommonUsages.primaryButton, out isPressed); // "A" button is the primary button
        return isPressed;
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shotDelay);
        canShoot = true;
    }

    // This function creates the bullet behavior
    void Shoot()
    {
        if (muzzleFlashPrefab)
        {
            // Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            // Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        // Cancels if there's no bullet prefab
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

        // Play shooting sound
        if (audioSource != null && shootSound != null)
        {
            audioSource.Play();
        }
    }

    // This function creates a casing at the ejection slot
    void CasingRelease()
    {
        // Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        // Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        // Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        // Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        // Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }
}
