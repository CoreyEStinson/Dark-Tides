using UnityEngine;
using UnityEngine.UI;

public class ImageFillManager : MonoBehaviour
{
    [SerializeField] private Image imageToFill;
    [SerializeField] private float fillSpeed = 0.01f;
    [SerializeField] private float refillAmount = 0.5f; // Amount to refill when battery collides
    private bool isFilling = false;

    void Update()
    {
        if (isFilling && imageToFill != null)
        {
            imageToFill.fillAmount -= fillSpeed * Time.deltaTime;
            if (imageToFill.fillAmount <= 0)
            {
                isFilling = false;
            }
        }
    }

    public void StartFilling()
    {
        isFilling = true;
    }

    public void StopFilling()
    {
        isFilling = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            Refill();
            Destroy(other.gameObject); // Optionally destroy the battery object
        }
    }

    private void Refill()
    {
        if (imageToFill != null)
        {
            imageToFill.fillAmount = Mathf.Clamp(imageToFill.fillAmount + refillAmount, 0, 1);
        }
    }
}
