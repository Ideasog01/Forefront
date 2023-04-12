using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            ProjectileController projectile = other.gameObject.GetComponent<ProjectileController>();
            GameManager.playerEntity.TakeDamage(projectile.ProjectileDamage);
            other.gameObject.SetActive(false);
        }
    }
}
