using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VRButton : MonoBehaviour
{
    [SerializeField]
    private float deadTime = 1.0f;

    [SerializeField]
    private bool _deadTimeActive = false;

    [SerializeField]
    private UnityEvent onPressed;

    [SerializeField]
    private UnityEvent onReleased;

    [SerializeField]
    private float minThreshold;

    [SerializeField]
    private float maxThreshold;

    [SerializeField]
    private Transform pushButton;

    [SerializeField]
    private Material[] buttonMats; //0 = default material, 1 = pressed material

    private void Update()
    {
        if (pushButton.localPosition.y >= maxThreshold)
        {
            pushButton.localPosition = new Vector3(pushButton.localPosition.x, maxThreshold, pushButton.localPosition.z);
        }

        if (pushButton.localPosition.y <= minThreshold)
        {
            pushButton.localPosition = new Vector3(pushButton.localPosition.x, minThreshold, pushButton.localPosition.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Button" && !_deadTimeActive)
        {
            onPressed.Invoke();
            pushButton.GetChild(0).GetComponent<MeshRenderer>().material = buttonMats[1];
            Debug.Log("Button Pressed!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Button" && !_deadTimeActive)
        {
            onReleased.Invoke();
            Debug.Log("Button Released!");
            pushButton.GetChild(0).GetComponent<MeshRenderer>().material = buttonMats[0];
            StartCoroutine(WaitForDeadTime());
        }
    }

    private IEnumerator WaitForDeadTime()
    {
        _deadTimeActive = true;
        yield return new WaitForSeconds(deadTime);
        _deadTimeActive = false;
    }
}
