using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEntity : EnemyEntity
{
    private float _dodgeCooldown;

    private float _attackCooldown;

    private int _droneDamage;

    private void Awake()
    {
        _dodgeCooldown = GameManager.gameSettings.DroneDodgeCooldown;
        _attackCooldown = GameManager.gameSettings.DroneAttackCooldown;
        _droneDamage = GameManager.gameSettings.DroneDamage;
        EntityHealth = GameManager.gameSettings.DroneHealth;
    }
}
