using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject teleportRay;

    [SerializeField]
    private GameObject weaponSelectObj;

    [SerializeField]
    private GameObject leftHandControllerObj;

    public void DisplayTeleportRay(bool active)
    {
        teleportRay.SetActive(active);
    }

    public void DisplayWeaponSelect(bool active)
    {
        weaponSelectObj.SetActive(active);
        
        if(active)
        {
            weaponSelectObj.transform.position = leftHandControllerObj.transform.position;
        }
    }
}
