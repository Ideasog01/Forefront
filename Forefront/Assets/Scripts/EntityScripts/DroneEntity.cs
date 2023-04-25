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

    private void Start()
    {
        if(GameManager.gameSettings != null)
        {
            EntityMaxHealth = GameManager.gameSettings.DroneHealth;
            EntityHealth = EntityMaxHealth;
            _attackCooldown = GameManager.gameSettings.DroneAttackCooldown;
            _projectilePrefab = GameManager.gameSettings.DroneProjectilePrefab;
        }

        EnemyAgent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (InitialTargetLocation != null)
        {
            EnemyAgent.SetDestination(InitialTargetLocation.position);

            this.transform.LookAt(PlayerCameraTransform.transform.position);
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);

            float distanceToTarget = Vector3.Distance(this.transform.position, InitialTargetLocation.position);

            if (distanceToTarget < 0.75f)
            {
                EnemyAgent.stoppingDistance = AttackThreshold - 1; //So the enemy moves into the attack radius
                InitialTargetLocation = null;
                DoorAnimator.SetBool("open", false);
            }

            return;
        }

        if (_projectilePrefab != null && !DisableEnemy && GameManager.gameInProgress)
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
                EnemyAgent.SetDestination(PlayerCameraTransform.position);
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
                GameManager.spawnManager.SpawnProjectile(_projectilePrefab, transform.position, transform.rotation);
            }
            
            GameManager.audioManager.PlaySound(AttackSound);

            if (_attackActivated)
            {
                StartCoroutine(ProjectileAttack());
            }
        }
    }
}
