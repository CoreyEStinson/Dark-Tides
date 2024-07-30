using UnityEngine;

public class LighthouseRotation : MonoBehaviour
{
    [Tooltip("Speed at which the light rotates in degrees per second")]
    [SerializeField] private float rotationSpeed = 30f;

    void Update()
    {
        // Rotate around the Y-axis at the specified speed
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
