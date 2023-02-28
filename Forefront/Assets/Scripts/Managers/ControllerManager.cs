using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
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

    [Header("General")]

    [SerializeField]
    private GameObject leftHandControllerObj;

    [SerializeField]
    private GameObject rightHandMeshObj;

    [SerializeField]
    private GameObject leftHandMeshObj;

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
        plasmaCannonObj.SetActive(active);
        rightHandMeshObj.SetActive(!active);
        PlayerEntity.plasmaActive = active;
    }

    public void EnableLaser(bool active)
    {
        laserObj.SetActive(active);
        leftHandMeshObj.SetActive(!active);
        PlayerEntity.laserActive = active;
    }

    public void EnableBlade(bool active)
    {

    }
}
