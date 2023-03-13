using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private bool isMainMenu;

    private XRIDefaultInputActions customInput;

    private void Awake()
    {
        InitialiseInput();
    }

    private void InitialiseInput()
    {
        customInput = new XRIDefaultInputActions();

        if(!isMainMenu)
        {
            customInput.XRILeftHandInteraction.WeaponSelectMenu.started += ctx => GameManager.controllerManager.DisplayWeaponSelect(true); //The left hand primary button
            customInput.XRILeftHandInteraction.WeaponSelectMenu.canceled += ctx => GameManager.controllerManager.DisplayWeaponSelect(false); //The left hand primary button

            customInput.XRILeftHandLocomotion.TeleportModeActivate.started += ctx => GameManager.controllerManager.DisplayTeleportRay(true); //The left hand stick forward
            customInput.XRILeftHandLocomotion.TeleportModeActivate.canceled += ctx => GameManager.controllerManager.DisplayTeleportRay(false); //The left hand stick forward

            customInput.XRILeftHandInteraction.Activate.started += ctx => GameManager.playerEntity.FireLaser(true); //The left hand trigger
            customInput.XRILeftHandInteraction.Activate.canceled += ctx => GameManager.playerEntity.FireLaser(false); //The left hand trigger

            customInput.XRIRightHandInteraction.Select.started += ctx => GameManager.controllerManager.EnableBlade(true);
            customInput.XRIRightHandInteraction.Select.canceled += ctx => GameManager.controllerManager.EnableBlade(false);
        }

        customInput.XRIRightHandInteraction.Activate.started += ctx => GameManager.playerEntity.FireCannon(false); //The right hand trigger
        customInput.XRIRightHandInteraction.Activate.canceled += ctx => GameManager.playerEntity.FireCannon(true); //The right hand trigger
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
