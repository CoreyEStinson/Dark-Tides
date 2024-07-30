using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Animator))]
public class AnimateHandController : MonoBehaviour
{
    public InputActionReference gripInputActionRefrence;
    public InputActionReference triggerInputActionRefrence;

    private Animator _handAnimator;
    private float _gripValue;
    private float _triggerValue;

    // Start is called before the first frame update
    void Start()
    {
        _handAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateGrip();
        AnimateTrigger();
    }

    private void AnimateGrip()
    {
        _gripValue = gripInputActionRefrence.action.ReadValue<float>();
        _handAnimator.SetFloat("Grip", _gripValue);
    }
    private void AnimateTrigger()
    {
        _triggerValue = triggerInputActionRefrence.action.ReadValue<float>();
        _handAnimator.SetFloat("Trigger", _triggerValue);
    }
}
