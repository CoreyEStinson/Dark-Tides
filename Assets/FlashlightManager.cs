using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class FlashlightManager : MonoBehaviour
{
    [SerializeField] private float toggleDelay = 0.5f;
    public Light flashlight; // Assign your flashlight object in the inspector
    public AudioClip toggleSound; // Assign your audio clip in the inspector
    private bool isFlashlightOn = false;
    private XRNode controllerNode = XRNode.RightHand; // Use the right hand controller
    private bool canToggle = true; // Track if the flashlight can be toggled
    public ImageFillManager imageFillManager; // Assign your ImageFillManager object in the inspector

    void Update()
    {
        // Check if the B button is pressed on the right hand controller
        if (canToggle && IsOculusButtonPressed())
        {
            ToggleFlashlight();
            StartCoroutine(ToggleCooldown());
        }
    }
    private void Start()
    {
        if (flashlight != null)
        {
            flashlight.enabled = isFlashlightOn;
        }
    }
    
    private bool IsOculusButtonPressed()
    {
        InputDevice controller = InputDevices.GetDeviceAtXRNode(controllerNode);
        bool isPressed = false;
        controller.TryGetFeatureValue(CommonUsages.secondaryButton, out isPressed); // "B" button is the secondary button
        return isPressed;
    }

    private IEnumerator ToggleCooldown()
    {
        canToggle = false;
        yield return new WaitForSeconds(toggleDelay);
        canToggle = true;
    }

    private void ToggleFlashlight()
    {
        if (flashlight != null)
        {
            isFlashlightOn = !isFlashlightOn;
            flashlight.enabled = isFlashlightOn;
            if (toggleSound != null)
            {
                AudioSource.PlayClipAtPoint(toggleSound, flashlight.transform.position);
            }

            if (isFlashlightOn)
            {
                imageFillManager.StartFilling();
            }
            else
            {
                imageFillManager.StopFilling();
            }
        }
    }
}
