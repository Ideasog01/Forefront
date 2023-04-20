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

        StartCoroutine(DelayDisable());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("EnemyDefault"))
        {
            EnemyEntity enemy = collision.transform.parent.GetComponent<EnemyEntity>();
            enemy.TakeDamage(projectileDamage);

            if (_perkArray[0].IsActive)
            {
                enemy.DamageOvertime();
            }
        }

        if (collision.collider.CompareTag("EnemyCritical"))
        {
            EnemyEntity enemy = collision.transform.parent.GetComponent<EnemyEntity>();
            enemy.TakeDamage(projectileDamage * 2);
            GameManager.audioManager.PlaySound(criticalHitSound);

            if (_perkArray[0].IsActive)
            {
                enemy.DamageOvertime();
            }
        }

        Collider[] enemyColliders = Physics.OverlapSphere(this.transform.position, projectileBlastRadius);

        foreach (Collider collider in enemyColliders)
        {
            if (collider.CompareTag("EnemyDefault") && collider != collision.collider)
            {
                EnemyEntity enemy = collider.GetComponent<EnemyEntity>();
                enemy.TakeDamage(projectileDamage);

                if (_perkArray[0].IsActive)
                {
                    enemy.DamageOvertime();
                }
            }
        }

        if (collision.collider.CompareTag("EnemyDefault") || collision.collider.CompareTag("EnemyCritical"))
        {
            Debug.Log("Enemy Collision!");

            GameManager.audioManager.PlaySound(collisionSound);
            GameManager.visualEffectManager.StartVFX(collisionVisualEffect);
            this.gameObject.SetActive(false);
        }
        else if (_perkArray[1])//Don't bounce if the collision was with an enemy
        {
            var speed = Mathf.Abs(_lastVelocity.magnitude) * 2; //Increases the speed on bounce
            _direction = Vector3.Reflect(_lastVelocity.normalized, collision.contacts[0].normal); //Gets the oppositite direction from the hit point
            _projectileRb.velocity = _direction * Mathf.Max(speed, 0); //Assign the new velocity (movement direction)
            Debug.Log("Bounce!");

            GameManager.audioManager.PlaySound(bounceSound);
        }
    }

    private void LateUpdate()
    {
        _lastVelocity = _projectileRb.velocity;
    }

    private IEnumerator DelayDisable()
    {
        yield return new WaitForSeconds(projectileDuration);
        this.gameObject.SetActive(false);
    }
}
