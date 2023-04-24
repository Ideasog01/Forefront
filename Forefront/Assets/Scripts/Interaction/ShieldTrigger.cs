using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "LeftHand Controller")
        {
            ControllerManager.shieldInReach = true;
            Debug.Log("Shield in reach");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "LeftHand Controller")
        {
            ControllerManager.shieldInReach = false;
            Debug.Log("Shield NOT in reach");
        }
    }
}
