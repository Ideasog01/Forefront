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
            if (_damageTimes > 0)
            {
                GameManager.visualEffectManager.StopVFX(burningVisualEffect);
            }

            onDeathEvent.Invoke();
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
