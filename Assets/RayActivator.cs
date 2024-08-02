using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayActivator : MonoBehaviour
{

    public XRRayInteractor leftRayInteractor;
    public XRRayInteractor rightRayInteractor;
    public Canvas canvas;
    public ScrollBarChecker scrollBarChecker;

    private void Start()
    {
        canvas.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !scrollBarChecker.puzzleSolved)
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
        if (canvas != null)
        canvas.enabled = enable;
    }
    
}
