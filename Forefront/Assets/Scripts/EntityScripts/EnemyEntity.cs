using UnityEngine;
using UnityEngine.UI;

public class EnemyEntity : BaseEntity
{
    public enum EnemyType { Drone };

    [Header("Enemy Properties")]

    [SerializeField]
    private EnemyType enemyType;

    [SerializeField]
    private int enemyDamage;

    [Header("Health Display")]

    [SerializeField]
    private Slider enemySlider;

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
        EntityMaxHealth = GameManager.gameSettings.DroneHealth;
        EntityHealth = GameManager.gameSettings.DroneHealth;
        DisplayHealth();
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

        this.gameObject.SetActive(false);
    }
}
