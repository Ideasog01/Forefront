using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public static bool shieldInReach; //Is a controller inside the blade interaction bounds

    [Header("Teleportation")]

    [SerializeField]
    private GameObject teleportRay;

    [SerializeField]
    private GameObject teleportRecticle;

    [Header("Weapons")]

    [SerializeField]
    private GameObject weaponSelectObj;

    [SerializeField]
    private GameObject plasmaCannonObj;

    [SerializeField]
    private GameObject laserObj;

    [SerializeField]
    private SelectorController selectorController;

    [SerializeField]
    private GameObject shieldObj;

    [Header("General")]

    [SerializeField]
    private GameObject leftHandControllerObj;

    [SerializeField]
    private GameObject rightHandMeshObj;

    [SerializeField]
    private GameObject leftHandMeshObj;

    [Header("Sounds")]

    [SerializeField]
    private Sound swordEquipSound;

    [SerializeField]
    private Sound swordUnequipSound;

    public void DisplayTeleportRay(bool active)
    {
        if(!GameManager.gameInProgress && active)
        {
            return;
        }

        teleportRay.SetActive(active);

        if(teleportRecticle == null)
        {
            teleportRecticle = GameObject.Find("LocomotionRecticle(Clone)");

            if(teleportRecticle != null)
            {
                teleportRecticle.SetActive(active);
            }
        }
        else
        {
            teleportRecticle.SetActive(active);
        }
    }

    public void DisplayWeaponSelect(bool active)
    {
        weaponSelectObj.SetActive(active);
        
        if(active)
        {
            weaponSelectObj.transform.rotation = leftHandControllerObj.transform.rotation;
            weaponSelectObj.transform.position = leftHandControllerObj.transform.position;
        }
        else
        {
            selectorController.ConfirmOption();
        }    
    }

    public void EnablePlasmaCannon(bool active)
    {
        if(!shieldObj.activeSelf)
        {
            plasmaCannonObj.SetActive(active);
            rightHandMeshObj.SetActive(!active);
            PlayerEntity.plasmaActive = active;
        }
    }

    public void EnableLaser(bool active)
    {
        laserObj.SetActive(active);
        leftHandMeshObj.SetActive(!active);
        PlayerEntity.laserActive = active;
    }

    public void EnableBlade(bool active)
    {
        //The blade can only be enabled/disabled when in reach. Meaning that the player's right hand must be behind them and right hand grip is being performed.

        if(shieldInReach && !plasmaCannonObj.activeSelf)
        {
            if (active)
            {
                if(!shieldObj.activeSelf) //Ensures the blade is not already active to avoid playing the equip sound when putting the sword away
                {
                    GameManager.audioManager.PlaySound(swordEquipSound);
                }
            }
            else
            {
                GameManager.audioManager.PlaySound(swordUnequipSound);
            }

            shieldObj.SetActive(active);
            rightHandMeshObj.SetActive(!active);
        }
    }
}
