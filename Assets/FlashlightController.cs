using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction;

[AddComponentMenu("Nokobot/Modern Guns/Flashlight Controller")]
public class FlashlightController : MonoBehaviour
{
    [Header("Flashlight Settings")]
    [Tooltip("The spotlight object representing the flashlight")]
    [SerializeField] private Light flashlight;

    [Header("Toggle Settings")]
    [Tooltip("Delay in seconds between toggles")]
    [SerializeField] private float toggleDelay = 0.5f;

    [Header("Sound Settings")]
    [Tooltip("Sound effect for toggling the flashlight")]
    [SerializeField] private AudioClip toggleSound;

    [Header("Battery Settings")]
    [Tooltip("UI Image representing the battery fill")]
    [SerializeField] private Image batteryFillImage;
    [Tooltip("Rate at which the battery drains when the flashlight is on")]
    [SerializeField] private float batteryDrainRate = 0.1f;

    private XRNode controllerNode = XRNode.RightHand; // Use XRNode.LeftHand if you want to use the left controller
    public static bool isFlashlightOn = true; // Track if the flashlight is on
    private bool canToggle = true; // Track if the flashlight can be toggled
    private AudioSource audioSource;

    void Start()
    {
        if (flashlight == null)
        {
            Debug.LogError("Flashlight object is not assigned!");
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        if (toggleSound != null)
        {
            audioSource.clip = toggleSound;
        }

        if (batteryFillImage == null)
        {
            Debug.LogError("Battery fill image is not assigned!");
        }
    }

    void Update()
    {
        if (canToggle && IsOculusButtonPressed())
        {
            ToggleFlashlight();
            StartCoroutine(ToggleCooldown());
        }

        if (isFlashlightOn && batteryFillImage != null)
        {
            DrainBattery();
        }
    }

    private bool IsOculusButtonPressed()
    {
        InputDevice controller = InputDevices.GetDeviceAtXRNode(controllerNode);
        bool isPressed = false;
        controller.TryGetFeatureValue(CommonUsages.secondaryButton, out isPressed); // "B" button is the secondary button
        return isPressed;
    }

    private void ToggleFlashlight()
    {
        if (flashlight != null)
        {
            isFlashlightOn = !isFlashlightOn;
            flashlight.enabled = isFlashlightOn;

            if (audioSource != null && toggleSound != null)
            {
                audioSource.Play();
            }
        }
    }

    private void DrainBattery()
    {
        batteryFillImage.fillAmount -= batteryDrainRate * Time.deltaTime;
        if (batteryFillImage.fillAmount <= 0)
        {
            TurnOffFlashlight();
        }
    }

    public static void TurnOffFlashlight()
    {
        isFlashlightOn = false;
        // Additional code to handle turning off the flashlight
    }

    private IEnumerator ToggleCooldown()
    {
        canToggle = false;
        yield return new WaitForSeconds(toggleDelay);
        canToggle = true;
    }
}
