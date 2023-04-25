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

    private bool _explosionActivated;

    private void Start()
    {
        if (GameManager.gameSettings != null)
        {
            EnemyDamage = GameManager.gameSettings.ExploderAttackDamage;
            EntityMaxHealth = GameManager.gameSettings.ExploderHealth;
            EntityHealth = EntityMaxHealth;
        }

        EnemyAgent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (InitialTargetLocation != null)
        {
            EnemyAgent.SetDestination(InitialTargetLocation.position);

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

        if (!DisableEnemy && !_explosionActivated)
        {
            EnemyAgent.SetDestination(PlayerCameraTransform.position);

            CheckDistanceToPlayer();
        }
    }

    private void CheckDistanceToPlayer()
    {
        float distanceToPlayer = Vector3.Distance(this.transform.position, PlayerCameraTransform.position);

        if(distanceToPlayer < AttackThreshold && !_explosionActivated)
        {
            _explosionActivated = true;
            EnemyAgent.SetDestination(this.transform.position);
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
