using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExploderEntity : EnemyEntity
{
    [SerializeField]
    private VisualEffect explodeVisualEffect;

    [SerializeField]
    private Sound explodeSound;

    [SerializeField]
    private GameObject enemyMesh;

    private NavMeshAgent _navMeshAgent;

    private float _attackDamage;

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
        if (!DisableEnemy)
        {
            _navMeshAgent.SetDestination(PlayerCameraTransform.position);

            CheckDistanceToPlayer();
        }
    }

    private void CheckDistanceToPlayer()
    {
        float distanceToPlayer = Vector3.Distance(this.transform.position, PlayerCameraTransform.position);

        if(distanceToPlayer < AttackThreshold)
        {
            Explode();
        }
    }

    private void Explode()
    {
        GameManager.playerEntity.TakeDamage(EnemyDamage);
        GameManager.visualEffectManager.StartVFX(explodeVisualEffect);
        GameManager.audioManager.PlaySound(explodeSound);
        TakeDamage(EntityHealth);
    }
}
