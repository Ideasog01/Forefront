using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileController : MonoBehaviour
{
    [Header("Projectile Settings")]

    [SerializeField]
    private float projectileBlastRadius;

    [SerializeField]
    private float projectileMovementSpeed;

    [SerializeField]
    private int projectileDamage;

    [Header("General Settings")]

    [SerializeField]
    private float projectileDuration;

    [SerializeField]
    private Sound collisionSound;

    [SerializeField]
    private VisualEffect collisionVisualEffect;

    public void InitialiseProjectile(Vector3 position, Quaternion rotation) //Reset projectile values
    {
        this.transform.position = position;
        this.transform.rotation = rotation;

        int[] generalSettingsIndexArray = GameManager.mainLoadout.GeneralSettingsValueArray;

        projectileBlastRadius = GameManager.playerEntity.ProjectileBlastRadius[generalSettingsIndexArray[0]];
        projectileMovementSpeed = GameManager.playerEntity.ProjectileVelocity[generalSettingsIndexArray[1]];
        projectileDamage = GameManager.playerEntity.DamageOutput[generalSettingsIndexArray[4]];

        StartCoroutine(DelayDisable());
    }

    private void Update()
    {
        ProjectileMovement();
    }

    private void ProjectileMovement()
    {
        this.transform.position += this.transform.forward * Time.deltaTime * projectileMovementSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ProjectileCollision(collision);
    }

    private IEnumerator DelayDisable()
    {
        yield return new WaitForSeconds(projectileDuration);
        this.gameObject.SetActive(false);
    }

    private void ProjectileCollision(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<BaseEntity>().TakeDamage(projectileDamage);
        }

        Collider[] enemyColliders = Physics.OverlapSphere(this.transform.position, projectileBlastRadius);

        foreach(Collider collider in enemyColliders)
        {
            if(collider.CompareTag("Enemy") && collider != collision.collider)
            {
                collider.GetComponent<BaseEntity>().TakeDamage(projectileDamage);
            }
        }

        GameManager.audioManager.PlaySound(collisionSound);
        GameManager.visualEffectManager.StartVFX(collisionVisualEffect);

        this.gameObject.SetActive(false);
    }
}
