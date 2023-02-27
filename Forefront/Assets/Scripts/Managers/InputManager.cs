using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private XRIDefaultInputActions customInput;

    private void Awake()
    {
        InitialiseInput();
    }

    private void InitialiseInput()
    {
        customInput = new XRIDefaultInputActions();
        customInput.XRILeftHandLocomotion.TeleportModeActivate.started += ctx => GameManager.controllerManager.DisplayTeleportRay(true);
        customInput.XRILeftHandLocomotion.TeleportModeActivate.canceled += ctx => GameManager.controllerManager.DisplayTeleportRay(false);
    }

    private void OnEnable()
    {
        customInput.Enable();
    }

    private void OnDisable()
    {
        customInput.Disable();
    }
}
