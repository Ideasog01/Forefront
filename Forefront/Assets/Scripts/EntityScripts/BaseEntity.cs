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

    private Transform _playerCameraTransform;

    private int _damageTimes;

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
        entityHealth -= amount;

        onTakeDamageEvent.Invoke();

        if(entityHealth <= 0)
        {
            onDeathEvent.Invoke();
        }
        else
        {
            _damageTimes = 0;

            if (_damageTimes > 0)
            {
                Invoke("DamageOvertime", 2f);
                _damageTimes--;
            }
        }
    }

    public void DamageOvertime()
    {
        _damageTimes = 3;
        TakeDamage(10);
        Invoke("DamageOvertime", 1f);
    }

    private void Awake()
    {
        _playerCameraTransform = GameObject.Find("Main Camera").transform;
    }
}
