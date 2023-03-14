using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public static bool bladeInReach; //Is a controller inside the blade interaction bounds

    [Header("Teleportation")]

    [SerializeField]
    private GameObject teleportRay;

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
    private GameObject bladeObj;

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
        teleportRay.SetActive(active);
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
        if(!bladeObj.activeSelf)
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

        if(bladeInReach && !plasmaCannonObj.activeSelf)
        {
            if (active)
            {
                if(!bladeObj.activeSelf) //Ensures the blade is not already active to avoid playing the equip sound when putting the sword away
                {
                    GameManager.audioManager.PlaySound(swordEquipSound);
                }
            }
            else
            {
                GameManager.audioManager.PlaySound(swordUnequipSound);
            }

            bladeObj.SetActive(active);
            rightHandMeshObj.SetActive(!active);
        }
    }
}
