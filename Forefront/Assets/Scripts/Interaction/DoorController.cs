using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator _doorAnimator;

    private void Start()
    {
        _doorAnimator = this.GetComponent<Animator>(); 
    }

    public void DoorInteraction()
    {
        bool isOpen = _doorAnimator.GetBool("open");
        _doorAnimator.SetBool("open", !isOpen); //Set 'open' to the opposite of its current value
    }
}
