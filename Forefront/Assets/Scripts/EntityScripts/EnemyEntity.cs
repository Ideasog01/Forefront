using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyEntity : BaseEntity
{
    public enum EnemyType { Drone, Exploder, Tank };

    public enum AIState { Idle, Chase, Attack}

    [Header("Enemy Properties")]

    [SerializeField]
    private EnemyType enemyType;

    [SerializeField]
    private AIState aiState;

    [SerializeField]
    private Animator enemyAnimator;

    [SerializeField]
    private int enemyDamage;

    [SerializeField]
    private float attackThreshold;

    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private bool attackActivated;

    [Header("Health Display")]

    [SerializeField]
    private Slider enemySlider;

    [SerializeField]
    private bool disableEnemy;

    [Header("Enemy Sounds")]

    [SerializeField]
    private Sound destroySound;

    [SerializeField]
    private Sound attackSound;

    private Animator _doorAnimator;

    private Transform _initialTargetLocation;

    public Transform InitialTargetLocation
    {
        get { return _initialTargetLocation; }
        set { _initialTargetLocation = value; }
    }

    public Animator DoorAnimator
    {
        get { return _doorAnimator; }
        set { _doorAnimator = value; }
    }

    public EnemyType EnemyTypeRef
    { 
        get { return enemyType; }
    }

    public AIState AIStateRef
    {
        get { return aiState; }
        set { aiState = value; }
    }

    public Animator EnemyAnimator
    {
        get { return enemyAnimator; }
    }

    public int EnemyDamage
    {
        get { return enemyDamage; }
        set { enemyDamage = value; }
    }

    public float AttackThreshold
    {
        get { return attackThreshold; }
    }

    public float AttackCooldown
    {
        get { return attackCooldown; }
    }

    public bool AttackActivated
    {
        get { return attackActivated; }
        set { attackActivated = value; }
    }

    public bool DisableEnemy
    {
        get { return disableEnemy; }
        set { disableEnemy = value; }
    }

    public Sound AttackSound
    {
        get { return attackSound; }
    }

    public void ResetEnemy()
    {
        EntityHealth = EntityMaxHealth;
        DisplayHealth();
        disableEnemy = false;
        attackActivated = false;
        IsDead = false;
    }

    public void DisplayHealth()
    {
        enemySlider.maxValue = EntityMaxHealth;
        enemySlider.value = EntityHealth;
    }

    public void OnEnemyDeath()
    {
        SpawnManager.activeHostiles--;
        GameManager.waveManager.hostilesDefeated++;

        Debug.Log(SpawnManager.activeHostiles);

        switch (EnemyTypeRef)
        {
            case EnemyType.Drone:
                GameManager.waveManager.playerScore += GameManager.gameSettings.DroneScoreAmount;
                break;
            case EnemyType.Exploder:
                GameManager.waveManager.playerScore += GameManager.gameSettings.ExploderScoreAmount;
                break;
            case EnemyType.Tank:
                GameManager.waveManager.playerScore += GameManager.gameSettings.TankScoreAmount;
                break;
        }

        GameManager.guiManager.DisplayScore();

        GameManager.audioManager.PlaySound(destroySound);

        disableEnemy = true;
        this.gameObject.SetActive(false);
    }

    public void Disable(float duration)
    {
        if(this.isActiveAndEnabled)
        {
            StartCoroutine(DisableEnemyForSeconds(duration));
        }
    }

    private IEnumerator DisableEnemyForSeconds(float duration)
    {
        disableEnemy = true;
        yield return new WaitForSeconds(duration);
        disableEnemy = false;
    }
}
