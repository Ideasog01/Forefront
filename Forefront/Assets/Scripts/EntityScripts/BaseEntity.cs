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
    }

    private void Awake()
    {
        _playerCameraTransform = GameObject.Find("Main Camera").transform;
    }
}
