using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveAfterTime : MonoBehaviour
{
    [SerializeField]
    private float duration;

    private void OnEnable()
    {
        StartCoroutine(InactiveDelay());
    }

    private IEnumerator InactiveDelay()
    {
        yield return new WaitForSeconds(duration);
        this.gameObject.SetActive(false);
    }
}
