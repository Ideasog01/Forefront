using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField]
    private Sound projectileDamageSound;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            ProjectileController projectile = other.gameObject.GetComponent<ProjectileController>();
            GameManager.playerEntity.TakeDamage(projectile.ProjectileDamage);
            GameManager.audioManager.PlaySound(projectileDamageSound);
            other.gameObject.SetActive(false);
        }
    }
}
