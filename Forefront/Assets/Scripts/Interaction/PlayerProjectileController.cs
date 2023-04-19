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
    private Sound criticalHitSound;

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
        Perk fireProtocol = GameManager.perkManager.fireProtocol; //Damages enemy overtime

        if (collision.collider.CompareTag("EnemyDefault"))
        {
            EnemyEntity enemy = collision.transform.parent.GetComponent<EnemyEntity>();
            enemy.TakeDamage(projectileDamage);

            if(fireProtocol.IsActive)
            {
                enemy.DamageOvertime();
            }
        }

        if (collision.collider.CompareTag("EnemyCritical"))
        {
            EnemyEntity enemy = collision.transform.parent.GetComponent<EnemyEntity>();
            enemy.TakeDamage(projectileDamage * 2);
            GameManager.audioManager.PlaySound(criticalHitSound);

            if (fireProtocol.IsActive)
            {
                enemy.DamageOvertime();
            }
        }

        Collider[] enemyColliders = Physics.OverlapSphere(this.transform.position, projectileBlastRadius);

        foreach (Collider collider in enemyColliders)
        {
            if (collider.CompareTag("Enemy") && collider != collision.collider)
            {
                EnemyEntity enemy = collider.GetComponent<EnemyEntity>();
                enemy.TakeDamage(projectileDamage);

                if (fireProtocol.IsActive)
                {
                    enemy.DamageOvertime();
                }
            }
        }

        GameManager.audioManager.PlaySound(collisionSound);
        GameManager.visualEffectManager.StartVFX(collisionVisualEffect);

        this.gameObject.SetActive(false);
    }

    private IEnumerator DelayDisable()
    {
        yield return new WaitForSeconds(projectileDuration);
        this.gameObject.SetActive(false);
    }
}
