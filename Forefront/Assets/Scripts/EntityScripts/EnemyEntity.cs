using UnityEngine;
using UnityEngine.UI;

public class EnemyEntity : BaseEntity
{
    public enum EnemyType { Drone };

    public enum AIState { Idle, Chase, Attack}

    [Header("Enemy Properties")]

    [SerializeField]
    private EnemyType enemyType;

    [SerializeField]
    private AIState aiState;

    [SerializeField]
    private int enemyDamage;

    [SerializeField]
    private float attackThreshold;

    [Header("Health Display")]

    [SerializeField]
    private Slider enemySlider;

    [SerializeField]
    private bool disableEnemy;

    public EnemyType EnemyTypeRef
    { 
        get { return enemyType; }
    }

    public AIState AIStateRef
    {
        get { return aiState; }
        set { aiState = value; }
    }

    public int EnemyDamage
    {
        get { return enemyDamage; }
    }

    public float AttackThreshold
    {
        get { return attackThreshold; }
    }

    public bool DisableEnemy
    {
        get { return disableEnemy; }
        set { disableEnemy = value; }
    }

    public void ResetEnemy()
    {
        EntityMaxHealth = GameManager.gameSettings.DroneHealth;
        EntityHealth = EntityMaxHealth;
        DisplayHealth();
        disableEnemy = false;
    }

    public void DisplayHealth()
    {
        enemySlider.maxValue = EntityMaxHealth;
        enemySlider.value = EntityHealth;
    }

    public void OnEnemyDeath()
    {
        if (CompareTag("Enemy"))
        {
            SpawnManager.activeHostiles--;
        }

        disableEnemy = true;
        this.gameObject.SetActive(false);
    }
}
