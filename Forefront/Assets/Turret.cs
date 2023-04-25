using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret Properties")]

    [SerializeField]
    private float attackThreshold;

    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private Transform projectilePrefab;

    [SerializeField]
    private Transform projectileSpawn;

    [Header("Other")]

    [SerializeField]
    private Transform turretTop;

    [SerializeField]
    private LayerMask enemyLayerMask;

    [Header("Effects")]

    [SerializeField]
    private VisualEffect fireEffect;

    [SerializeField]
    private Sound fireSound;

    private EnemyEntity _nearestEnemy;

    private SpawnManager _spawnManager;

    private bool _hasAttacked;

    private void Start()
    {
        _spawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();
    }

    private void Update()
    {
        FindNearestEnemy();
        LookAtNearestEnemy();
        Attack();
    }

    private void FindNearestEnemy()
    {
        foreach(EnemyEntity enemy in _spawnManager.enemyList)
        {
            if(enemy.isActiveAndEnabled)
            {
                float distance = Vector3.Distance(this.transform.position, enemy.transform.position);

                if(distance < attackThreshold)
                {
                    RaycastHit hit;

                    if(Physics.Raycast(this.transform.position, enemy.transform.position, out hit, attackThreshold, enemyLayerMask))
                    {
                        if(hit.collider.CompareTag("EnemyDefault"))
                        {
                            _nearestEnemy = enemy;
                        }
                    }
                }
            }
        }
    }

    private void LookAtNearestEnemy()
    {
        if(_nearestEnemy != null)
        {
            if(!_nearestEnemy.isActiveAndEnabled) //Avoids the nearest enemy being a disabled enemy
            {
                _nearestEnemy = null;
                return;
            }

            turretTop.LookAt(_nearestEnemy.transform.position);
        }
    }

    private void Attack()
    {
        if(_nearestEnemy != null && !_hasAttacked)
        {
            GameManager.spawnManager.SpawnProjectile(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
            GameManager.audioManager.PlaySound(fireSound);
            GameManager.visualEffectManager.StartVFX(fireEffect);

            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        _hasAttacked = true;
        yield return new WaitForSeconds(attackCooldown);
        _hasAttacked = false;
    }
}
