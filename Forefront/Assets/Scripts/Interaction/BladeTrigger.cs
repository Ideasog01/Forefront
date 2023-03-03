using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "RightHand Controller")
        {
            ControllerManager.bladeInReach = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "RightHand Controller")
        {
            ControllerManager.bladeInReach = false;
        }
    }
}
