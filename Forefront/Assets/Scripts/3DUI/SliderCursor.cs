using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderCursor : MonoBehaviour
{
    [SerializeField]
    private float maxZCoord;

    private Transform _hand;

    private TDSlider _slider;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("GameController"))
        {
            _hand = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("GameController"))
        {
            _hand = null;
        }
    }

    private void Awake()
    {
        _slider = this.transform.parent.GetComponent<TDSlider>();
    }

    private void Update()
    {
        if(_hand != null)
        {
            this.transform.position = _hand.position;
            this.transform.localPosition = new Vector3(0, 0, this.transform.localPosition.z);

            float newZ = this.transform.localPosition.z;

            newZ = Mathf.Clamp(newZ, 0, maxZCoord);

            this.transform.localPosition = new Vector3(0, 0, newZ);

            Debug.Log("Moving!");

            _slider.UpdateSlider(this.transform.localPosition.z);
        }
    }
}
