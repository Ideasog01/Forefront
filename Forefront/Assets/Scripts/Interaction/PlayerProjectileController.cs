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
    private Transform seekerProjectilePrefab;

    [SerializeField]
    private float projectileDuration;

    [Header("Sound Effects")]

    [SerializeField]
    private Sound collisionSound;

    [SerializeField]
    private Sound criticalHitSound;

    [SerializeField]
    private Sound bounceSound;

    private Transform _projectTarget;

    public Transform ProjectileTarget
    {
        set { _projectTarget = value; }
    }


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

            PerkEffects(enemy); //These perks interact with the enemy directly

            if(collision.collider.CompareTag("EnemyCritical"))
            {
                int multiplier = 0; //The multplier for the critical hit damage.

                if (_perkArray[4].IsActive) //Critical hits deal more damage perk
                {
                    multiplier = 3;
                }
                else
                {
                    multiplier = 2;
                }

                enemy.TakeDamage(projectileDamage * multiplier);
                GameManager.audioManager.PlaySound(criticalHitSound); //Play a critical hit sound for player feedback

                if(enemy.EntityHealth <= 0 && _perkArray[2].IsActive) //Releases projectiles that find and track nearby enemies
                {
                    SeekerProjectiles(collision);
                }
            }
            else
            {
                enemy.TakeDamage(projectileDamage);

                if (enemy.EntityHealth <= 0 && _perkArray[2].IsActive)
                {
                    SeekerProjectiles(collision);
                }
            }

            Debug.Log("Enemy Collision!");
            GameManager.audioManager.PlaySound(collisionSound);
            GameManager.visualEffectManager.StartVFX(collisionVisualEffect);

            DamageNearbyEnemies(collision);

            this.gameObject.SetActive(false);
        }
        else if (_perkArray[1].IsActive && _projectileRb.velocity.magnitude != 0) //Projectile Bounce Perk
        {
            var speed = Mathf.Abs(_lastVelocity.magnitude);
            _direction = Vector3.Reflect(_lastVelocity.normalized, collision.contacts[0].normal); //Gets the oppositite direction from the hit point
            _projectileRb.velocity = _direction * Mathf.Max(speed, 0); //Assign the new velocity (movement direction)
            Debug.Log("Bounce!");
            GameManager.audioManager.PlaySound(bounceSound);
        }
        else
        {
            GameManager.audioManager.PlaySound(collisionSound);
            GameManager.visualEffectManager.StartVFX(collisionVisualEffect);

            DamageNearbyEnemies(collision);

            this.gameObject.SetActive(false);
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
            }
        }
    }

    private void SeekerProjectiles(Collision collision)
    {
        Collider[] enemyColliders = Physics.OverlapSphere(this.transform.position, 10);

        foreach (Collider collider in enemyColliders)
        {
            if (collider.CompareTag("EnemyDefault") && collider != collision.collider)
            {
                EnemyEntity enemy = collider.transform.parent.GetComponent<EnemyEntity>();

                if (enemy.EntityHealth > 0)
                {
                    ProjectileController projectile = GameManager.spawnManager.SpawnSeekerProjectile(seekerProjectilePrefab, this.transform.position, this.transform.rotation);
                    projectile.IgnoreCollisionEnemy = collision.transform.parent.GetComponent<EnemyEntity>();
                    projectile.ProjectileTarget = enemy.transform;
                }
            }
        }
    }
}
