using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.waveManager.BeginEncounter();
            Debug.Log("Wave Triggered");
            this.gameObject.SetActive(false);
        }
    }
}
