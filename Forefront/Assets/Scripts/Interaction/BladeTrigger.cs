using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "LeftHand Controller")
        {
            ControllerManager.shieldInReach = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "LeftHand Controller")
        {
            ControllerManager.shieldInReach = false;
        }
    }
}
