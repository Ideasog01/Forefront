using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : BaseEntity
{
    public enum EnemyType { Drone };

    [SerializeField]
    private EnemyType enemyType;

    [SerializeField]
    private int enemyDamage;

    public EnemyType EnemyTypeRef
    { 
        get { return enemyType; }
    }

    public int EnemyDamage
    {
        get { return enemyDamage; }
    }

    public void ResetEnemy()
    {
        EntityHealth = GameManager.gameSettings.DroneHealth;
    }
}
