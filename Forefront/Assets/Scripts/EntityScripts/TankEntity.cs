using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankEntity : EnemyEntity
{
    [SerializeField]
    private Transform projectileSpawn;

    [SerializeField]
    private int maxAmmo;

    private float _attackCooldown;

    private bool _attackActivated;

    private Transform _projectilePrefab;

    private NavMeshAgent _navMeshAgent;

    private int _ammo;

    private void Start()
    {
        if (GameManager.gameSettings != null)
        {
            _attackCooldown = GameManager.gameSettings.DroneAttackCooldown;
            EntityHealth = GameManager.gameSettings.DroneHealth;
            _projectilePrefab = GameManager.gameSettings.DroneProjectilePrefab;
        }

        _ammo = maxAmmo;

        _navMeshAgent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_projectilePrefab != null && !DisableEnemy)
        {
            if (AIStateRef != AIState.Idle)
            {
                AIStateMachine();
            }

            if (AIStateRef == AIState.Chase || AIStateRef == AIState.Attack)
            {
                LookAtPlayer();
            }

            if (AIStateRef == AIState.Attack)
            {
                if (!_attackActivated && _ammo > 0) //Looped via coroutine
                {
                    EnemyAnimator.SetTrigger("attack");
                    _attackActivated = true;
                }
            }
            else
            {
                _attackActivated = false;
            }

            if (AIStateRef == AIState.Chase)
            {
                _navMeshAgent.SetDestination(PlayerCameraTransform.position);
            }
        }
    }

    private void AIStateMachine()
    {
        float distanceToPlayer = Vector3.Distance(this.transform.position, PlayerCameraTransform.position);

        if (distanceToPlayer > AttackThreshold)
        {
            AIStateRef = AIState.Chase;
        }
        else
        {
            AIStateRef = AIState.Attack;
        }
    }

    private void LookAtPlayer()
    {
        this.transform.LookAt(PlayerCameraTransform.transform.position);
        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
    }

    public void ProjectileAttackEvent()
    {
        if (GameManager.playerEntity.EntityHealth <= 0)
        {
            AIStateRef = AIState.Idle;
        }
        else
        {
            GameManager.spawnManager.SpawnEnemyProjectile(_projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);

            _ammo--;

            if(_ammo <= 0)
            {
                EnemyAnimator.SetTrigger("reload");
            }

            StartCoroutine(DelayAttackCooldown());
        }
    }

    private IEnumerator DelayAttackCooldown()
    {
        yield return new WaitForSeconds(AttackCooldown);
        _attackActivated = false; //Causes a looping effect
    }

    public void ReloadEvent()
    {
        _ammo = maxAmmo;
    }

    public void PlayDeathAnimation()
    {
        EnemyAnimator.SetTrigger("die");
    }
}
