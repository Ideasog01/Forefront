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

    [Header("Effects")]

    [SerializeField]
    private VisualEffect fireEffect;

    [SerializeField]
    private Sound fireSound;

    [SerializeField]
    private EnemyEntity _nearestEnemy;

    private SpawnManager _spawnManager;

    private bool _hasAttacked;

    private void Start()
    {
        _spawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();
    }

    private void OnEnable()
    {
        _hasAttacked = false; //In case the turret was deactivated during cooldown (coroutine)
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
            if(enemy.gameObject.activeSelf)
            {
                float distance = Vector3.Distance(this.transform.position, enemy.transform.position);

                if(distance < attackThreshold)
                {
                    _nearestEnemy = enemy;
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
            GameManager.spawnManager.SpawnTurretProjectile(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
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
