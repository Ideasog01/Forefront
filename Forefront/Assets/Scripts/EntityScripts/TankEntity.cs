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

    private Transform _projectilePrefab;

    private int _ammo;

    private void Start()
    {
        if (GameManager.gameSettings != null)
        {
            AttackCooldown = GameManager.gameSettings.TankAttackCooldown;
            EntityMaxHealth = GameManager.gameSettings.TankHealth;
            EntityHealth = EntityMaxHealth;
            _projectilePrefab = GameManager.gameSettings.TankProjectilePrefab;
        }

        _ammo = maxAmmo;

        EnemyAgent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(InitialTargetLocation != null)
        {
            EnemyAgent.SetDestination(InitialTargetLocation.position);
            EnemyAnimator.SetBool("isMoving", EnemyAgent.velocity.magnitude != 0);

            this.transform.LookAt(PlayerCameraTransform.transform.position);
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);

            float distanceToTarget = Vector3.Distance(this.transform.position, InitialTargetLocation.position);

            if(distanceToTarget < 0.75f)
            {
                EnemyAgent.stoppingDistance = AttackThreshold - 1; //So the enemy moves into the attack radius
                InitialTargetLocation = null;
                DoorAnimator.SetBool("open", false);
            }

            return;
        }

        if (_projectilePrefab != null && !DisableEnemy && EntityHealth > 0 && GameManager.gameInProgress)
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
                if(EnemyAgent.velocity.magnitude == 0)
                {
                    if (!AttackActivated) //Looped via coroutine
                    {
                        if(_ammo > 0)
                        {
                            EnemyAnimator.SetTrigger("attack");
                        }
                        else
                        {
                            EnemyAnimator.SetTrigger("reload");
                        }

                        AttackActivated = true;
                    }

                    EnemyAnimator.SetBool("isMoving", false);
                }
            }
            else
            {
                AttackActivated = false;
            }

            if (AIStateRef == AIState.Chase)
            {
                EnemyAgent.SetDestination(PlayerCameraTransform.position);
                EnemyAnimator.SetBool("isMoving", true);
            }
        }
        else
        {
            EnemyAnimator.SetBool("isMoving", false);
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
            GameManager.spawnManager.SpawnProjectile(_projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);

            _ammo--;

            StartCoroutine(DelayAttackCooldown());
        }
    }

    private IEnumerator DelayAttackCooldown()
    {
        yield return new WaitForSeconds(AttackCooldown);
        AttackActivated = false; //Causes a looping effect
    }

    public void ReloadEvent()
    {
        _ammo = maxAmmo;
        AttackActivated = false;
    }

    public void PlayDeathAnimation()
    {
        EnemyAnimator.SetTrigger("die");
    }
}
