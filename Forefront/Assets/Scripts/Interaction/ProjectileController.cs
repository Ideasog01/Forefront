using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private float projectileMovementSpeed;

    [SerializeField]
    private int projectileDamage;

    [SerializeField]
    private ProjectileType projectileType;

    [SerializeField]
    private float projectileDuration;

    [SerializeField]
    private VisualEffect destroyVfx;

    [SerializeField]
    private Sound destroySound;

    public enum ProjectileType { PlayerProjectile, DroneProjectile, TankProjectile };

    public ProjectileType ProjectileTypeRef
    {
        get { return projectileType; }
    }

    public int ProjectileDamage
    {
        get { return projectileDamage; }
    }

    private EnemyEntity _ignoreCollisionEnemy;

    private Transform _projectileTarget;

    public EnemyEntity IgnoreCollisionEnemy
    {
        set { _ignoreCollisionEnemy = value; }
    }

    public Transform ProjectileTarget
    {
        set { _projectileTarget = value; }
    }

    public void InitialiseProjectile(Vector3 position, Quaternion rotation) //Reset projectile values
    {
        this.transform.position = position;
        this.transform.rotation = rotation;
        _projectileTarget = null;
        StartCoroutine(DelayDisable());
    }

    private void Update()
    {
        ProjectileMovement();
    }

    private void ProjectileMovement()
    {
        if (_projectileTarget != null)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _projectileTarget.position, Time.deltaTime * projectileMovementSpeed);
        }
        else
        {
            this.transform.position += this.transform.forward * Time.deltaTime * projectileMovementSpeed;
        }
    }

    private IEnumerator DelayDisable()
    {
        yield return new WaitForSeconds(projectileDuration);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(projectileType != ProjectileType.PlayerProjectile)
        {
            if (other.gameObject.CompareTag("Shield"))
            {
                other.GetComponent<ShieldController>().DamageShield(projectileDamage);
            }

            if (!other.gameObject.CompareTag("Player"))
            {
                GameManager.visualEffectManager.StartVFX(destroyVfx);
                GameManager.audioManager.PlaySound(destroySound);
            }
        }
        else
        {
            if(other.gameObject.CompareTag("EnemyDefault"))
            {
                if (_ignoreCollisionEnemy != null)
                {
                    if (_ignoreCollisionEnemy == other.transform.parent.GetComponent<EnemyEntity>())
                    {
                        return;
                    }
                }
            }

            if (other.gameObject.CompareTag("EnemyDefault"))
            {
                other.transform.parent.GetComponent<EnemyEntity>().TakeDamage(projectileDamage);
            }
            else if(other.gameObject.CompareTag("EnemyCritical"))
            {
                other.transform.parent.GetComponent<EnemyEntity>().TakeDamage(projectileDamage * 2);
            }

            GameManager.visualEffectManager.StartVFX(destroyVfx);
            GameManager.audioManager.PlaySound(destroySound);
        }

        this.gameObject.SetActive(false);
    }
}
