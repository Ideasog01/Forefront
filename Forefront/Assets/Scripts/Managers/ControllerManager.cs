using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject teleportRay;

    public void DisplayTeleportRay(bool active)
    {
        teleportRay.SetActive(active);
    }
}
