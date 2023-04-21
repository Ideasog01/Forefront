using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneEntity : EnemyEntity
{
    [SerializeField]
    private Transform[] projectileSpawn;

    private float _attackCooldown;

    private bool _attackActivated;

    private Transform _projectilePrefab;

    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        if(GameManager.gameSettings != null)
        {
            _attackCooldown = GameManager.gameSettings.DroneAttackCooldown;
            EntityHealth = GameManager.gameSettings.DroneHealth;
            _projectilePrefab = GameManager.gameSettings.DroneProjectilePrefab;
        }

        _navMeshAgent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(_projectilePrefab != null && !DisableEnemy)
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
                if (!_attackActivated)
                {
                    StartCoroutine(ProjectileAttack());
                    _attackActivated = true;
                }
            }
            else
            {
                _attackActivated = false;
            }

            if(AIStateRef == AIState.Chase)
            {
                _navMeshAgent.SetDestination(PlayerCameraTransform.position);
            }
        }
    }

    private void AIStateMachine()
    {
        float distanceToPlayer = Vector3.Distance(this.transform.position, PlayerCameraTransform.position);

        if(distanceToPlayer > AttackThreshold)
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
    }

    private IEnumerator ProjectileAttack()
    {
        yield return new WaitForSeconds(_attackCooldown);

        if(GameManager.playerEntity.EntityHealth <= 0)
        {
            AIStateRef = AIState.Idle;
        }
        else
        {
            foreach(Transform transform in projectileSpawn)
            {
                GameManager.spawnManager.SpawnEnemyProjectile(_projectilePrefab, transform.position, transform.rotation);
            }
            
            GameManager.audioManager.PlaySound(AttackSound);

            if (_attackActivated)
            {
                StartCoroutine(ProjectileAttack());
            }
        }
    }
}
