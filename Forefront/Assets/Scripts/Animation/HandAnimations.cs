using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandAnimations : MonoBehaviour
{
    [SerializeField]
    private InputDeviceCharacteristics inputDeviceCharacteristics;

    private Animator _handAnimator;

    private InputDevice _targetDevice;

    private void Start()
    {
        _handAnimator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        if(!_targetDevice.isValid)
        {
            InitialiseHand();
        }
        else
        {
            UpdateHand();
        }   
    }

    private void InitialiseHand()
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics, inputDevices);

        if(inputDevices.Count > 0)
        {
            _targetDevice = inputDevices[0];

            Debug.Log("Device found!");
        }
    }

    private void UpdateHand()
    {
        if(_targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            _handAnimator.SetFloat("triggerValue", triggerValue);
        }
        else
        {
            _handAnimator.SetFloat("triggerValue", 0);
        }

        if (_targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            _handAnimator.SetFloat("gripValue", gripValue);
        }
        else
        {
            _handAnimator.SetFloat("gripValue", 0);
        }
    }
}
