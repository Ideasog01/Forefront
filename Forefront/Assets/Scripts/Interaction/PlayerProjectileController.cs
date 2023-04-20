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

    [Header("Sound Effects")]

    [SerializeField]
    private Sound collisionSound;

    [SerializeField]
    private Sound criticalHitSound;

    [SerializeField]
    private Sound bounceSound;


    [Header("Visual Effects")]

    [SerializeField]
    private VisualEffect collisionVisualEffect;

    [SerializeField]
    private Transform projectileTarget;

    private Rigidbody _projectileRb;

    private Perk[] _perkArray;

    //Bounce Variables

    private Vector3 _lastVelocity;
    private float _curveSpeed;
    private Vector3 _direction;

    private EnemyEntity _ignoreCollisionEnemy;

    public EnemyEntity IgnoreCollisionEnemy
    {
        set { _ignoreCollisionEnemy = value; }
    }

    public Transform ProjectileTarget
    {
        set { projectileTarget = value; }
    }

    private void Awake()
    {
        _perkArray = GameManager.perkManager.perkArray;
    }

    private void Update()
    {
        if (projectileTarget != null)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, projectileTarget.position, Time.deltaTime * projectileMovementSpeed);
            _projectileRb.velocity = Vector3.zero;
        }
    }

    private void LateUpdate()
    {
        _lastVelocity = _projectileRb.velocity;
    }

    public void InitialiseProjectile(Vector3 position, Quaternion rotation) //Reset projectile values
    {
        _projectileRb = this.GetComponent<Rigidbody>();

        this.transform.position = position;
        this.transform.rotation = rotation;

        int[] generalSettingsIndexArray = GameManager.mainLoadout.GeneralSettingsValueArray;

        projectileBlastRadius = GameManager.playerEntity.ProjectileBlastRadius[generalSettingsIndexArray[0]];
        projectileMovementSpeed = GameManager.playerEntity.ProjectileVelocity[generalSettingsIndexArray[1]];
        projectileDamage = GameManager.playerEntity.DamageOutput[generalSettingsIndexArray[4]];

        _projectileRb.velocity = this.transform.forward * Time.fixedDeltaTime * projectileMovementSpeed * 10;

        projectileTarget = null;

        StartCoroutine(DelayDisable());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("EnemyDefault") || collision.collider.CompareTag("EnemyCritical")) //Disable projectile when collision occurs with enemy
        {
            EnemyEntity enemy = collision.transform.parent.GetComponent<EnemyEntity>();

            if (_ignoreCollisionEnemy != null)
            {
                if (_ignoreCollisionEnemy == enemy)
                {
                    return;
                }
            }

            PerkEffects(enemy);

            if(collision.collider.CompareTag("EnemyCritical"))
            {
                enemy.TakeDamage(projectileDamage * 2);
                GameManager.audioManager.PlaySound(criticalHitSound);
            }
            else
            {
                enemy.TakeDamage(projectileDamage);
            }

            Debug.Log("Enemy Collision!");
            GameManager.audioManager.PlaySound(collisionSound);
            GameManager.visualEffectManager.StartVFX(collisionVisualEffect);

            DamageNearbyEnemies(collision);

            this.gameObject.SetActive(false);
        }
        else if (_perkArray[1] && _projectileRb.velocity.magnitude != 0) //Projectile Bounce Perk
        {
            var speed = Mathf.Abs(_lastVelocity.magnitude) * 2; //Increases the speed on bounce
            _direction = Vector3.Reflect(_lastVelocity.normalized, collision.contacts[0].normal); //Gets the oppositite direction from the hit point
            _projectileRb.velocity = _direction * Mathf.Max(speed, 0); //Assign the new velocity (movement direction)
            Debug.Log("Bounce!");
            GameManager.audioManager.PlaySound(bounceSound);
        }
    }

    private IEnumerator DelayDisable()
    {
        yield return new WaitForSeconds(projectileDuration);
        this.gameObject.SetActive(false);
    }

    private void PerkEffects(EnemyEntity enemy)
    {
        if (_perkArray[0].IsActive)
        {
            enemy.DamageOvertime();
        }

        if (_perkArray[2].IsActive)
        {
        }

        if (_perkArray[3])
        {
            enemy.Disable(1);
        }
    }

    private void DamageNearbyEnemies(Collision collision)
    {
        Collider[] enemyColliders = Physics.OverlapSphere(this.transform.position, 10);

        foreach (Collider collider in enemyColliders)
        {
            if (collider.CompareTag("EnemyDefault") && collider != collision.collider)
            {
                EnemyEntity enemy = collider.transform.parent.GetComponent<EnemyEntity>();

                float distanceToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);

                if(distanceToEnemy < projectileBlastRadius)
                {
                    enemy.TakeDamage(projectileDamage);
                }

                if (_perkArray[2].IsActive)
                {
                    if (enemy.EntityHealth > 0)
                    {
                        PlayerProjectileController projectile = GameManager.spawnManager.SpawnPlayerProjectile(GameManager.playerEntity.plasmaProjectilePrefab, this.transform.position, this.transform.rotation);
                        projectile.IgnoreCollisionEnemy = collision.transform.parent.GetComponent<EnemyEntity>();
                        projectile.ProjectileTarget = enemy.transform;
                    }
                }
            }
        }
    }
}
