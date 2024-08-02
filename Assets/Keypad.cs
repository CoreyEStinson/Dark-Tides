using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Keypad : MonoBehaviour
{
    public TextMeshProUGUI output; // Use TextMeshProUGUI for UI text
    public TextMeshProUGUI debugOutput; // Text object to display the code for debugging
    private string enteredCode = "";
    private string correctCode;
    private string debugCode = "000"; // Debug code that will always work
    public DoorScript.Door door; // Reference to the Door script
    public AudioSource radioAudioSource; // Reference to the AudioSource component of the radio

    // Ray Interactor fields
    public XRRayInteractor leftRayInteractor;
    public XRRayInteractor rightRayInteractor;
    public BoxCollider triggerZone;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRandomCode();
        EnableRayInteractors(false);
        UpdateOutput();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Method to handle button presses
    public void OnButtonPress(string number)
    {
        if (enteredCode.Length < 3)
        {
            enteredCode += number;
            UpdateOutput();

            if (enteredCode.Length == 3)
            {
                CheckCode();
            }
        }
    }

    // Method to check the entered code
    private void CheckCode()
    {
        if (enteredCode == correctCode) //|| enteredCode == debugCode)
        {
            Debug.Log("Code correct!");
            door.OpenDoor(); // Open the door
            PlayRadio(); // Start playing the radio
        }
        else
        {
            Debug.Log("Code incorrect!");
            StartCoroutine(ResetSystemWithDelay());
        }
    }

    // Coroutine to reset the system with a delay
    private IEnumerator ResetSystemWithDelay()
    {
        yield return new WaitForSeconds(1); // Wait for 1 second
        ResetSystem();
    }

    // Method to reset the system
    private void ResetSystem()
    {
        enteredCode = "";
        UpdateOutput();
    }

    // Method to update the output text
    private void UpdateOutput()
    {
        output.text = enteredCode.PadRight(3, '_');
    }

    // Method to generate a random code
    private void GenerateRandomCode()
    {
        correctCode = Random.Range(100, 1000).ToString(); // Generate a random 3-digit code
        if (debugOutput != null)
        {
            debugOutput.text = "Code for the \r\nstorage room is \r\n" + correctCode; // Display the code for debugging
        }
    }

    // Method to play the radio audio
    private void PlayRadio()
    {
        if (radioAudioSource != null && !radioAudioSource.isPlaying)
        {
            radioAudioSource.Play();
        }
    }

    // Trigger methods to enable/disable ray interactors
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !door.open)
        {
            EnableRayInteractors(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnableRayInteractors(false);
        }
    }

    private void EnableRayInteractors(bool enable)
    {
        if (leftRayInteractor != null)
        {
            leftRayInteractor.enabled = enable;
        }

        if (rightRayInteractor != null)
        {
            rightRayInteractor.enabled = enable;
        }
    }
}
