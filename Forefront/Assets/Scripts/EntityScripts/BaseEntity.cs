using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BaseEntity : MonoBehaviour
{
    [SerializeField]
    private int entityMaxHealth;

    [SerializeField]
    private int entityHealth;

    [SerializeField]
    private UnityEvent onTakeDamageEvent;

    [SerializeField]
    private UnityEvent onDeathEvent;

    [SerializeField]
    private VisualEffect burningVisualEffect;

    private Transform _playerCameraTransform;

    private int _damageTimes;

    private bool _isDead;

    public bool IsDead
    {
        get { return _isDead; }
        set { _isDead = value; }
    }

    public int EntityMaxHealth
    {
        get { return entityMaxHealth; }
        set { entityMaxHealth = value; }
    }

    public int EntityHealth
    {
        get { return entityHealth; }
        set { entityHealth = value; }
    }

    public Transform PlayerCameraTransform
    {
        get { return _playerCameraTransform; }
    }    

    public void TakeDamage(int amount)
    {
        if(!_isDead)
        {
            entityHealth -= amount;

            onTakeDamageEvent.Invoke();

            if (entityHealth <= 0)
            {
                if (_damageTimes > 0)
                {
                    GameManager.visualEffectManager.StopVFX(burningVisualEffect);
                }

                onDeathEvent.Invoke();

                _isDead = true;
            }
            else
            {
                if (_damageTimes > 0)
                {
                    Invoke("DamageOvertime", 1f);
                    _damageTimes--;
                }
            }
        }
    }

    public void Heal(int amount)
    {
        if(entityHealth > 0)
        {
            entityHealth += amount;

            if(entityHealth >= entityMaxHealth)
            {
                entityHealth = entityMaxHealth;
            }

            GameManager.guiManager.DisplayPlayerHealth();
        }
    }

    public void DamageOvertime()
    {
        if(_damageTimes == 0)
        {
            _damageTimes = 3;
            GameManager.visualEffectManager.StartVFX(burningVisualEffect);
        }

        TakeDamage(10);
    }

    private void Awake()
    {
        _playerCameraTransform = GameObject.Find("Main Camera").transform;
    }
}
