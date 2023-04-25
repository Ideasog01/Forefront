using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeController : MonoBehaviour
{
    [SerializeField]
    private int bladeDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyDefault"))
        {
            other.transform.parent.GetComponent<EnemyEntity>().TakeDamage(bladeDamage);
        }
    }
}
