using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAnimations : MonoBehaviour
{
    private TankEntity _tankEntity;

    private void Awake()
    {
        _tankEntity = this.transform.parent.GetComponent<TankEntity>();
    }

    public void AttackAnimationEvent()
    {
        _tankEntity.ProjectileAttackEvent();
    }

    public void ReloadAnimationEvent()
    {
        _tankEntity.ReloadEvent();
    }

    public void DieAnimationEvent()
    {
        _tankEntity.OnEnemyDeath();
    }
}
