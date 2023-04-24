using System.Collections;
using UnityEngine;
using UnityEngine.AI;
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

    [SerializeField]
    private Transform _initialTargetLocation;

    private NavMeshAgent _navMeshAgent;

    public Transform InitialTargetLocation
    {
        get { return _initialTargetLocation; }
        set { _initialTargetLocation = value; }
    }

    public NavMeshAgent EnemyAgent
    {
        get { return _navMeshAgent; }
        set { _navMeshAgent = value; }
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
        set { attackCooldown = value; }
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

    public void ResetEnemy(Vector3 newPosition)
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        _navMeshAgent.Warp(newPosition);
        _navMeshAgent.stoppingDistance = 0;

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
        GameManager.waveManager.EnemyDefeated(EnemyTypeRef);

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
