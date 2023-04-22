using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExploderEntity : EnemyEntity
{
    [SerializeField]
    private VisualEffect explodeVisualEffect;

    [SerializeField]
    private GameObject enemyMesh;

    private NavMeshAgent _navMeshAgent;

    private bool _explosionActivated;

    private void Start()
    {
        if (GameManager.gameSettings != null)
        {
            EnemyDamage = GameManager.gameSettings.ExploderAttackDamage;
            EntityHealth = GameManager.gameSettings.DroneHealth;
        }

        _navMeshAgent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (InitialTargetLocation != null)
        {
            _navMeshAgent.SetDestination(InitialTargetLocation.position);
            EnemyAnimator.SetBool("isMoving", _navMeshAgent.velocity.magnitude != 0);

            float distanceToTarget = Vector3.Distance(this.transform.position, InitialTargetLocation.position);

            if (distanceToTarget < 0.5f)
            {
                InitialTargetLocation = null;
                DoorAnimator.SetBool("open", false);
            }

            return;
        }

        if (!DisableEnemy && !_explosionActivated)
        {
            _navMeshAgent.SetDestination(PlayerCameraTransform.position);

            CheckDistanceToPlayer();
        }
    }

    private void CheckDistanceToPlayer()
    {
        float distanceToPlayer = Vector3.Distance(this.transform.position, PlayerCameraTransform.position);

        if(distanceToPlayer < AttackThreshold && !_explosionActivated)
        {
            _explosionActivated = true;
            _navMeshAgent.SetDestination(this.transform.position);
            StartCoroutine(DelayExplosion());
        }
    }

    private IEnumerator DelayExplosion()
    {
        yield return new WaitForSeconds(AttackCooldown);
        Explode();
    }

    private void Explode()
    {
        if(EntityHealth > 0)
        {
            float distanceToPlayer = Vector3.Distance(this.transform.position, PlayerCameraTransform.position);

            if (distanceToPlayer < AttackThreshold)
            {
                GameManager.playerEntity.TakeDamage(EnemyDamage);
            }
            
            GameManager.visualEffectManager.StartVFX(explodeVisualEffect);
            TakeDamage(EntityHealth);
        }
    }
}
