using UnityEngine;
using UnityEngine.UI;

public class ScrollBarChecker : MonoBehaviour
{
    public Scrollbar scrollbar1;
    public Scrollbar scrollbar2;
    public Scrollbar scrollbar3;
    public Scrollbar scrollbar4;
    public AudioSource audioSource; // Add this line

    // Add references to the box objects and the lightbulb
    public GameObject box1;
    public GameObject box2;
    public GameObject lightbulb;

    public bool puzzleSolved = false;

    // Define target ranges for each scroll bar (values between 0 and 1)
    private (float Min, float Max) targetRange1 = (0.167f, 0.499f);
    private (float Min, float Max) targetRange2 = (0.0f, 0.166f);
    private (float Min, float Max) targetRange3 = (0.167f, 0.499f);
    private (float Min, float Max) targetRange4 = (0.834f, 1f);

    public LightbulbPlacement lightbulbPlacement;

    void Start()
    {
        scrollbar1.onValueChanged.AddListener(delegate { CheckScrollBars(); });
        scrollbar2.onValueChanged.AddListener(delegate { CheckScrollBars(); });
        scrollbar3.onValueChanged.AddListener(delegate { CheckScrollBars(); });
        scrollbar4.onValueChanged.AddListener(delegate { CheckScrollBars(); });

        if (box1 != null) box1.SetActive(!true);
        if (box2 != null) box2.SetActive(!false);
        if (lightbulb != null) lightbulb.SetActive(!true);
    }

    private void CheckScrollBars()
    {
        if (IsWithinRange(scrollbar1.value, targetRange1) &&
            IsWithinRange(scrollbar2.value, targetRange2) &&
            IsWithinRange(scrollbar3.value, targetRange3) &&
            IsWithinRange(scrollbar4.value, targetRange4))
        {
            // All scroll bars are within their target ranges
            Debug.Log("All scroll bars are in the right positions!");
            PlayAudio(); // Add this line
            ActivateObjects(); // Add this line
            puzzleSolved = true;
            lightbulbPlacement.hasBulb = true;
        }
    }

    private bool IsWithinRange(float value, (float Min, float Max) range)
    {
        return value >= range.Min && value <= range.Max;
    }

    private void PlayAudio() // Add this method
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void ActivateObjects() // Add this method
    {
        if (box1 != null) box1.SetActive(true);
        if (box2 != null) box2.SetActive(false);
        //if (lightbulb != null) lightbulb.SetActive(true);
    }
}
