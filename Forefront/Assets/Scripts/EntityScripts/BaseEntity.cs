using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BaseEntity : MonoBehaviour
{
    [SerializeField]
    private int entityHealth;

    [SerializeField]
    private UnityEvent onTakeDamageEvent;

    [SerializeField]
    private UnityEvent onDeathEvent;

    public int EntityHealth
    {
        get { return entityHealth; }
        set { entityHealth = value; }
    }

    public void TakeDamage(int amount)
    {
        entityHealth -= amount;

        if (CompareTag("Enemy"))
        {
            SpawnManager.activeHostiles--;
        }

        this.gameObject.SetActive(false);
    }
}
